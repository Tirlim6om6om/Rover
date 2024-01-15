using System;
using System.Collections;
using ITPRO.Rover.RoboticArm;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITPRO.Rover.Container
{
    [System.Serializable]
    public class Container
    {
        [SerializeField]private Transform pos;
        [SerializeField]private CollisionDetect detectArm;
        [SerializeField]private float coord;
        [SerializeField]private float min;
        [SerializeField]private float max;
        [SerializeField]private float speed;
        private bool _active;
        private bool _moving;

        public void SwitchMove()
        {
            if(_moving) return;
            _active = !_active;
        }

        public IEnumerator Moving()
        {
            if (_moving)  yield break;
            _moving = true;
            if (_active)
            {
                while (Move(speed))
                {
                    yield return new WaitForSeconds(0.025f);
                }
            }
            else
            {
                while (Move(-speed))
                {
                    yield return new WaitForSeconds(0.025f);
                }
            }
            _moving = false;
        }
        
        private bool Move(float speedV)
        {
            if (detectArm.GetActive()) return true;
            if (coord + speedV >= min && coord + speedV <= max)
            {
                pos.transform.localPosition += new Vector3(0,0,1) * speedV;
                coord += speedV;
                return true;
            }
            return false;
        }
    }
    
    public class ContainerController : MonoBehaviour
    {
        [SerializeField] private Container small;
        [SerializeField] private Container medium;
        [SerializeField] private Container big;
        
        private void Start()
        {
            JoystickInput.button.Container_1.performed += context => MoveSmall(context);
            JoystickInput.button.Container_2.performed += context => MoveMedium(context);
            JoystickInput.button.Container_3.performed += context => MoveBig(context);
        }

        public void MoveSmall(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                small.SwitchMove();
                StartCoroutine(small.Moving());
            }
        }
        
        public void MoveMedium(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                medium.SwitchMove();
                StartCoroutine(medium.Moving());
            }
        }
        
        public void MoveBig(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                big.SwitchMove();
                StartCoroutine(big.Moving());
            }
        }
    }
}
