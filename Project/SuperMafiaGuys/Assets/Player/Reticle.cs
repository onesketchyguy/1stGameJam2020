using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Reticle : MonoBehaviour
    {
        public UnityEngine.UI.Image image;

        public static bool show = false;

        private void Update()
        {
            image.enabled = show;
        }
    }
}