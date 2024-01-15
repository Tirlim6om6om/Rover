using System;
using ITPRO.Rover.Controller;
using ITPRO.Rover.Test;
using ITPRO.Rover.User;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    public bool couping;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private LayerMask layer;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(((1<<collision.collider.gameObject.layer) & layer) == 0) return;
        if(couping) return;
        if(speedometer.GetSpeedMeter() < 0.2f) return;
        StatisticWriter.instance.ControlErrors++;
        if (speedometer.GetSpeedMeter() < 1)
        {
            TestController.instance.ActivateTest(TypeTest.wheelFialure);
        }
        else
        {
            TestController.instance.ActivateTest(TypeTest.matrix);
        }
    }


}