using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ITPRO.Rover.Minerals
{

    [System.Serializable]
    public class MineralSpawnInfo
    {
        public Transform point;
        [SerializeField]private GameObject prefab;
        [SerializeField] private float radius;
        public Terrain terrain { private get; set; }
        
        
        
        public Vector3 SpawnPos()
        {
            float x = point.position.x + Random.Range(-radius, radius);
            float z = point.position.z + Random.Range(-radius, radius);
            float y = terrain.SampleHeight(new Vector3(x,0,z))+0.15f;
            return new Vector3(x, y, z);
        }

        public GameObject GetPrefab()
        {
            return prefab;
        }
    }
    
    public class MineralSpawn : MonoBehaviour
    {
        [HideInInspector] public List<Mineral> spawned = new List<Mineral>();
        [SerializeField] private List<MineralSpawnInfo> minerals;
        [SerializeField] private Terrain terrain;
        
        private void Start()
        {
            foreach (var mineral in minerals)
            {
                mineral.terrain = terrain;
                spawned.Add(Instantiate(mineral.GetPrefab(), mineral.SpawnPos(), Quaternion.identity).GetComponent<Mineral>());
            }

            foreach (var mineral in spawned)
            {
                mineral.spawn = this;
            }
        }
        
        private void OnDrawGizmos()
        {
            foreach (var mineral in minerals)
            {
                Gizmos.DrawIcon(mineral.point.position, "SpawnMineral.png", false);
            }
        }
    }
}