using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_SystemManager : MonoBehaviour
{
    SH_WorldBuilder worldBuilder;
    SH_NavMeshBuilder navMeshBuilder;
    SH_EnemySpanwer enemySpawner;
    SH_PlaneSetting planeSetting;
    public SH_Player player;

    bool setFirstPlane;

    private void Start()
    {
        planeSetting = FindObjectOfType<SH_PlaneSetting>();
        planeSetting.Init();
    }

    public void Find()
    {
        worldBuilder = FindObjectOfType<SH_WorldBuilder>();
        navMeshBuilder = FindObjectOfType<SH_NavMeshBuilder>();
        enemySpawner = FindObjectOfType<SH_EnemySpanwer>();
        player = FindObjectOfType<SH_Player>();
    }
    void Init()
    {
        worldBuilder.Init();
        navMeshBuilder.Init();
        enemySpawner.Init();
        StartCoroutine(navMeshBuilder.NavMeshUpdate());
    }

    public void SetFirstPlane(GameObject anchor)
    {
        Find();
        player.controlDamagochi.transform.position = anchor.transform.position;
        setFirstPlane = true;
        navMeshBuilder.center = player.controlDamagochi.transform;
        worldBuilder.center = player.controlDamagochi.transform;
        worldBuilder.height = player.controlDamagochi.transform.position.y;
        Init();
        player.controlDamagochi.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!setFirstPlane)
            return;

        worldBuilder.UpdateInstances();
        enemySpawner.Spawn();
    }
}
