using UnityEngine;
using UnityEngine.Events;

namespace ITPRO.Rover.Test
{
    public class TestBase : MonoBehaviour
    {
        protected bool Active { get; set; }

        /// <summary>
        /// Запуск теста
        /// </summary>
        /// <returns></returns>
        public virtual bool Activate()
        {
            Active = true;
            //TODO
            return Active;
        }

        /// <summary>
        /// Выключение теста
        /// </summary>
        /// <returns></returns>
        public virtual bool Deactivate()
        {
            Active = false;
            //TODO
            return Active;
        }

        public bool GetActive()
        {
            return Active;
        } //Получение активности

        
        [System.Serializable]
        public class TimeEvents : UnityEvent<string> { }

        public TimeEvents activateTest = new TimeEvents();
        public TimeEvents deactivateTest = new TimeEvents();
    }
}