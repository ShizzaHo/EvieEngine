using UnityEngine;
using System.Collections.Generic;
using EvieEngine.FPC;

namespace EvieEngine.GUNSystem
{
    public class GunInput : MonoBehaviour
    {
        [Header("Inventory")] public List<Gun> gunInventory = new List<Gun>();
        public Gun equippedGun;

        private int currentGunIndex = 0;
        private bool canShoot = true;
        private bool isReloading = false;

        private void Start()
        {
            if (gunInventory.Count > 0)
                EquipGun(currentGunIndex, instant: true);
        }

        private void Update()
        {
            if (equippedGun != null && canShoot && !isReloading && FPCController.Instance.isRunning == false)
            {
                HandleShooting();
            }

            HandleReloading();
            HandleWeaponSwitching();
        }

        private void HandleShooting()
        {
            if (equippedGun.allowSpam)
            {
                if (Input.GetMouseButton(0))
                    equippedGun.TryShoot();
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    equippedGun.TryShoot();
            }
        }

        private void HandleReloading()
        {
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && equippedGun != null)
            {
                equippedGun.Reload();

                isReloading = true;
                canShoot = false;

                Invoke(nameof(FinishReloading), equippedGun.reloadTime);
            }
        }

        private void FinishReloading()
        {
            isReloading = false;
            canShoot = true;
        }

        private void HandleWeaponSwitching()
        {
            if (isReloading) return; // запрещаем менять оружие во время перезарядки

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
                SelectNextGun();
            else if (scroll < 0f)
                SelectPreviousGun();
        }

        private void EquipGun(int index, bool instant = false)
        {
            if (gunInventory.Count == 0 || index < 0 || index >= gunInventory.Count)
                return;

            canShoot = false;

            Gun newGun = gunInventory[index];

            if (!instant && equippedGun != null)
            {
                // Проигрываем hide на старом оружии
                var oldAnim = equippedGun.animation;
                if (oldAnim != null)
                {
                    oldAnim.PlayAnimationWithCallback("hide", () => { ActivateGun(newGun, index); },
                        oldAnim.hideDuration);
                }
                else
                {
                    ActivateGun(newGun, index);
                }
            }
            else
            {
                ActivateGun(newGun, index, instant: true);
            }
        }

        private void ActivateGun(Gun newGun, int index, bool instant = false)
        {
            foreach (Gun gun in gunInventory)
            {
                if (gun != null)
                    gun.gameObject.SetActive(false);
            }

            equippedGun = newGun;
            equippedGun.gameObject.SetActive(true);
            currentGunIndex = index;

            var anim = equippedGun.animation;

            if (!instant && anim != null)
            {
                anim.PlayAnimationWithCallback("show", () => { canShoot = true; }, anim.showDuration);
            }
            else
            {
                canShoot = true; // сразу можно стрелять
            }
        }

        private void SelectNextGun()
        {
            if (isReloading) return; // блокируем смену
            if (gunInventory.Count == 0) return;

            int nextIndex = (currentGunIndex + 1) % gunInventory.Count;
            EquipGun(nextIndex);
        }

        private void SelectPreviousGun()
        {
            if (isReloading) return; // блокируем смену
            if (gunInventory.Count == 0) return;

            int prevIndex = (currentGunIndex - 1 + gunInventory.Count) % gunInventory.Count;
            EquipGun(prevIndex);
        }

        public void ToggleWeaponVisibility()
        {
            if (equippedGun != null)
            {
                bool isActive = equippedGun.gameObject.activeSelf;
                equippedGun.gameObject.SetActive(!isActive);
            }
        }
    }
}