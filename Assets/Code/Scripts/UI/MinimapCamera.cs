using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ITPRO.Rover.Minimap
{
    public class MinimapCamera : MonoBehaviour
    {
        public static UnityAction<float> onUpdateCameraState;

        [Header("Position")] [SerializeField] private Transform targetObject;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool rotation;

        [Header("Minimap Camera Settings")] [SerializeField]
        private Camera cameraMap;

        [SerializeField] private Vector2 sizeRange = new Vector2(10, 850);
        [SerializeField] private float resizePower = 1;
        [SerializeField] private InputActionProperty sizeAction;
        
        [SerializeField] private TextMeshProUGUI textSmallCircle;
        [SerializeField] private TextMeshProUGUI textBigCircle;

        private Coroutine _sizeCoroutine;

        private void Start()
        {
            sizeAction.action.performed += GetValue;
            sizeAction.action.canceled += GetValue;
            IEnumerator StartMax()
            {
                yield return new WaitForSeconds(0.1f);
                SetSize(sizeRange.y);
            }
            StartCoroutine(StartMax());
        }
        

        private void Update()
        {
            if (transform.position != targetObject.position + offset)
            {
                transform.position = targetObject.position + offset;
                MinimapMarker.SetRoverWithoutXZRotation(targetObject);
                onUpdateCameraState?.Invoke(cameraMap.orthographicSize);
            }

            if (rotation)
            {
                transform.rotation = Quaternion.LookRotation(transform.forward, targetObject.forward);
            }
        }

        private void GetValue(InputAction.CallbackContext context)
        {
            float actionValue = context.ReadValue<Vector2>().y;

            _sizeCoroutine = actionValue != 0 ? StartCoroutine(UpdateSize(actionValue)) : null;
        }

        private IEnumerator UpdateSize(float value)
        {
            yield return null;
            while (_sizeCoroutine != null)
            {
                float orthographicSize = cameraMap.orthographicSize;
                orthographicSize += value * resizePower * Time.deltaTime;
                SetSize(orthographicSize);
                yield return null;
            }
        }

        private void SetSize(float orthographicSize)
        {
            cameraMap.orthographicSize = Mathf.Clamp(orthographicSize, sizeRange.x, sizeRange.y);
            onUpdateCameraState?.Invoke(cameraMap.orthographicSize);
            textSmallCircle?.SetText(Mathf.Round(cameraMap.orthographicSize/4*1.5f).ToString() +" м");
            textBigCircle?.SetText(Mathf.Round(cameraMap.orthographicSize/4*3f).ToString() +" м");
        }

        public void SetRenderTexture(RenderTexture renderTexture) => cameraMap.targetTexture = renderTexture;
    }
}