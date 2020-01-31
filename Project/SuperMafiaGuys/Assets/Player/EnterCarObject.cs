using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Player
{
    public class EnterCarObject : MonoBehaviour
    {
        public MonoBehaviour[] objects;
        public CarAudio audio;

        private MeshRenderer ren;

        public GameObject cam;

        private Vector3 camOffset;

        private GameObject player;

        private bool playerInCar = false;

        private void Start()
        {
            ren = GetComponent<MeshRenderer>();

            cam.SetActive(false);
            camOffset = cam.transform.localPosition;
            foreach (var item in objects)
            {
                item.enabled = false;
            }
        }

        private void Update()
        {
            if (player != null)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    if (!playerInCar)
                    {
                        EnterCar();
                    }
                    else
                    {
                        ExitCar();
                    }
                }
            }

            ren.enabled = !playerInCar;
        }

        private void EnterCar()
        {
            playerInCar = true;

            // Get player in car
            foreach (var item in objects)
            {
                item.enabled = true;
            }

            audio.StartSound();

            player.transform.SetParent(transform);
            player.transform.position = player.transform.position + (Vector3.up * 0.1f);
            player.SetActive(false);

            cam.transform.SetParent(null);
            cam.SetActive(true);
        }

        private void ExitCar()
        {
            playerInCar = false;

            audio.StopSound();

            // Get player in car
            foreach (var item in objects)
            {
                item.enabled = false;
            }

            cam.SetActive(false);
            cam.transform.SetParent(objects[0].transform);
            cam.transform.localPosition = camOffset;

            player.transform.SetParent(null);
            player.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                player = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player" && !playerInCar)
            {
                player = null;
            }
        }
    }
}