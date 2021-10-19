using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_EnemySpanwer : MonoBehaviour
{
    public float radius;
    public float spawnSpeed;
    float spawnTimer;

    public int maxEnemy;
    public List<Damagochi> damagochiList;

    public List<Damagochi> enablePool = new List<Damagochi>();
    public List<Damagochi> disablePool = new List<Damagochi>();

    private void Start()
    {
        for(int i = 0; i < maxEnemy; ++i)
        {
        }
    }

    public void Spawn()
    {
        if (enablePool.Count == maxEnemy)
        {
            Debug.Log("Enemy Max");
            return;
        }
        
        spawnTimer += Time.deltaTime;
        if (spawnSpeed < spawnTimer)
            return;
    }
}
