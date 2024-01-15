using UnityEngine;

namespace ITPRO.Rover.User
{
    [System.Serializable]
    public class User
    {
        [SerializeField] private int id;
        [SerializeField] private string name;
        

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }
    }   
}