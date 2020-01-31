using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Combat
{
    public class GunManager : MonoBehaviour
    {
        public List<WeaponAmmo> ammoTypes = new List<WeaponAmmo>();

        public bool fire;

        private bool shooting;

        public GameObject hitWallEffect;

        public bool autoEquipWeapon = true;
        public bool consumesAmmo = false;

        [SerializeField]
        private List<WeaponData> weapons = new List<WeaponData>();

        public void GetAmmo(out int ammo)
        {
            ammo = -1;

            if (equipedWeapon == null) return;

            foreach (var item in ammoTypes)
            {
                if (item.type == equipedWeapon.ammoType)
                    ammo = item.count;
            }
        }

        public void AddWeapon(WeaponData weapon)
        {
            if (weapons.Contains(weapon))
            {
                AddAmmo(weapon.ammoType, Random.Range(5, 100));
            }
            else
            {
                weapons.Add(weapon);
            }

            if (equipedWeapon == null || weapon.fireRate > equipedWeapon.fireRate)
            {
                EquipWeapon(weapon);
            }
        }

        internal WeaponObject CurrentWeapon;
        internal WeaponData equipedWeapon;
        internal WeaponData lastEquipedWeapon;

        public bool weaponEquipped
        {
            get
            {
                return equipedWeapon != null;
            }
        }

        internal float GetFireRate()
        {
            if (equipedWeapon == null) return 1;

            var value = equipedWeapon.fireRate / 60;

            return (value * 10) * Time.deltaTime;
        }

        public Transform gunSlot;

        private void Start()
        {
            foreach (var item in System.Enum.GetValues(typeof(AmmoType)))
            {
                ammoTypes.Add(new WeaponAmmo((AmmoType)item, 100));
            }

            if (autoEquipWeapon)
                EquipWeapon(weapons[0]);
        }

        public void EquipWeapon(WeaponData newWeapon)
        {
            // Remove old weapon
            foreach (var item in gunSlot.GetComponentsInChildren<Transform>())
            {
                if (item == gunSlot) continue;
                Destroy(item.gameObject);
            }

            lastEquipedWeapon = equipedWeapon;

            if (newWeapon == null)
            {
                // Unequip weapon
                equipedWeapon = null;
                CurrentWeapon = null;

                return;
            }

            // Add new weapon
            var go = Instantiate(newWeapon.weaponMesh, gunSlot);

            CurrentWeapon = go.GetComponent<WeaponObject>();

            // set the weapon
            equipedWeapon = newWeapon;
        }

        private void Update()
        {
            if (CurrentWeapon != null)
            {
                if (fire)
                {
                    float fireRate = GetFireRate();

                    if (shooting == false)
                    {
                        shooting = true;
                        InvokeRepeating(nameof(ShootWeapon), fireRate * Time.deltaTime, fireRate);
                    }
                }
                else
                {
                    if (shooting == true)
                    {
                        shooting = false;
                        CancelInvoke(nameof(ShootWeapon));
                    }
                }
            }
        }

        private bool ConsumeAmmo()
        {
            foreach (var item in ammoTypes)
            {
                if (item.type == equipedWeapon.ammoType && item.count > 0)
                {
                    item.count--;

                    return true;
                }
            }

            return false;
        }

        public void AddAmmo(AmmoType type, int count)
        {
            foreach (var item in ammoTypes)
            {
                if (item.type == type)
                {
                    item.count += count;
                }
            }
        }

        public void ShootWeapon()
        {
            // create projectile and shoot
            // Create a ray to shoot out

            bool canFire = true;

            if (consumesAmmo)
                canFire = ConsumeAmmo();

            if (canFire == false) return;

            CurrentWeapon.WeaponShot();

            RaycastHit objectHit;
            var rayDist = 100;

            Vector3 fwd = CurrentWeapon.bulletDir.transform.TransformDirection(-Vector3.forward + (Vector3)(Random.insideUnitCircle * 0.01f));
            if (Physics.Raycast(CurrentWeapon.bulletDir.transform.position, fwd, out objectHit, rayDist))
            {
                //do something if hit object ie
                if (objectHit.transform.gameObject.layer == LayerMask.NameToLayer("Damagable"))
                {
                    var enemy = objectHit.transform.gameObject.GetComponent<IDamagable>();
                    if (enemy != null)
                    {
                        if (objectHit.point.y > objectHit.transform.position.y + 0.75f)
                        {
                            enemy.Hurt(Random.Range(enemy.Health.MaxValue / 2, enemy.Health.MaxValue), objectHit.point, gameObject);
                        }
                        else
                            enemy.Hurt(Random.Range(1, 5), objectHit.point, gameObject);
                    }
                }
                else
                {
                    Instantiate(hitWallEffect, objectHit.point, Quaternion.identity);
                }

                FindObjectOfType<GameAI.AgentManager>().HearGunShot(objectHit.point, 10);
            }

            FindObjectOfType<GameAI.AgentManager>().HearGunShot(transform.position, 10);
        }

        private void OnDrawGizmos()
        {
            if (CurrentWeapon != null)
            {
                Vector3 fwd = CurrentWeapon.bulletDir.transform.TransformDirection(-Vector3.forward);

                Debug.DrawRay(CurrentWeapon.bulletDir.transform.position, fwd * 1000, Color.red);
            }
        }
    }
}