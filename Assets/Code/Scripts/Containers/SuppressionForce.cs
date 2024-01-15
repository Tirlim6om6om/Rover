using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ITPRO.Rover.Minerals;
using ITPRO.Rover.User;

namespace ITPRO.Rover.Container
{
    public class SuppressionForce : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private CollectedMinerals collectedMinerals;
        [SerializeField] private LayerMask layer;
        [SerializeField] private int size;
        
        private List<GameObject> _objects = new List<GameObject>();

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & layer) == 0) return;
            if (!_objects.Find(x => x == collision.gameObject))
            {
                _objects.Add(collision.gameObject);
                collision.gameObject.transform.parent = parent;
                StartCoroutine(FixingTimer(collision.gameObject));
            }
            
        }
        

        private void OnCollisionExit(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & layer) == 0) return;
            if (_objects.Find(x => x == collision.gameObject))
            {
                if(collision.gameObject.transform.parent == parent
                   && collision.gameObject.layer != LayerMask.NameToLayer("Container"))
                    collision.gameObject.transform.parent = null;
                _objects.Remove(collision.gameObject);
            }
        }

        private IEnumerator FixingTimer(GameObject obj)
        {
            yield return new WaitForSeconds(1);
            if (_objects.Find(x => x == obj))
            {
                SetFixMineral(obj);
            }
        }
        
        private void SetFixMineral(GameObject obj)
        {
            if (obj.TryGetComponent(out Mineral mineral))
            {
                mineral.FixContainer(parent);
                if (mineral.size == size)
                {
                    collectedMinerals.AddCollected();
                    switch (size)
                    {
                        case 0:
                            StatisticWriter.instance.SmallMineral = "собран";
                            break;
                        case 1:
                            StatisticWriter.instance.MediumMineral = "собран";
                            break;
                        case 2:
                            StatisticWriter.instance.BigMineral = "собран";
                            break;
                    }
                }
                else
                {
                    switch (mineral.size)
                    {
                        case 0:
                            StatisticWriter.instance.SmallMineral = "помещён не в тот контейнер";
                            break;
                        case 1:
                            StatisticWriter.instance.MediumMineral = "помещён не в тот контейнер";
                            break;
                        case 2:
                            StatisticWriter.instance.BigMineral = "помещён не в тот контейнер";
                            break;
                    }
                }
            }
        }
    }
}
