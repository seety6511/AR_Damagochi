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

    public List<Damagochi> enablePool;
    public List<Damagochi> disablePool;

    private void Start()
    {
        enablePool = new List<Damagochi>();
        disablePool = new List<Damagochi>();

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
        if (spawnSpeed < spawnTimer)
            return;

        disablePool[0].On(GetSpawnPoint());
        spawnTimer = 0f;
    }

    Vector3 GetSpawnPoint()
    {
        var x = Random.Range(-radius, radius);
        float temp = Mathf.Pow(radius, 2) - Mathf.Pow(x, 2);
        var z = Mathf.Sqrt(temp);

        Vector3 result = (new Vector3(x, 0f, z) * Random.Range(0, radius)) + new Vector3(transform.position.x, 0f, transform.position.z);

        return result;
    }
}
