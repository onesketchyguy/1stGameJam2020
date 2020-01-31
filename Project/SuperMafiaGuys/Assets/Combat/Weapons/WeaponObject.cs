using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WeaponObject : MonoBehaviour
    {
        public Transform bulletDir;

        public GameObject MuzzleFlash;
        public Transform muzzleSpawnPoint;

        public GameObject shell;
        public Transform shellSpawnPoint;

        public AudioSource audioSource;

        public void WeaponShot()
        {
            audioSource.Play();

            if (MuzzleFlash == null || shell == null) return;
            var muz = Instantiate(MuzzleFlash, muzzleSpawnPoint.position, muzzleSpawnPoint.rotation);
            muz.transform.localScale = transform.localScale;
            var shel = Instantiate(shell, shellSpawnPoint.position, shellSpawnPoint.rotation);
            shel.transform.localScale = transform.localScale;
        }
    }
}