using UnityEngine;
using UnityEngine.UI;

namespace ITPRO.Rover.Minimap
{
    public class MinimapNoises : MonoBehaviour
    {
        [System.Serializable]
        private struct ResolutionOnDistance
        {
            public int distance;
            [SerializeField] private RenderTexture renderTexture;

            public RenderTexture ActivateResolution()
            {
                return renderTexture;
            }
        }

        [SerializeField] private Transform target;
        [SerializeField] private Transform centerOfWorld;
        [SerializeField] private ResolutionOnDistance[] resolutionOnDistances;
        [SerializeField] private RawImage minimapImage;
        [SerializeField] private MinimapCamera minimapCamera;

        private float _distance;
        private int _activeResolution;

        private void FixedUpdate()
        {
            _distance = Vector3.Distance(target.position, centerOfWorld.position);
            UpdateResolution();
        }

        private void UpdateResolution()
        {
            for (int i = resolutionOnDistances.Length - 1; i >= 0; i--)
            {
                if (!(_distance > resolutionOnDistances[i].distance)) continue;
                if (_activeResolution == i) return;

                _activeResolution = i;
                RenderTexture renderTexture = resolutionOnDistances[i].ActivateResolution();
                minimapCamera.SetRenderTexture(renderTexture);
                minimapImage.texture = renderTexture;
                return;
            }
        }
    }
}