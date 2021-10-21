using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_EnemySpanwer : MonoBehaviour
{
    public float radius;
    public float spawnSpeed;
    float spawnTimer;

    public int maxEnemy;
    public List<SH_PoolDamagochi> damagochiList;

    public List<SH_PoolDamagochi> enablePool;
    public List<SH_PoolDamagochi> disablePool;

    private void Start()
    {
        enablePool = new List<SH_PoolDamagochi>();
        disablePool = new List<SH_PoolDamagochi>();

        int ec = maxEnemy;
        int i = 0;
        while (ec-- != 0)
        {
            var dama = Instantiate(damagochiList[i++]);
            enablePool.Add(dama);
            dama.SetPool(enablePool, disablePool);
            dama.Off();

            if (i == damagochiList.Count)
                i = 0;
        }
    }
    private void Update()
    {
        Spawn();
    }
    public void Spawn()
    {
        if (enablePool.Count == maxEnemy)
            return;
        
        spawnTimer += Time.deltaTime;
        if (spawnSpeed > spawnTimer)
            return;

        disablePool[0].On(SH_GameManager.GetRandomInnerCirclePoint(gameObject.transform.position, radius));
        spawnTimer = 0f;
    }
}
