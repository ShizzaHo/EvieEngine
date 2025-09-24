using System;
using EvieEngine.CameraEffects;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace EvieEngine.FPC
{
    public class FPCController : MonoBehaviour
    {
        // PUBLIC
        [Title("Настройка скоростей")] [HideInInspector]
        public float curretSpeed = 5f;

        public float moveSpeed = 5f;
        public float runSpeed = 10f;
        public float jumpForce = 8f;
        public float groundCheckDistance = 1.1f;

        [Title("Привязка объектов")] 
        public Camera PlayerCamera;

        [Title("Интерактив (анимации)")] 
        public bool interactingWithCamera = true;
        public bool enableJumpAnimateCamera = true;

        [Title("Настройка поведения")] 
        public bool allowMovement = true;
        public bool allowRunning = true;
        public bool allowJump = true;
        
        [Title("Настройка характеристик")]
        
        [Header("Здоровье")]
        public Image damageImage;
        public float HP = 100;
        public float MaxHP = 100;
    
        [Header("Регенерация здоровья")]
        public bool allowRegenerate = false;
        public float regenerationRate = 1f;     
        public float regenerationDelay = 1f;
        private float regenTimer;

        [Header("Стамина")]
        public bool infiniteStamina = false;
        public float stamina = 100f;
        public float staminaMAX = 100f;
        
        [Header("Регенерация стамины")]
        public float staminaRegenerationRate = 10f;     
        public float staminaRegenerationDelay = 1f;
        private float staminaRegenTimer;
        
        [Header("Настройка снижения стамины")]
        public float staminaRunningRate = 1f;     
        public float staminaJumpingRate = 1f;     

        // PRIVATE
        private Rigidbody rb;
        private bool isGrounded;
        [HideInInspector] public bool isRunning;
        
        private STATSHUDManager stats;
        
        // SINGLETONE
        
        public static FPCController Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            
            stats = FindObjectOfType<STATSHUDManager>();
        }

        private void Update()
        {
            CheckGrounded();

            if (allowMovement)
            {
                PlayerMovement();

                if (allowRunning)
                {
                    PlayerRunning();
                }

                if (allowJump)
                {
                    PlayerJump();
                }
            }
            
            RegenerateStamina();
        }
        
        private void RegenerateStamina()
        {
            // Если игрок не бегает и не прыгает — начинаем регенерацию
            if (!isRunning && stamina < staminaMAX)
            {
                // Увеличиваем таймер регенерации
                staminaRegenTimer += Time.deltaTime;

                if (staminaRegenTimer >= staminaRegenerationDelay)
                {
                    // Регенерируем стамину
                    stamina += staminaRegenerationRate * Time.deltaTime;
                    stamina = Mathf.Min(stamina, staminaMAX); // Ограничиваем максимумом
                }
            }
            else
            {
                // Если игрок бегает или стамина полная — сбрасываем таймер
                staminaRegenTimer = 0f;
            }
        }

        private void CheckGrounded()
        {
            RaycastHit hit;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
        }

        private void PlayerMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // --- Горизонтальное движение ---
            Vector3 cameraForward = PlayerCamera.transform.forward;
            Vector3 cameraRight = PlayerCamera.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Получаем ненормализованный вектор движения
            Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal);

            // Нормализуем, если есть движение
            if (moveDirection.sqrMagnitude > 0.001f)
                moveDirection.Normalize();

            // Вычисляем желаемую горизонтальную скорость
            Vector3 desiredHorizontalVelocity = moveDirection * curretSpeed;

            // --- Сохраняем текущую вертикальную скорость (важно!) ---
            float currentYVelocity = rb.linearVelocity.y;

            // --- Если игрок не двигается по горизонтали — плавно затухаем горизонтальную составляющую ---
            if (horizontal == 0 && vertical == 0)
            {
                rb.linearVelocity = new Vector3(
                    Mathf.Lerp(rb.linearVelocity.x, 0f, Time.deltaTime * 10f),
                    currentYVelocity,
                    Mathf.Lerp(rb.linearVelocity.z, 0f, Time.deltaTime * 10f)
                );
            }
            else
            {
                rb.linearVelocity = new Vector3(
                    desiredHorizontalVelocity.x,
                    currentYVelocity,
                    desiredHorizontalVelocity.z
                );
            }
        }


        private void PlayerJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && stamina >= staminaJumpingRate)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (enableJumpAnimateCamera)
                {
                    FPCCamera.Instance.effectsManager.AddEffect(new CameraJumpEffect(1f));
                }
                
                if (!infiniteStamina)
                {
                    stamina -= staminaRunningRate * Time.deltaTime;
                    stamina = Mathf.Max(stamina, 0f);
                }
            }
        }

        private void PlayerRunning()
        {
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && stamina >= staminaRunningRate)
            {
                isRunning = true;
                curretSpeed = runSpeed;

                if (!infiniteStamina)
                {
                    stamina -= staminaJumpingRate * Time.deltaTime;
                    stamina = Mathf.Max(stamina, 0f);
                }
            }
            else
            {
                isRunning = false;
                curretSpeed = moveSpeed;
            }
        }

        public void TakeDamage(float damage)
        {
            HP -= damage;
            FPCCamera.Instance.effectsManager.AddEffect(new CameraDamageEffect(1));
            stats.DamageEffect(damage);
        }
    }
}
