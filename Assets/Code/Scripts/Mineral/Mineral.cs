using System;
using System.Collections;
using ITPRO.Rover.Controller;
using ITPRO.Rover.User;
using UnityEngine;


namespace ITPRO.Rover.Minerals
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mineral : MonoBehaviour
    {
        public float radius;
        public int size;

        [SerializeField] private GameObject crush;

        private MineralSpawn _spawn;
        private Rigidbody _rb;
        private float _maxVelocity;
        private FixedJoint _fixedJoint;

        private void OnCollisionEnter(Collision collision)
        {
            if (_maxVelocity > 1.6f)
            {
                Crash();
            }
            _maxVelocity = 0;
        }
        
        private void Start()
        {
            TryGetComponent(out _rb);
        }
        
        private void FixedUpdate()
        {
            _maxVelocity = _rb.velocity.magnitude;
        }
        
        public void Crash()
        {
            print("Mineral crash");
            switch (size)
            {
                case 0:
                    StatisticWriter.instance.SmallMineral = "разбит";
                    break;
                case 1:
                    StatisticWriter.instance.MediumMineral = "разбит";
                    break;
                case 2:
                    StatisticWriter.instance.BigMineral = "разбит";
                    break;
            }

            Instantiate(crush, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        public void FixRb(Rigidbody center)
        {
            if (_fixedJoint != null)
            {
                Debug.LogError("Конфликт присоединения камня");
                return;
            }
            _fixedJoint = gameObject.AddComponent<FixedJoint>();
            _fixedJoint.enableCollision = true;
            _rb.useGravity = false;
            _fixedJoint.connectedMassScale = 5;
            _fixedJoint.massScale = 5;
            _fixedJoint.connectedBody = center;
        }

        public void UnfixRb()
        {
            _rb.useGravity = true;
            Destroy(_fixedJoint);
        }

        public void FixContainer(Transform parent)
        {
            if (_fixedJoint != null)
            {
                Destroy(_fixedJoint);
                Debug.Log("Destroy joint!");
            }
            _rb.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer("Container");
            transform.parent = parent;
            print(parent.name);
        }

        private void OnDestroy()
        {
            _spawn.spawned.Remove(this);
        }

        public MineralSpawn spawn
        {
            set => _spawn = value;
        }
    }
}