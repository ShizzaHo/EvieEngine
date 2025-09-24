using EvieEngine.FPC;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public Image damageImage;
    public float HP = 100;
    public float MaxHP = 100;
    
    [Header("Regeneration")]
    public bool allowRegenerate = false;
    public float regenerationRate = 1f;     
    public float regenerationDelay = 1f;
    private float regenTimer;

    [Header("Stamina")]
    public float stamina = 100f;
    public float staminaMAX = 100f;
    public float staminaRegenerationRate = 10f;     
    public float staminaRegenerationDelay = 1f;
    private float staminaRegenTimer;

    private void Update()
    {
        HandleRegeneration();
        HandleStamina();
    }

    private void HandleRegeneration()
    {
        if (!allowRegenerate || HP >= MaxHP) return;

        regenTimer += Time.deltaTime;
        if (regenTimer >= regenerationDelay)
        {
            HP = Mathf.Min(HP + regenerationRate * Time.deltaTime, MaxHP);
        }
    }

    private void HandleStamina()
    {
        // если игрок бежит — уменьшаем
        if (FPCController.Instance.isRunning)
        {
            stamina -= 20f * Time.deltaTime; // скорость расхода
            stamina = Mathf.Max(0, stamina);
            staminaRegenTimer = 0;
        }
        else
        {
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenerationDelay)
            {
                stamina = Mathf.Min(stamina + staminaRegenerationRate * Time.deltaTime, staminaMAX);
            }
        }

        // если выносливость на нуле — запрещаем бег
        FPCController.Instance.allowRunning = stamina > 0;
    }

    public void TakeDamage(float damage)
    {
        HP = Mathf.Max(0, HP - damage);
        regenTimer = 0;
    }
}