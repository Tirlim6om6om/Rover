using System.Collections.Generic;
using UnityEngine;


namespace  ITPRO.Rover.RoboticArm
{
    public class CheckTakingObj : MonoBehaviour
    {
        [SerializeField] private List<CollisionDetect> clawes;
        [SerializeField] private LayerMask mask;
        public GameObject GetObjPalm()
        {
            List<GameObject> objects = new List<GameObject>();
            List<int> counts = new List<int>();

            foreach (var clawe in clawes)
            {
                foreach (var obj in clawe.GetObjs())
                {
                    int index = GetIndexFind(objects, obj);
                    if (index == -1)
                    {
                        if (((1 << obj.gameObject.layer) & mask) != 0)
                        {
                            objects.Add(obj);
                            counts.Add(1);
                        }
                    }
                    else
                    {
                        counts[index] += 1;
                    }
                }
            }

            int maxIndex = GetMaxIndex(counts);
            if (maxIndex == -1) return null;
            if (counts[maxIndex] < 3) return null;
            return objects[maxIndex];
        }

        private int GetIndexFind<T>(List<T> list, T find)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(find))
                {
                    return i;
                }
            }
            return -1;
        }

        private int GetMaxIndex(List<int> list)
        {
            int max = 0;
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] >= max)
                {
                    max = list[i];
                    index = i;
                }
            }
            return index;
        }

        public void ClearObj(GameObject obj)
        {
            foreach (var clawe in clawes)
            {
                for (int i = 0; i < clawe.GetObjs().Count; i++)
                {
                    if (clawe.GetObjs()[i] == obj)
                    {
                        clawe.GetObjs().Remove(obj);
                    }
                }
            }
        }
    }
}