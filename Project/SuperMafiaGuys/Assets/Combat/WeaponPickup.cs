using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponPickup : MonoBehaviour
    {
        public WeaponData weapon;

        private void OnValidate()
        {
            var col = GetComponent<Collider>();

            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void Start()
        {
            Instantiate(weapon.weaponMesh, transform);

            StartCoroutine(rotate());
        }

        private IEnumerator rotate()
        {
            while (true)
            {
                transform.Rotate(Vector3.up, 30 * Time.deltaTime);
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;

            var otherInventory = other.GetComponentInChildren<GunManager>();
            otherInventory.AddWeapon(weapon);

            Destroy(gameObject);
        }
    }
}