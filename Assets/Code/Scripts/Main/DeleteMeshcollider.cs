using System;
using UnityEngine;

public class DeleteMeshcollider : MonoBehaviour
{
    private void Start()
    {
        var col = GetComponentsInChildren<MeshCollider>();
        foreach (var VARIABLE in col)
        {
            if(VARIABLE.convex == false)
                Destroy(VARIABLE);
        }

        var mesh = GetComponentsInChildren<MeshFilter>();
        foreach (var VARIABLE in mesh)
        {
            if(VARIABLE.mesh == null)
                Destroy(VARIABLE.gameObject);
        }
        
        
    }
}