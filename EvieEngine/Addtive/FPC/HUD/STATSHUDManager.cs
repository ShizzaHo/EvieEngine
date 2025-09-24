using EvieEngine.FPC;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

public class STATSHUDManager : MonoBehaviour
{
    public CanvasGroup DamageGroup;
    public CanvasGroup StaminaGroup;
    public CanvasGroup HPGroup;

    public Image HPCounter;          
    public Image StaminaCounter; 

    [Title("Настройки")]
    public float fadeSpeed = 5f;
    public float fillSpeed = 5f;

    [Title("Автоматическое исчезновение")]
    public bool autoHideHP = true;
    public bool autoHideStamina = true;
    public float idleFadeDelayHP = 2f;
    public float idleFadeDelayStamina = 2f;
    public float hiddenAlpha = 0.2f;

    // --- внутреннее для DamageEffect ---
    private float damageAlphaTarget = 0f;
    private float damageTimer = 0f;
    private float damageDuration = 0f;

    // --- внутреннее для HP ---
    private float lastHPValue;
    private float hpIdleTimer;

    // --- внутреннее для Stamina ---
    private float lastStaminaValue;
    private float staminaIdleTimer;

    private void Start()
    {
        // Скрываем StaminaGroup при запуске
        if (StaminaGroup != null)
        {
            StaminaGroup.alpha = 0f;
            StaminaGroup.interactable = false;
            StaminaGroup.blocksRaycasts = false;
        }
    }

    public void DamageEffect(float damage)
    {
        if (DamageGroup == null) return;

        // Определяем длительность эффекта на основе урона
        damageDuration = Mathf.Clamp(damage * 0.1f, 0.5f, 3f);
        damageTimer = 0f;

        // Целевая прозрачность — зависит от урона
        damageAlphaTarget = Mathf.Clamp01(damage / 100f);
        damageAlphaTarget = Mathf.Max(damageAlphaTarget, 0.3f); // Минимум 0.3 для заметности
    }

    private void Update()
    {
        // --- Логика DamageGroup (эффект получения урона) ---
        if (DamageGroup != null)
        {
            if (damageTimer < damageDuration)
            {
                damageTimer += Time.deltaTime;
                float progress = damageTimer / damageDuration;
                // Пульсация: пик в середине
                float pulseAlpha = damageAlphaTarget * (1f - Mathf.Abs(2f * progress - 1f));
                DamageGroup.alpha = Mathf.Lerp(DamageGroup.alpha, pulseAlpha, Time.deltaTime * fadeSpeed * 2f);
            }
            else
            {
                // Плавно скрываем после окончания
                DamageGroup.alpha = Mathf.Lerp(DamageGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
            }
        }

        // --- Логика HP и Stamina (если контроллер доступен) ---
        if (FPCController.Instance != null)
        {
            float targetHP = FPCController.Instance.HP / FPCController.Instance.MaxHP;
            float targetStamina = FPCController.Instance.stamina / FPCController.Instance.staminaMAX;

            LerpFill(HPCounter, targetHP);
            LerpFill(StaminaCounter, targetStamina);

            // --- HP: обновляем таймер только если изменилось здоровье ---
            if (!Mathf.Approximately(targetHP, lastHPValue))
            {
                hpIdleTimer = 0f;
                lastHPValue = targetHP;
            }
            else
            {
                hpIdleTimer += Time.deltaTime;
            }

            // --- Stamina: обновляем таймер только если изменилась стамина ---
            if (!Mathf.Approximately(targetStamina, lastStaminaValue))
            {
                staminaIdleTimer = 0f;
                lastStaminaValue = targetStamina;
            }
            else
            {
                staminaIdleTimer += Time.deltaTime;
            }

            // --- Целевая прозрачность для HP ---
            float hpTargetAlpha = 1f;
            if (autoHideHP && hpIdleTimer > idleFadeDelayHP)
                hpTargetAlpha = hiddenAlpha;

            // --- Целевая прозрачность для Stamina ---
            float staminaTargetAlpha = 1f;
            if (autoHideStamina && staminaIdleTimer > idleFadeDelayStamina)
                staminaTargetAlpha = hiddenAlpha;

            // Применяем независимо
            SetCanvasGroupAlpha(HPGroup, hpTargetAlpha);
            SetCanvasGroupAlpha(StaminaGroup, staminaTargetAlpha);
        }
    }

    private void SetCanvasGroupAlpha(CanvasGroup group, float targetAlpha)
    {
        if (group == null) return;
        group.alpha = Mathf.Lerp(group.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        group.interactable = group.alpha > 0.1f;
        group.blocksRaycasts = group.alpha > 0.1f;
    }

    private void LerpFill(Image image, float targetFill)
    {
        if (image == null) return;
        image.fillAmount = Mathf.Lerp(image.fillAmount, targetFill, Time.deltaTime * fillSpeed);
    }
}