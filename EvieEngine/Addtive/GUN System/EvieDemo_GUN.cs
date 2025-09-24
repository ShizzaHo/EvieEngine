using EvieEngine.FPC;
using UnityEngine;
using System.Collections;

public class EvieDemo_GUN : Gun
{
    private AudioSource audio;

    public AudioClip shot;
    public GameObject muzzleFlash;
    public float muzzleFlashDuration = 0.05f; // Длительность вспышки

    public void Awake()
    {
        audio = GetComponentInChildren<AudioSource>();
    }

    protected override void Shoot()
    {
        FPCCamera.Instance.AddCameraEffect(new CameraShake(0.2f, 0.2f));

        // Спавн пули
        GameObject bulletGO = ObjectPool.Instance.Spawn(
            "Bullet",
            cam.transform.position + cam.transform.forward * 0.5f,
            cam.transform.rotation
        );

        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Shoot(
            cam.transform.position + cam.transform.forward * 0.5f,
            cam.transform.forward
        );

        // Воспроизведение звука
        audio.PlayOneShot(shot);

        // Включаем муззлфлеш
        if (muzzleFlash != null)
            StartCoroutine(ShowMuzzleFlash());
    }

    private IEnumerator ShowMuzzleFlash()
    {
        // Случайный поворот по X
        muzzleFlash.transform.localRotation = Quaternion.Euler(
            Random.Range(0f, 360f),
            muzzleFlash.transform.localRotation.eulerAngles.y,
            muzzleFlash.transform.localRotation.eulerAngles.z
        );

        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(muzzleFlashDuration);

        muzzleFlash.SetActive(false);
    }

    protected override void OnReload()
    {

    }

    protected override void OnEmptyMagazine()
    {

    }
}