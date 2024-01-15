using System.Collections.Generic;
using System;
using ITPRO.Rover.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ITPRO.Rover.Test
{
    public enum TypeTest
    {
        wheelFialure,
        RDO,
        matrix,
    }
    
    [System.Serializable]
    public class Test
    {
        public TypeTest type;
        public TestBase test;
    }
    
    public class TestController : MonoBehaviour
    {
        public static TestController instance;
        [SerializeField] private RoverController rover;
        [SerializeField] private List<Test> testes;

        private bool _active;
        private Test _currentTest = new Test();

        private UnityAction<string> _deactivate;
        private void Awake()
        {
            _deactivate = _ => CheckDeactivateTest(); 
            
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
            SetCursor(false);
        }

        public void ActivateTest(TypeTest type)
        {
            if(_active) return;

            TestBase test = GetTest(type);
            if (test == null)
            {
                Debug.LogError("Dont exist test: " + type.ToString());
                return;
            }
            if (!test.GetActive())
            {
                test.Activate();
                test.deactivateTest.AddListener(_deactivate);
                _active = true;
                _currentTest.type = type;
                _currentTest.test = test;
                if(type == TypeTest.matrix) SetCursor(true);
                rover.SetTesting(true);
            }
        }

        private TestBase GetTest(TypeTest type)
        {
            foreach (var test in testes)
            {
                if (test.type == type)
                {
                    return test.test;
                }
            }
            Debug.LogError("Not fount test");
            return null;
        }

        private void CheckDeactivateTest()
        {
            _active = false;
            rover.SetTesting(false);
            if(_currentTest.type == TypeTest.matrix) SetCursor(false);
            _currentTest.test.deactivateTest.RemoveListener(_deactivate);
        }
        
        private void SetCursor(bool active)
        {
            VirtualCursor.instance.SetVisible(active);
        }

        public Test GetActualTest() => _currentTest;
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ActivateTest(TypeTest.wheelFialure);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ActivateTest(TypeTest.matrix);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ActivateTest(TypeTest.RDO);
            }
        }
#endif
    }
}
