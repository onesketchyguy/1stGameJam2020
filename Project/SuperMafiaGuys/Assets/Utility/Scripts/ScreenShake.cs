using System.Collections;
using UnityEngine;

namespace Utility
{
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake instance;

        private void Awake()
        {
            instance = this;
        }

        public IEnumerator Shake(float shakeAmount, float shakeTime)
        {
            var startPos = Camera.main.transform.position;
            shakeTime += Time.time;

            while (Time.time < shakeTime)
            {
                Camera.main.transform.position = startPos + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount));
                yield return null;
            }
        }
    }
}