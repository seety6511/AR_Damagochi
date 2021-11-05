using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_GPSEnemySpawner : MonoBehaviour
{
    SH_GoogleMap gm;

    public int maxEnemy;
    public float spawnRadius;
    public float spawnSpeed;
    float spawnTimer;

    public SH_Button_Enemy enemyMarkerPrefab;
    public List<SH_ActionDamagochi> enemyList;
    public List<SH_Button_Enemy> enableEnemyMarkers;
    SH_DamagochiTrainer trainer;
    private void Start()
    {
        gm = FindObjectOfType<SH_GoogleMap>();
        trainer = FindObjectOfType<SH_DamagochiTrainer>();
    }

    private void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (!gm.mapOn)
            return;

        if (enableEnemyMarkers.Count == maxEnemy)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnSpeed > spawnTimer)
            return;

        var marker = GetMarker();
        if (marker == null)
            return;

        var enemy = GetRandomEnemy();

        enemy.level = Random.Range(1, trainer.damagochi.level + 6);
        marker.SetEnemy(enemy);
        enableEnemyMarkers.Add(marker);

        marker.transform.position = GetRandomPos();
        spawnTimer = 0f;
    }

    public void ResetEnemy()
    {
        var count = enableEnemyMarkers.Count;
        for (int i = 0; i < count; ++i)
        {
            var e = enableEnemyMarkers[0];
            enableEnemyMarkers.RemoveAt(0);
            if (e.enemy != null)
                Destroy(e.enemy.gameObject);
            Destroy(e.gameObject);
        }

        enableEnemyMarkers.Clear();
    }

    SH_ActionDamagochi GetRandomEnemy()
    {
        return Instantiate(enemyList[Random.Range(0, enemyList.Count)]);
    }

    SH_Button_Enemy GetMarker()
    {
        return  Instantiate(enemyMarkerPrefab, transform);
    }
    Vector3 GetRandomPos()
    {
        var pos = Random.insideUnitCircle* spawnRadius;
        return new Vector3(pos.x + gm.rawImage.transform.position.x, pos.y+ gm.rawImage.transform.position.y);
    }
}
