using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ITPRO.Rover.Minerals;

namespace  ITPRO.Rover.RoboticArm
{
    public enum TypesLimitaion
    {
        min,
        max
    }

    [System.Serializable]
    public class Limits
    {
        public TypesLimitaion x;
        public TypesLimitaion y;
        public TypesLimitaion z;
    }

    [System.Serializable]
    public class Claw
    {
        [SerializeField]private Transform pos;
        [SerializeField]private Vector3 direction;
        [SerializeField]private Vector3 rotation;
        [SerializeField]private Limits OpenLimitaion;
        [SerializeField]private Vector3 min;
        [SerializeField]private Vector3 max;
        public float speed;
        [SerializeField]private CollisionDetect collision;
        [SerializeField]private CollisionDetect collisionBack;
        [SerializeField]private GameObject collider;

        private bool blockOpen;
        
        
        public bool isOpened()
        {
            float x = 0;
            float y = 0;
            float z = 0;
            //x
            if (OpenLimitaion.x  == TypesLimitaion.min)
            {
                x = Math.Abs(rotation.x - min.x);
            }
            else
            {
                x = Math.Abs(max.x - rotation.x );
            }
            //y
            if (OpenLimitaion.y  == TypesLimitaion.min)
            {
                y = Math.Abs(rotation.y - min.y);
            }
            else
            {
                y = Math.Abs(max.y - rotation.y);
            }
            //z
            if (OpenLimitaion.z  == TypesLimitaion.min)
            {
                z = Math.Abs(rotation.z - min.z);
            }
            else
            {
                z = Math.Abs(max.z - rotation.z);
            }
            
            return x < speed && y < speed && z < speed;
        }
        
        public bool isClosed()
        {
            float x = 0;
            float y = 0;
            float z = 0;
            if (collision.GetActive()) return true;
            //x
            if (OpenLimitaion.x  != TypesLimitaion.min)
            {
                x = Math.Abs(rotation.x - min.x);
            }
            else
            {
                x = Math.Abs(max.x - rotation.x);
            }
            //y
            if (OpenLimitaion.y  != TypesLimitaion.min)
            {
                y = Math.Abs(rotation.y - min.y);
            }
            else
            {
                y = Math.Abs(max.y - rotation.y);
            }
            //z
            if (OpenLimitaion.z  != TypesLimitaion.min)
            {
                z = Math.Abs(rotation.z - min.z);
            }
            else
            {
                z = Math.Abs(max.z - rotation.z);
            }
            
            return x < speed && y < speed && z < speed;
        }

        /// <summary>
        /// Передвижение когтей
        /// </summary>
        /// <param name="speedV">Скорость</param>
        /// <param name="checkCol">Остановка, при касании</param>
        public void AddSpeed(float speedV, bool checkCol, bool open)
        {
            if (checkCol && collision.GetActive()) return;
            
            if (open)
            {
                if (collisionBack.GetActive())
                {
                    speedV = -speedV;
                    blockOpen = true;
                }
                else
                {
                    if(blockOpen) return;
                }
            }
            else
            {
                blockOpen = false;
            }

            //x
            if (direction.x != 0)
            {
                Vector3 dir = new Vector3(direction.x, 0, 0);
                if (AddSpeedCheck(speedV, dir))
                {
                    pos.localEulerAngles += dir * speedV;
                    rotation += dir * speedV;
                }
            }
            //y
            if (direction.y != 0)
            {
                Vector3 dir = new Vector3(0, direction.y, 0);
                if (AddSpeedCheck(speedV, dir))
                {
                    pos.localEulerAngles += dir * speedV;
                    rotation += dir * speedV;
                }
            }
            //z
            if (direction.z != 0)
            {
                Vector3 dir = new Vector3(0, 0, direction.z);
                if (AddSpeedCheck(speedV, dir))
                {
                    pos.localEulerAngles += dir * speedV;
                    rotation += dir * speedV;
                }
            }
        }
        
        /// <summary>
        /// Проверка лимитов
        /// </summary>
        /// <param name="speedV"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool AddSpeedCheck(float speedV, Vector3 dir)
        {
            Vector3 vec = rotation + speedV*dir;

            bool checkMin = vec.x >= min.x && vec.y >= min.y && vec.z >= min.z;
            bool checkMax = vec.x <= max.x && vec.y <= max.y && vec.z <= max.z;

            return checkMin && checkMax;
        }

        public void SetActiveCollier(bool active)
        {
            collider.SetActive(active);
        }
    }
    
    public class PalmController : MonoBehaviour
    {
        [SerializeField] private Claw[] clawes;
        [SerializeField] private CheckTakingObj takingObjCheck;
        [SerializeField] private Transform center;
        [SerializeField] private Rigidbody centerFix;
        private GameObject _taked;
        private bool _open = true;


        public void Trigger()
        {
            if (_open)
                Close();
            else
                Open();
        }

        /// <summary>
        /// Открытие клешни
        /// </summary>
        public void Open()
        {
            _open = true;
            StartCoroutine(Opening());
        }

        private IEnumerator Opening()
        {
            CheckThrow();
            while (!CheckOpened() && _open)
            {
                foreach (var clawe in clawes)
                {
                    clawe.AddSpeed(clawe.speed, false, true);
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
        
        private bool CheckOpened()
        {
            foreach (var clawe in clawes)
            {
                if (!clawe.isOpened())
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Отпускание объекта
        /// </summary>
        private void CheckThrow()
        {
            if(_taked == null) return;
            if (_taked.TryGetComponent(out Mineral mineral))
            {
                mineral.UnfixRb();
            }
            else
            {
                return;
            }
            _taked.transform.parent = null;
        }
        
        /// <summary>
        /// Закрытие (захват)
        /// </summary>
        public void Close()
        {
            _open = false;
            StartCoroutine(Closing());
        }

        private IEnumerator Closing()
        {
            while (!CheckClosed() && !_open)
            {
                foreach (var clawe in clawes)
                {
                    clawe.AddSpeed(-clawe.speed, true, false);
                }
                yield return new WaitForSeconds(0.025f);
            }
            if(!_open)
                TakeObj();
        }

        private bool CheckClosed()
        {
            foreach (var clawe in clawes)
            {
                if (!clawe.isClosed())
                {
                    return false;
                }
            }
            return true;
        }

        
        /// <summary>
        /// Взятие объекта
        /// </summary>
        private void TakeObj()
        {
            _taked = takingObjCheck.GetObjPalm();
            if(_taked == null) return;
            if (CheckCrash(_taked.transform))
            {
                takingObjCheck.ClearObj(_taked);
                return;
            }
            if (_taked.TryGetComponent(out Mineral mineral))
            {
                mineral.FixRb(centerFix);
            }
            else
            {
                return;
            }
            takingObjCheck.ClearObj(_taked);
        }

        private bool CheckCrash(Transform obj)
        {
            print("Dist: " + Vector3.Distance(center.position, obj.position));
            if (obj.TryGetComponent<Mineral>(out var mineral))
            {
                if (Vector3.Distance(center.position, obj.position) > mineral.radius)
                {
                    mineral.Crash();
                    return true;
                }
            }
            return false;
        }
    }
}