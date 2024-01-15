using UnityEngine;

namespace ITPRO.Rover.Controller.Statistic
{
    public class PositionRover : MonoBehaviour
    {
        [SerializeField] private Transform start;

        public Vector3 GetPos()
        {
            return transform.position - start.position;
        }
    }
}