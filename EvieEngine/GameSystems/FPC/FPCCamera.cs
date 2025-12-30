using EvieEngine.CameraEffects;
using TriInspector;
using UnityEngine;

namespace EvieEngine.FPC
{
    public class FPCCamera : MonoBehaviour
    {
        [Title("Настройка чувствительности")] public float mouseSensitivity = 150f;
        public float verticalLookRange = 80f;
        private float xRotation = 0f;

        [Title("Настройка поведения")] public bool allowCameraRotate = true;

        [Title("Настройка анимации камеры")] public bool enableTiltLook = true;
        public bool enableMovementTilt = true;
        public bool enableMovementShake = true;
        public bool enableRunningShake = true;

        [Title("Объект, реагирующий противоположно эффектам камеры")]
        public Transform oppositeReactObject;

        [HideInInspector] public CameraEffectsManager effectsManager;
        
        public static FPCCamera Instance { get; private set; }

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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            effectsManager = gameObject.AddComponent<CameraEffectsManager>();

            if (enableTiltLook)
                AddCameraEffect(new CameraTiltOnLook());

            if (enableMovementTilt)
                AddCameraEffect(new CameraMovementTiltEffect());

            if (enableMovementShake)
                AddCameraEffect(new CameraMovementShakeEffect());

            if (enableRunningShake)
                AddCameraEffect(new CameraRunningShakeEffect());
        }

        private void Update()
        {
            if (allowCameraRotate)
                CameraControl();
        }

        private void LateUpdate()
        {
            // Применяем эффекты после вращения мышью
            transform.localPosition = effectsManager.PositionOffset;
            transform.localRotation *= Quaternion.Euler(effectsManager.RotationOffset);

            // Применяем обратные эффекты к выбранному объекту
            if (oppositeReactObject != null)
            {
                oppositeReactObject.localPosition = -effectsManager.PositionOffset * 0.5f;
                oppositeReactObject.localRotation = Quaternion.Euler(-effectsManager.RotationOffset * 0.5f);
            }
        }

        private void CameraControl()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -verticalLookRange, verticalLookRange);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            float horizontalRotation = transform.parent.rotation.eulerAngles.y + mouseX;
            transform.parent.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        }

        // Вызов для добавления эффекта извне
        public void AddCameraEffect(CameraEffect effect)
        {
            effectsManager.AddEffect(effect);
        }
    }
}
