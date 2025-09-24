using TriInspector;
using UnityEngine;
using UnityEngine.UI;

public class GUNHUDManager : MonoBehaviour
{
    [Title("Привязки")]
    public GunInput gunInput;

    public CanvasGroup AMMOGroup;

    public Image AmmoCounter;          // Полная шкала патронов
    public Image CurrentAmmoCounter;   // Текущие патроны в магазине

    [Title("Настройки")]
    public float fadeSpeed = 5f;
    public float fillSpeed = 5f;

    [Title("Автоматическое исчезновение")]
    public bool autoHideAMMO = true;
    public float idleFadeDelay = 2f;     // Время бездействия перед затуханием
    public float hiddenAlpha = 0.2f;     // Минимальная прозрачность при скрытии

    // --- внутреннее ---
    private float lastAmmoValue;
    private float lastCurrentAmmoValue;
    private float ammoIdleTimer;

    private float lastHPValue;
    private float statsIdleTimer;

    private void Update()
    {
        if (gunInput == null || gunInput.equippedGun == null)
            return;

        Gun gun = gunInput.equippedGun;

        float targetCurrentAmmo = (float)gun.currentAmmoInMagazine / gun.magazineSize;
        float targetTotalAmmo = gun.maxAmmo > 0 ? (float)gun.totalAmmo / gun.maxAmmo : 0f;

        // --- Логика для AMMO ---
        LerpFill(CurrentAmmoCounter, targetCurrentAmmo);
        LerpFill(AmmoCounter, targetTotalAmmo);

        if (!Mathf.Approximately(targetCurrentAmmo, lastCurrentAmmoValue) || !Mathf.Approximately(targetTotalAmmo, lastAmmoValue))
        {
            ammoIdleTimer = 0f;
            lastCurrentAmmoValue = targetCurrentAmmo;
            lastAmmoValue = targetTotalAmmo;
        }
        else
        {
            ammoIdleTimer += Time.deltaTime;
        }

        float ammoTargetAlpha = 1f;
        if (autoHideAMMO && ammoIdleTimer > idleFadeDelay)
            ammoTargetAlpha = hiddenAlpha;

        SetCanvasGroupAlpha(AMMOGroup, ammoTargetAlpha);
        
        float statsTargetAlpha = 1f;
    }

    private void SetCanvasGroupAlpha(CanvasGroup group, float targetAlpha)
    {
        if (group == null) return;
        group.alpha = Mathf.Lerp(group.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }

    private void LerpFill(Image image, float targetFill)
    {
        if (image == null) return;
        image.fillAmount = Mathf.Lerp(image.fillAmount, targetFill, Time.deltaTime * fillSpeed);
    }
}
