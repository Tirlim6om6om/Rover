using System;
using ITPRO.Rover.User;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITPRO.Rover.Controller
{
    public enum States
    {
        stay,
        acceleration,
        move,
        breaking
    }
        
    
    [RequireComponent(typeof(Rigidbody))]
    public class RoverController : MonoBehaviour
    {
        public delegate void SwitchStates(States state, float speed); 
        public event SwitchStates StateEvent;

        public delegate void Junction();
        public event Junction OutJunction;
        
        public bool collectMode;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float brakeAcceleration;
        [Tooltip("Сила, с который ровер движется, влияет на ускорение и проходимость")]
        [SerializeField] private float motorTorque;
        [SerializeField] private WheelCollider[] wheelsRotater;
        [SerializeField] private WheelCollider[] wheels;
        [SerializeField] private Wheels wheelsChecker;
        
       
        private float _speed;
        private float _targetSpeed;
        private Rigidbody _rb;
        private bool _breaking;
        private bool _wasBreaking = false;
        private bool _testing = false;
        private bool _junctionLock;
        private bool _junctionConnect;
        
        private void Awake()
        {
            TryGetComponent(out _rb);
        }

        private void Start()
        {
            JoystickInput.joy.Controller.Axis.performed += context => Move(context);
            JoystickInput.joy.Controller.Axis.canceled += context => Move(context);
            JoystickInput.joy.Controller.AxisWasd.performed += context => Move(context);
            JoystickInput.joy.Controller.AxisWasd.canceled += context => Move(context);
            JoystickInput.joy.Controller.Button_2.performed += context => BrakeButton(context);
            JoystickInput.joy.Controller.Button_2.canceled += context => BrakeButton(context);
            UnityEngine.Cursor.visible = false;
        }

        /// <summary>
        /// Принятия значений джойстика
        /// </summary>
        /// <param name="context"></param>
        public void Move(InputAction.CallbackContext context)
        {
            if(collectMode || _testing) return;
            if (_junctionLock)
            {
                if (context.ReadValue<Vector2>().y != 0)
                {
                    if (_junctionConnect)
                    {
                        if (context.ReadValue<Vector2>().y < -0.1f)
                        {
                            _rb.isKinematic = false;
                            _junctionConnect = false;
                            OutJunction?.Invoke();
                        }
                        else
                        {
                            _targetSpeed = 0;
                        }
                    }
                    else
                    {
                        _targetSpeed = maxSpeed * context.ReadValue<Vector2>().y;
                    }
                }
                else
                {
                    _targetSpeed = 0;
                }
                return;
            }
            float vertical = context.ReadValue<Vector2>().y;
            float horizontal = context.ReadValue<Vector2>().x;
            _targetSpeed = maxSpeed * vertical;
            foreach (WheelCollider wheel in wheelsRotater)
            {
                wheel.steerAngle = horizontal * 45;
            }
        }

        /// <summary>
        /// Кнопка остановки (2)
        /// </summary>
        /// <param name="context"></param>
        public void BrakeButton(InputAction.CallbackContext context)
        {
            _breaking = context.ReadValue<float>() != 0;
        }
        
        private void FixedUpdate()
        {
            if(!wheelsChecker.CheckGround()) return;
            if (Stopping()) return;
            Moving();
        }

        /// <summary>
        /// Обычное движение
        /// </summary>
        private void Moving()
        {
            float a = acceleration * Time.fixedDeltaTime; //Локальное ускорение
            if(_wasBreaking){TurnOffBreaking();}
            //МОЩНОСТЬ в Л.с. = КРУТЯЩИЙ МОМЕНТ х ОБОРОТЫ ÷ 5252
            //N = FV => V = N/F
            foreach (var wheel in wheels)
            {
                //Скорость по числу оборотов колеса на длину его окружности
                double realSpeed = wheel.rpm / 60 * (2 * wheel.radius * Math.PI);
                if (Math.Abs(realSpeed) <  Math.Abs(_targetSpeed))
                {
                    wheel.motorTorque = _speed * motorTorque;
                }
                else
                {
                    wheel.motorTorque = 0;
                }
            }
            
            if (Math.Abs(_targetSpeed) <  a && Math.Abs(_speed) < a)
            {
                StateEvent(States.stay, _speed);
                _speed = 0;
            }
            else
            {
                if (Math.Abs(_targetSpeed - _speed) < a)
                {
                    StateEvent(States.move, _speed);
                }
                else
                {
                    if (Math.Abs(_targetSpeed) > a)
                    {
                        if (_speed < _targetSpeed)
                        {
                            _speed += a;
                        }
                        else
                        {
                            _speed -= a;
                        }
                        StateEvent(States.acceleration, _speed);
                    }
                    else
                    {
                        if (_speed < _targetSpeed)
                        {
                            _speed += a/2;
                        }
                        else
                        {
                            _speed -= a/2;
                        }
                        StateEvent(States.stay, _speed);
                    }
                }
            }
        }
        
        /// <summary>
        /// Остановка
        /// </summary>
        /// <returns></returns>
        private bool Stopping()
        {
            if (!_breaking && !collectMode && !_testing) return false;
            _wasBreaking = true;
            float a = brakeAcceleration * Time.fixedDeltaTime;
            foreach (var wheel in wheels)
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = 25;
            }

            if (Math.Abs(_speed) < a)
            {
                _speed = 0;
                StateEvent(States.stay, _speed);
            }
            else
            {
                if (_speed > 0)
                    _speed -= a;
                else
                    _speed += a;
                StateEvent(States.breaking, _speed);
            }
            return true;
        }

        
        private void TurnOffBreaking()
        {
            foreach (var wheel in wheels)
            {
                wheel.brakeTorque = 0;
            }

            _wasBreaking = false;
        }

        public void SetTesting(bool stateBreaking)
        { 
            _testing = stateBreaking;
            _rb.isKinematic = stateBreaking;
            _rb.velocity = Vector3.zero;
            _targetSpeed = 0;
        }

        public void SetLock(bool locking)
        {
            _junctionLock = locking;
            //_rb.isKinematic = locking;
        }
        
        public void SetConnect(Transform pos)
        {
            _rb.isKinematic = true;
            transform.position = pos.position;
            transform.rotation = pos.rotation;
            _rb.velocity = Vector3.zero;
            _junctionConnect = true;
        }

        public float GetMaxSpeed() => maxSpeed;

        public float GetTargetSpeed() => _targetSpeed;
        
        public float GetRealSpeed() => _rb.velocity.magnitude;

        public bool GetTesting() => _testing;
    }
}
