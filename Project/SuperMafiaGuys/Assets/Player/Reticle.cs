using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Reticle : MonoBehaviour
    {
        public UnityEngine.UI.Graphic[] graphics;

        public static bool show = false;

        private bool graphicsEnabled
        {
            get
            {
                return graphics[0].enabled;
            }
            set
            {
                for (int i = 0; i < graphics.Length; i++)
                {
                    var item = graphics[i];

                    item.enabled = value;
                }
            }
        }

        private void Update()
        {
            graphicsEnabled = show;
        }
    }
}