using System.Collections;
using ITPRO.Rover.Managers;
using ITPRO.Rover.RoboticArm;
using ITPRO.Rover.User;
using UnityEngine;

namespace ITPRO.Rover.Controller
{
    public class CheckCoup : MonoBehaviour
    {
        [SerializeField]private float force;
        [SerializeField]private float angular;
        [SerializeField]private FixRoverHand arm;
        [SerializeField]private HitCheck hitCheck;
        
        private Rigidbody _rb;
        private bool _active;
        private bool _closeCoup;
        
        
        
        private void Start()
        {
            TryGetComponent(out _rb);
            StartCoroutine(Checking());
        }

        private IEnumerator Checking()
        {
            while (true)
            {
                if ((transform.eulerAngles.x > 30 && transform.eulerAngles.x < 330)
                    || (transform.eulerAngles.z > 30 && transform.eulerAngles.z < 330))
                {
                    if (!_active)
                    {
                        _active = true;
                        arm.CheckFix(true);
                        hitCheck.couping = true;
                        TimeController.instance?.SetDefault();
                        StartCoroutine(Rotating());
                    }
                }
                else
                {
                    if (_active)
                    {
                        arm.CheckFix(false);
                        hitCheck.couping = false;
                        _active = false;
                    }
                }
                
                if ((transform.eulerAngles.x > 28 && transform.eulerAngles.x < 332)
                    || (transform.eulerAngles.z > 28 && transform.eulerAngles.z < 332))
                {
                    if (!_closeCoup)
                    {
                        StatisticWriter.instance.ControlErrors++;
                        _closeCoup = true;
                    }
                }
                else
                {
                    _closeCoup = false;
                }
                

                if ((transform.eulerAngles.x > 60 && transform.eulerAngles.x < 300)
                    || (transform.eulerAngles.z > 60 && transform.eulerAngles.z < 300))
                {
                    GameManager.instance.Lose("ровер перевернулся");
                    _active = false;
                    break;
                }

                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator Rotating()
        {
            float timer = 0;
            while (_active)
            {
                _rb.AddForce(new Vector3(0,1,0) * force);
                timer += Time.fixedDeltaTime;
                if (transform.eulerAngles.x > 30  && transform.eulerAngles.x < 180)
                {
                    transform.Rotate(2*angular * timer, 0, 0);
                }
                if (transform.eulerAngles.x > 180  && transform.eulerAngles.x < 330)
                {
                    transform.Rotate(-angular* timer, 0, 0);
                }
                if (transform.eulerAngles.z > 30  && transform.eulerAngles.z < 180)
                {
                    transform.Rotate(0, 0, 2*angular* timer);
                }
                if (transform.eulerAngles.z > 180  && transform.eulerAngles.z < 330)
                {
                    transform.Rotate(0, 0, 2*-angular* timer);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}