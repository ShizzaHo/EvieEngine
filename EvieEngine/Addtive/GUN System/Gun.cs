using UnityEngine;
using EvieEngine.FPC;
using EvieEngine.CameraEffects;

public abstract class Gun : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName = "Unnamed Weapon";
    public Bullet bulletType;
    
    [HideInInspector]
    public bool isReloading = false;

    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int currentAmmoInMagazine = 30;
    public int totalAmmo = 90;
    public int maxAmmo = 120;
    public float reloadTime = 0;

    [Header("Fire Settings")]
    public float fireRate = 0.1f;
    public bool allowSpam = true;

    [Header("Recoil Settings")]
    public Vector2 recoilPerShot = new Vector2(2f, 1f);
    public float recoilReturnSpeed = 8f;

    protected Camera cam;
    private float nextFireTime = 0f;

    private CameraRecoilEffect recoilEffect;
    [HideInInspector]
    public HandAnimationRegister animation;

    protected virtual void Start()
    {
        cam = GetComponentInParent<Camera>();
        animation = GetComponentInParent<HandAnimationRegister>();

        var fpcCam = GetComponentInParent<FPCCamera>();
        if (fpcCam != null)
        {
            recoilEffect = new CameraRecoilEffect(Vector2.zero, recoilReturnSpeed);
            fpcCam.AddCameraEffect(recoilEffect);
        }
    }

    private void Update()
    {
        
    }

    public void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        if (currentAmmoInMagazine > 0)
        {
            Shoot();
            currentAmmoInMagazine--;
            nextFireTime = Time.time + fireRate;

            ApplyRecoil();
        }
        else
        {
            OnEmptyMagazine();
        }
    }

    private void ApplyRecoil()
    {
        if (recoilEffect != null)
        {
            recoilEffect.AddRecoil(new Vector2(
                Random.Range(recoilPerShot.x * 0.8f, recoilPerShot.x * 1.2f),
                Random.Range(-recoilPerShot.y, recoilPerShot.y)
            ));
        }
    }

    public void Reload()
    {
        if (isReloading)
            return;

        isReloading = true;
        
        int needed = magazineSize - currentAmmoInMagazine;
        int toLoad = Mathf.Min(needed, totalAmmo);

        animation.PlayAnimationWithCallback("reload", () =>
        {
            currentAmmoInMagazine += toLoad;
            totalAmmo -= toLoad;
            
            isReloading = false;
        }, reloadTime);
    }

    protected abstract void Shoot();

    protected virtual void OnReload()
    {

    }
    protected virtual void OnEmptyMagazine() { }
}
