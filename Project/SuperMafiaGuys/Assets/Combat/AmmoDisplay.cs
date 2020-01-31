using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class AmmoDisplay : MonoBehaviour
    {
        public UnityEngine.UI.Text text;

        public GunManager weapon;

        private void Update()
        {
            var ammoCount = 0;

            weapon.GetAmmo(out ammoCount);

            var ammoText = "";
            if (ammoCount > 0)
            {
                ammoText = $"{ammoCount}w";
            }

            text.text = ammoText;
        }
    }
}