using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    using Combat;
    using UnityEngine.SceneManagement;
    using Utility;

    public class PlayerBehaviour : MonoBehaviour, IDamagable
    {
        public GunManager gunManager;

        public GameObject onHitEffect;

        public Container Health { get; set; }

        private float Fov;
        public float zoomFovChangeAmount = 10;

        public AudioClip[] hurtSounds;

        public void Hurt(float damageToDeal, Vector3 hitPoint, GameObject sender)
        {
            Health.ModifyValue(-damageToDeal);

            AudioSource.PlayClipAtPoint(hurtSounds[Random.Range(0, hurtSounds.Length)], transform.position);

            if (Health.empty)
            {
                // Replace with a game over message in future!
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Instantiate(onHitEffect, hitPoint, Quaternion.Euler(hitPoint - sender.transform.position));
            }
        }

        private void Start()
        {
            Health = new Container(100);
            Fov = Camera.main.fieldOfView;
        }

        private void Update()
        {
            gunManager.fire = Input.GetButton("Fire1");
            Reticle.show = Input.GetButton("Fire2");

            Camera.main.fieldOfView = Reticle.show ? Fov - zoomFovChangeAmount : Fov;

            if (Input.GetButtonDown("Equip"))
            {
                if (gunManager.weaponEquipped)
                    gunManager.EquipWeapon(null);
                else gunManager.EquipWeapon(gunManager.lastEquipedWeapon);
            }

            ForceForward(gunManager.transform);
        }

        private void ForceForward(Transform _transform)
        {
            var rayDist = 1000;
            RaycastHit objectHit;

            var cam = Camera.main.transform;
            Vector3 fwd = cam.TransformDirection(Vector3.forward);

            var lookAt = fwd * rayDist;
            if (Physics.Raycast(cam.position, fwd, out objectHit, rayDist))
            {
                if (objectHit.point != null)
                {
                    if (Vector3.Distance(_transform.position, objectHit.point) < 1)
                    {
                        lookAt = cam.TransformDirection(transform.up) * rayDist;
                    }
                    else
                        lookAt = objectHit.point;
                }
            }
            _transform.LookAt(lookAt);
        }
    }
}