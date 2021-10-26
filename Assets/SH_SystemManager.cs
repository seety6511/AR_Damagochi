using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_SystemManager : MonoBehaviour
{
    [SerializeField]
    SH_WorldBuilder worldBuilder;
    [SerializeField]
    SH_NavMeshBuilder navMeshBuilder;

    private void Start()
    {
        worldBuilder = FindObjectOfType<SH_WorldBuilder>();
        navMeshBuilder = FindObjectOfType<SH_NavMeshBuilder>();

        worldBuilder.Init();
        navMeshBuilder.Init();
        StartCoroutine(navMeshBuilder.NavMeshUpdate());
    }

    private void Update()
    {
        worldBuilder.UpdateInstances();
    }
}
