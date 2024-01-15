using System;
using System.Collections;
using UnityEngine;

namespace ITPRO.Rover.Minerals
{
    public class HighlightMineral : MonoBehaviour
    {
        private RoboticArm.RoboticArm arm;
        private MeshRenderer _mesh;
        private bool _active;
        
        private void Start()
        {
            arm = RoboticArm.RoboticArm.instance;
            arm.CollectActive += SetActiveHighliht;
            TryGetComponent(out _mesh);
            _active = true;
            StartCoroutine(Highlight());
        }

        public void SetActiveHighliht(bool active)
        {
            _active = !active;
            if (_active)
            {
                StartCoroutine(Highlight());
            }
            else
            {
                _mesh.material.SetColor("_EmissionColor", 0*Color.red);
                StopCoroutine(Highlight());
            }
        }

        private void OnDestroy()
        {
            arm.CollectActive -= SetActiveHighliht;
            _active = false;
        }

        private IEnumerator Highlight()
        {
            bool up = true;
            float value = 0;
            while (_active)
            {
                if (up)
                {
                    value += 0.0015f;
                    if (value > 0.3f)
                    {
                        up = false;
                    }
                }
                else
                {
                    value -= 0.0015f;
                    if (value < 0)
                    {
                        up = true;
                    }
                }
                _mesh.material.SetColor("_EmissionColor", value*Color.red);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}