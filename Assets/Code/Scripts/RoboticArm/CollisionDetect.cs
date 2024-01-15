using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.RoboticArm
{
    public class CollisionDetect : MonoBehaviour
    {
        [SerializeField] private LayerMask ignoreLayer;
        private List<GameObject> _objects = new List<GameObject>();
        private bool _ground;

        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Terrain"))
            {
                _ground = true;
            }
            else
            {
                if(((1<<other.gameObject.layer) & ignoreLayer) != 0) return;
                if (!HaveObj(obj) && !CheckTrigger(obj))
                {
                    _objects.Add(obj);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Terrain"))
            {
                _ground = false;
            }
            else
            {
                if(((1<<other.gameObject.layer) & ignoreLayer) != 0) return;
                if (HaveObj(obj) && !CheckTrigger(obj))
                {
                    _objects.Remove(obj);
                }
            }
        }
        
        private bool HaveObj(GameObject find)
        {
            foreach (var obj in _objects)
            {
                if (obj == find)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckTrigger(GameObject obj)
        {
            if (obj.TryGetComponent(out Collider col))
            {
                if(col.isTrigger) return true;
            }
            return false;
        }

        public bool GetActive()
        {
            foreach (var obj in _objects)
            {
                if (obj == null)
                {
                    _objects.Remove(obj);
                }
            }
            return _objects.Count != 0;
        }

        public List<GameObject> GetObjs()
        {
            return _objects;
        }
    }
}