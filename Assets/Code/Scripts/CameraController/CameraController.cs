using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ITPRO.Rover
{

    [System.Serializable]
    public class RotatingObj
    {
        public Transform pos;//Поз камеры
        public Transform posParent;//Позиция род объекта камеры
        //максимум-минимум по горизонтали
        public float maxX;
        public float maxY;
        //максимум-минимум по вертикале
        public float minX;
        public float minY;
        //настоящий поворот от начала
        [HideInInspector]public Vector2 rot;
    }
    
    public class CameraController : MonoBehaviour
    {
        public bool collectMode;
        [SerializeField] private RotatingObj frontCamera;
        [SerializeField] private RotatingObj backCamera;
        [SerializeField] private GameObject camUI;          //Интерфейс камер+миникарта
        [SerializeField] private GameObject[] camerasObj;   //Объекты камер
        [SerializeField] private float speed;               //Скорость поворота
        [SerializeField] private GameObject mainCamera;     //Главная камера
        [SerializeField] private GameObject[] roboticCamera;  //Камера на руке
        [SerializeField] private GameObject infoUI;
        [SerializeField] private GameObject armUI;

        private bool _active;                                //Активность поворотов/камер
        
        private void Start()
        {
            JoystickInput.joy.Controller.Button_1.performed += context => Activate(context);
            _active = camUI.activeSelf;
            if (_active)
            {
                StartCoroutine(CameraRotating(frontCamera));
                StartCoroutine(CameraRotating(backCamera));
            }
        }
    
        
        /// <summary>
        /// Нажатие на кнопку 1
        /// </summary>
        /// <param name="context"></param>
        public void Activate(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                _active = !_active;
                camUI.SetActive(_active);
                foreach (var camObj in camerasObj)
                {
                    camObj.SetActive(_active);
                }

                if (_active)
                {
                    StartCoroutine(CameraRotating(frontCamera));
                    StartCoroutine(CameraRotating(backCamera));
                }
            }
        }
        
        
        private IEnumerator CameraRotating(RotatingObj cam)
        {
            while (_active)
            {
                if (cam == frontCamera)
                {
                    CameraRotate(cam, JoystickInput.joy.Controller.Hut_2.ReadValue<Vector2>());
                }
                else
                {
                    CameraRotate(cam, JoystickInput.joy.Controller.Hut_3.ReadValue<Vector2>());
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        /// <summary>
        /// Поворот камеры
        /// </summary>
        /// <param name="cam">Камера</param>
        /// <param name="hut">хатка</param>
        public void CameraRotate(RotatingObj cam, Vector2 hut)
        {
            if(collectMode) return;
            if(hut.x == 0 && hut.y == 0) return;
            //Y (up-down)
            float newY = cam.rot.y - hut.y * speed;
            if (newY > cam.minY && newY < cam.maxY)
            {
                cam.pos.Rotate(hut.y * (-speed), 0, 0);
                cam.rot.y -= hut.y * speed;
            }
            
            //X (left-right)
            float newX = cam.rot.x + hut.x * speed;
            if (newX > cam.minX && newX < cam.maxX)
            {
                cam.posParent.Rotate(0, hut.x * speed, 0);
                cam.rot.x += hut.x * speed;
            }
        }


        public void SetActiveRoboticCam(bool active)
        {
            mainCamera.SetActive(!active);
            foreach (var armCam in roboticCamera)
            {
                armCam.SetActive(active);
            }
            infoUI.SetActive(!active);
            armUI.SetActive(active);
        }
    }
}