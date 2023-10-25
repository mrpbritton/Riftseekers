using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;


public class NavMeshThing : MonoBehaviour
{
    public NavMeshData m_NavMeshData;
    private NavMeshDataInstance m_NavMeshInstance;
    // Start is called before the first frame update
    void OnEnable()
    {
        m_NavMeshInstance = NavMesh.AddNavMeshData(m_NavMeshData);
    }

    // Update is called once per frame
    void OnDisable()
    {
        NavMesh.RemoveNavMeshData(m_NavMeshInstance);
    }
}
