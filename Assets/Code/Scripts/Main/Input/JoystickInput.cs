using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace ITPRO
{
    public class JoystickInput : MonoBehaviour
    {
        
        public static Joystick joy;
        public static Joystick.ControllerActions button;

        
        private void Awake()
        {
            joy = new Joystick();
            button = joy.Controller;
        }

        private void OnEnable()
        {
            joy.Enable();
        }

        private void OnDisable()
        {
            joy.Disable();
        }
        
        
        public static Vector2 GetJoystick() => joy.Controller.Axis.ReadValue<Vector2>();

        public static bool GetButton(InputAction actions) => actions.ReadValue<float>() > 0.5f;
    }
}