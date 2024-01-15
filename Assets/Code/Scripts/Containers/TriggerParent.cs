using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.Container
{
    public class TriggerParent : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        private List<GameObject> _queueObjs = new List<GameObject>();
        

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.GetComponent<Collider>().gameObject.layer) & layer) == 0) return;
            if (other.transform.parent == null)
            {
                other.transform.parent = transform;
            }
            else
            {
                _queueObjs.Add(other.gameObject);
                if (_queueObjs.Count == 1)
                {
                    StartCoroutine(CheckObj());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent == transform)
            {
                other.transform.parent = null;
            }
            else
            {
                _queueObjs.Remove(other.gameObject);
            }
        }


        private IEnumerator CheckObj()
        {
            while (_queueObjs.Count > 0)
            {
                foreach (var obj in _queueObjs)
                {
                    if (obj.transform.parent == null)
                    {
                        obj.transform.parent = transform;
                        _queueObjs.Remove(obj);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
