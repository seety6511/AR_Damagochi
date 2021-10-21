using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PoolDamagochi : Damagochi
{
    public Vector3 spawnPoint;
    public bool playerble;

    List<SH_PoolDamagochi> enablePool;
    List<SH_PoolDamagochi> disablePool;
    public void SetPool(List<SH_PoolDamagochi> enablePool, List<SH_PoolDamagochi> disablePool)
    {
        this.enablePool = enablePool;
        this.disablePool = disablePool;
    }

    public void On(Vector3 pos)
    {
        spawnPoint = pos;
        disablePool[0].transform.position = pos;
        disablePool[0].gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        if (playerble)
            return;

        enablePool?.Add(this);
        disablePool?.Remove(this);
        //Debug.Log("OnEnable : " + name);
    }

    protected virtual void OnDisable()
    {
        if (playerble)
            return;

        enablePool?.Remove(this);
        disablePool?.Add(this);
        //Debug.Log("OnDisable : " + name);
    }

}
