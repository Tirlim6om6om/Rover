using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.Managers
{
    public class ClearRenderTexture : MonoBehaviour
    {
        [SerializeField] private List<RenderTexture> _textures;

        private void Start()
        {
            GL.Clear(true, true, Color.clear);
            foreach (var texture in _textures)
            {
                texture.Release();
            }
        }
    }
}