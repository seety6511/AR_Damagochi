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
    private void Start()
    {
        gm = FindObjectOfType<SH_GoogleMap>();

    }

    private void Update()
    {
        if (enableEnemyMarkers.Count == maxEnemy)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnSpeed > spawnTimer)
            return;

        var marker = GetMarker();
        if (marker == null)
            return;

        var enemy = GetRandomEnemy();
        marker.SetEnemy(enemy);
        enableEnemyMarkers.Add(marker);

        marker.transform.position = GetRandomPos();
        spawnTimer = 0f;
    }

    SH_ActionDamagochi GetRandomEnemy()
    {
        return enemyList[Random.Range(0, enemyList.Count)];
    }

    SH_Button_Enemy GetMarker()
    {
        return  Instantiate(enemyMarkerPrefab, FindObjectOfType<Canvas>().transform);
    }
    Vector3 GetRandomPos()
    {
        var pos = Random.insideUnitCircle* spawnRadius;
        return new Vector3(pos.x + gm.rawImage.transform.position.x, pos.y+ gm.rawImage.transform.position.y);
    }
}
