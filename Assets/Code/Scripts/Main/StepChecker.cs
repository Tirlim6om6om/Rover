using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.Managers
{
    [System.Serializable]
    public class ListOfObjects
    {
        public List<GameObject> _objects;
    }

    public class StepChecker : MonoBehaviour
    {
        public static StepChecker instance;
        public bool startSteps;
        
        [SerializeField] private List<ListOfObjects> gameObjectsOff;
        [SerializeField] private List<ListOfObjects> gameObjectsOn;
        private int _step;

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        public void NextStep()
        {
            if (_step > gameObjectsOff.Count || _step > gameObjectsOn.Count) return;
            foreach (var obj in gameObjectsOff[_step]._objects)
            {
                obj.SetActive(false);
            }

            foreach (var obj in gameObjectsOn[_step]._objects)
            {
                obj.SetActive(true);
            }

            _step += 1;
        }
        
        public void CheckWin()
        {
            if (_step == 3)
            {
                GameManager.instance.Win();
            }
            else
            {
                if (startSteps)
                {
                    GameManager.instance.Lose("невыполнение задач");
                }
            }
        }
    }
}