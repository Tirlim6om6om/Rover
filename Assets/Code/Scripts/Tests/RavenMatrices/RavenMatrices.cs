using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.Test.Raven
{
    [System.Serializable]
    public struct Matrix
    {
        public Texture2D texture;
        public int rightAnswer;
    }

    public class RavenMatrices : MonoBehaviour
    {
        [System.Serializable]
        private struct MatricesCategory
        {
            public Matrix[] matrices;
        }

        [SerializeField] private List<MatricesCategory> matricesCategories;

        public List<Matrix> GetRandomMatrices()
        {
            var selectMatrices = new List<Matrix>();
            
            foreach (MatricesCategory item in matricesCategories)
            {
                selectMatrices.Add(item.matrices[Random.Range(0, item.matrices.Length - 1)]);
            }

            return selectMatrices;
        }
    }
}

