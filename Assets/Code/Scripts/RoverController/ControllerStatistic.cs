using UnityEngine;
using System.Collections;
using ITPRO.Rover.Managers;
using ITPRO.Rover.Test;
using ITPRO.Rover.User;

namespace ITPRO.Rover.Controller.Statistic
{
    public class ControllerStatistic : MonoBehaviour
    {
        [SerializeField] private RoverController rover;
        [SerializeField] private Battery battery;
        [SerializeField] private Gizma gizma;
        [SerializeField] private PositionRover pos;
        private int _frameCount;

        private void Start()
        {
            StartCoroutine(UpdateFrame());
        }

        private IEnumerator UpdateFrame()
        {
            while (GameManager.instance.GetActive())
            {
                SaveFrame();
                _frameCount++;
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void SaveFrame()
        {
            if(rover.GetTesting() || rover.collectMode) return;
            ControllerFrame frame = new ControllerFrame
            {
                index = _frameCount,
                joy = new Vector2(JoystickInput.GetJoystick().x * 10, JoystickInput.GetJoystick().y * 10),
                speed = rover.GetRealSpeed(),
                charge = Mathf.Round(battery.GetCharge()*100),
                pitch = gizma.GetPitch(), //тангаж
                roll = gizma.GetRoll(), //крен
                pos = pos.GetPos()
            };
            print(frame);
            StatisticWriter.instance.AddFrame(frame);
            //print("тангаж " + frame.pitch + "; крен " + frame.roll);
        }
    }
}