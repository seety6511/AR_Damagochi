using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

[DefaultExecutionOrder(-102)]
public class SH_NavMeshBuilder : MonoBehaviour
{
    public Transform center;

    public Vector3 builderSize = new Vector3(80.0f, 20.0f, 80.0f);

    NavMeshData navMeshData;
    AsyncOperation oper;
    NavMeshDataInstance instance;
    List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

    public void Init()
    {
        navMeshData = new NavMeshData();
        instance = NavMesh.AddNavMeshData(navMeshData);
        if (center == null)
            center = transform;
        UpdateNavMesh(false);
    }

    public IEnumerator NavMeshUpdate()
    {
        while (true)
        {
            UpdateNavMesh(true);
            yield return oper;
        }
    }

    void OnDisable()
    {
        instance.Remove();
    }

    void UpdateNavMesh(bool asyncUpdate = false)
    {
        SH_NavMeshTag.Collect(ref sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = QuantizedBounds();

        if (asyncUpdate)
            oper = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, defaultBuildSettings, sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(navMeshData, defaultBuildSettings, sources, bounds);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        var center = this.center ? this.center.position : transform.position;
        return new Bounds(Quantize(center, 0.1f * builderSize), builderSize);
    }

    void OnDrawGizmosSelected()
    {
        if (navMeshData)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(navMeshData.sourceBounds.center, navMeshData.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = this.center ? this.center.position : transform.position;
        Gizmos.DrawWireCube(center, builderSize);
    }
}