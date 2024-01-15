using System;
using System.Collections;
using System.Security.Cryptography;
using ITPRO.Rover;
using ITPRO.Rover.Controller;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITPRO.Rover.RoboticArm
{
    [System.Serializable]
    public class RotatingArm
    {
        public Transform pos;
        public HingeJoint joint;
        public float speed;
        public float max;
        public float min;
        [HideInInspector] public float rot;
    }
    
    public class RoboticArm : MonoBehaviour
    {
        [SerializeField] private RoverController rover;
        [SerializeField] private CameraController camerasController;
        [SerializeField] private PalmController palm;
        [SerializeField] private RotatingArm[] poses;
        public delegate void CollectMode(bool active);
        public event CollectMode CollectActive;
        public static RoboticArm instance;

        private bool _active;
        private bool _waitOpen;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            JoystickInput.joy.Controller.Collect_mode.performed += context => Activate(context);
            JoystickInput.joy.Controller.Trigger.performed += context => CheckPressTrigger(context);
        }

        private void Activate(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() < 0.5f) return;
            
            _active = !_active;
            rover.collectMode = _active;
            camerasController.collectMode = _active;
            camerasController.SetActiveRoboticCam(_active);
            CollectActive?.Invoke(_active);
            if (!_active)
            {
                
                foreach (var pos in poses)
                {
                    if (pos.joint != null)
                    {
                        JointMotor motor = new JointMotor();
                        motor.force = 5;
                        motor.targetVelocity = 0;
                        pos.joint.motor = motor;
                    }
                }
            }
        }
        
        private void CheckPressTrigger(InputAction.CallbackContext context)
        {
            if(!_active) return;
            if (context.ReadValue<float>() > 0.5f)
            {
                palm.Trigger();
                // palm.Close();
                // _waitOpen = true;
                // StartCoroutine(CheckOpenPalm());
            }
        }

        IEnumerator CheckOpenPalm()
        {
            while (_waitOpen)
            {
                if (JoystickInput.button.Trigger.ReadValue<float>() < 0.5f)
                {
                    palm.Open();
                    _waitOpen = false;
                    break;
                }
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void FixedUpdate()
        {
            if(!_active) return;
            MoveJoystick();
            MoveHuts();
        }

        private void MoveJoystick()
        {
            Vector2 joy = JoystickInput.joy.Controller.Axis.ReadValue<Vector2>();
            
            //Поворот 1 части влево-вправо
            float speed = poses[0].speed * joy.x * Time.fixedDeltaTime;
            JointMotor motor = poses[0].joint.motor;
            motor.targetVelocity = speed*100;
            motor.force = 5;
            poses[0].joint.motor = motor;

            //Поворо 2 части вверх-вниз
            speed = poses[1].speed * joy.y * Time.fixedDeltaTime;
            motor = poses[1].joint.motor;
            motor.targetVelocity = speed*100;
            motor.force = 5;
            poses[1].joint.motor = motor;
        }

        private void MoveHuts()
        {
            Vector2 hut_3 = JoystickInput.joy.Controller.Hut_3.ReadValue<Vector2>();
            Vector2 hut_2 = JoystickInput.joy.Controller.Hut_2.ReadValue<Vector2>();
            
            //Поворот 3 части вверх-вниз
            float speed = -(poses[2].speed * hut_3.y * Time.fixedDeltaTime);
            JointMotor motor = poses[2].joint.motor;
            motor.targetVelocity = speed*100;
            motor.force = 5;
            poses[2].joint.motor = motor;
            

            //Поворот 4 части вверх-вниз
            float speedWrist = -(poses[3].speed * hut_2.y * Time.fixedDeltaTime);
            JointMotor motorWrist = poses[3].joint.motor;
            motorWrist.targetVelocity = speedWrist*100;
            motorWrist.force = 5;
            poses[3].joint.motor = motorWrist;
        }
    }
}
