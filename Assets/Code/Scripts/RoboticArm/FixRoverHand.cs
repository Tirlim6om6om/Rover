
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.RoboticArm
{
    [System.Serializable]
    public class JointsHand
    {
        [SerializeField] private HingeJoint joint;
        private JointLimits limits;
        private bool active;

        public void SaveFix()
        {
            limits = joint.limits;
        }

        public IEnumerator Fixing()
        {
            active = true;
            while ((joint.limits.min != 0 || joint.limits.min != 0) && active)
            {
                JointLimits newLim = joint.limits;
                
                if(newLim.max > 0)
                    newLim.max = joint.limits.max - 0.25f;
                else
                    newLim.max = joint.limits.max + 0.25f;
                
                if(newLim.min > 0)
                    newLim.min = joint.limits.min - 0.25f;
                else
                    newLim.min = joint.limits.min + 0.25f;
                
                joint.limits = newLim;
                yield return new WaitForFixedUpdate();
            }

            if (!active && (joint.limits.max != limits.max || joint.limits.min != limits.min))
            {
                Unfix();
            }
        }
        
        public void Unfix()
        {
            joint.limits = limits;
            active = false;
        }
    }
    
    public class FixRoverHand : MonoBehaviour
    {
        [SerializeField] private List<JointsHand> joints;
        [SerializeField] private RoboticArm arm;

        private void Awake()
        {
            foreach (var joint in joints)
            {
                joint.SaveFix();
            }
        }

        private void Start()
        {
            CheckFix(false);
        }

        private void OnEnable()
        {
            arm.CollectActive += CheckFix;
        }

        private void OnDisable()
        {
            arm.CollectActive -= CheckFix;
        }

        public void CheckFix(bool active)
        {
            if (active)
            {
                foreach (var joint in joints)
                {
                    joint.Unfix();
                }
            }
            else
            {
                foreach (var joint in joints)
                {
                    StartCoroutine(joint.Fixing());
                }
            }
        }
        
        
    }
}