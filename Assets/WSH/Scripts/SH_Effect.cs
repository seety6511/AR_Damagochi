using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Effect : MonoBehaviour
{
    public List<SH_Effect> enablePool;
    public List<SH_Effect> disablePool;

    public float lifeTime;
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Off();
    }

    public void Set(List<SH_Effect> enablePool, List<SH_Effect> disablePool)
    {
        this.enablePool = enablePool;
        this.disablePool = disablePool;
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        timer = 0f;
        disablePool?.Remove(this);
        enablePool?.Add(this); 
    }

    private void OnDisable()
    {
        disablePool?.Add(this);
        enablePool?.Remove(this);
    }
}
