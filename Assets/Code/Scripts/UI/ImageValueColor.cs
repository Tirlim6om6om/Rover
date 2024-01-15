using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.UI
{
    public class ImageValueColor : MonoBehaviour
    {
        private Image image;

        private void OnEnable()
        {
            TryGetComponent(out image);
            StartCoroutine(SwitchColor());
        }

        private IEnumerator SwitchColor()
        {
            while (true)
            {
                float value = image.fillAmount;
                Color color = image.color;
                if (value > 0.5f)
                {
                    float localValue = (value - 0.5f)/0.5f;
                    color.r = 1 - localValue;
                    color.g = 1;
                }
                else
                {
                    float localValue = (value)/0.5f;
                    color.r = 1;
                    color.g = localValue;
                }
                image.color = color;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}