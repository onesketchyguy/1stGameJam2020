using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class DisplayHealth : MonoBehaviour
    {
        public PlayerBehaviour player;

        public Graphic graphic;

        private void Start()
        {
            player = FindObjectOfType<PlayerBehaviour>();
        }

        private void Update()
        {
            if ((Text)graphic)
            {
                var text = (Text)graphic;

                text.text = $"H{player.Health.CurrentValue}";
            }
        }
    }
}