using UnityEngine;

using System.Collections.Generic;

public class SH_WorldBuilder : MonoBehaviour
{
    public GameObject tilePrefab;
    public int tilePoolSize = 250;
    public int instancePerTile = 10;
    public float height;
    public float tileSize = 100.0f;

    public Transform center;
    List<Transform> instances = new List<Transform>();
    int usedTileCount;
    int localX, localZ;
    public void Init()
    {
        Clear();
        for (int i = 0; i < tilePoolSize; ++i)
        {
            var go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            instances.Add(go.transform);
        }
        localX = ~0;
        localZ = ~0;
        UpdateInstances();
    }

    public void Clear()
    {
        SH_GameManager.ListClear(instances);
    }

    public void UpdateInstances()
    {
        var x = (int)Mathf.Floor(center.position.x / tileSize);
        var z = (int)Mathf.Floor(center.position.z / tileSize);
        if (x == localX && z == localZ)
            return;

        localX = x;
        localZ = z;

        usedTileCount = 0;
        for (var i = x - 2; i <= x + 2; ++i)
        {
            for (var j = z - 2; j <= z + 2; ++j)
            {
                var count = UpdateTileInstances(i, j);
                if (count != instancePerTile)
                    return;
            }
        }

        for (int i = usedTileCount; i < tilePoolSize && instances[i].gameObject.activeSelf; ++i)
            instances[i].gameObject.SetActive(false);
    }

    int UpdateTileInstances(int i, int j)
    {
        var count = System.Math.Min(instancePerTile, tilePoolSize - usedTileCount);
        for (var end = usedTileCount + count; usedTileCount < end; ++usedTileCount)
        {
            var pos = new Vector3(i * tileSize, height, j * tileSize);
            instances[usedTileCount].position = pos;
            instances[usedTileCount].gameObject.SetActive(true);
        }

        if (count < instancePerTile)
            Debug.LogWarning("Pool exhausted", this);

        return count;
    }
}