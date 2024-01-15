using System;
using UnityEngine;
using UnityEngine.Events;

namespace ITPRO.Rover.Minimap
{
    public class MinimapMarker : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private bool scaling;
        [SerializeField] private float multiplyScaling = 1;
    
        private static Transform _roverWithoutXZRotation;
        private Vector3 _startScale;
        
        #region MonoBehaviour
        private void OnEnable()
        {
            MinimapCamera.onUpdateCameraState += OnCameraSizeChanged;
        }
    
        private void Start()
        {
            _roverWithoutXZRotation = new GameObject().transform;
            _startScale = rectTransform.localScale;
        }
    
        private void OnDisable()
        {
            MinimapCamera.onUpdateCameraState -= OnCameraSizeChanged;
        }
        #endregion
    
        private void OnCameraSizeChanged(float cameraSize)
        {
            Vector3 newPosition = Vector2.zero;
    
            Vector3 targetDirection = _roverWithoutXZRotation.InverseTransformPoint(target.position);
    
            newPosition.x = targetDirection.x;
            newPosition.y = targetDirection.z;
    
            rectTransform.localPosition = newPosition * 100 / (cameraSize / 2);
            if (scaling)
            {
                float dif = (cameraSize - 10) / 850 * multiplyScaling;
                if (dif > 0.6f)
                    dif = 0.6f;
                
                rectTransform.localScale = _startScale * (1 - dif);
            }
        }
    
        public static void SetRoverWithoutXZRotation(Transform roverTransform)
        {
            _roverWithoutXZRotation.SetPositionAndRotation(roverTransform.position,
                Quaternion.Euler(0, roverTransform.eulerAngles.y, 0));
        }
    }
}