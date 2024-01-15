using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.Minimap
{
    public class MinimapMarkerDepth : MonoBehaviour
    {

        [SerializeField] private RectTransform rover;
        private RectTransform _rectTransform;
        private Image _image;
        
        private void Start()
        {
            if(!TryGetComponent(out _rectTransform) || 
               !TryGetComponent(out _image)) Destroy(this);
        }

        private void OnEnable()
        {
            StartCoroutine(CheckDistance());
        }

        private IEnumerator CheckDistance()
        {
            yield return new WaitWhile(() => !_rectTransform || !_image);
            while (true)
            {
                if (Vector2.Distance(rover.anchoredPosition,
                        _rectTransform.anchoredPosition) < _rectTransform.localScale.x * 40)
                {
                    SetDepth(0.15f);
                }
                else
                {
                    SetDepth(1);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void SetDepth(float value)
        {
            Color color = _image.color;
            color.a = value;
            _image.color = color;
        }
    }
}