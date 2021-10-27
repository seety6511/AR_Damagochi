using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[DefaultExecutionOrder(-200)]
public class SH_NavMeshTag : MonoBehaviour
{
    public static List<MeshFilter> meshes = new List<MeshFilter>();
    public static List<Terrain> terrains = new List<Terrain>();
    void OnEnable()
    {
        var m = GetComponent<MeshFilter>();
        var t = GetComponent<Terrain>();
        if (m != null)
            meshes.Add(m);

        if (t != null)
            terrains.Add(t);
    }

    void OnDisable()
    {
        var m = GetComponent<MeshFilter>();
        var t = GetComponent<Terrain>();

        if (m != null)
            meshes.Remove(m);

        if (t != null)
            terrains.Remove(t);
    }

    public static void Collect(ref List<NavMeshBuildSource> sources)
    {
        sources.Clear();

        for (var i = 0; i < meshes.Count; ++i)
        {
            var mf = meshes[i];
            if (mf == null) continue;

            var m = mf.sharedMesh;
            if (m == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Mesh;
            s.sourceObject = m;
            s.transform = mf.transform.localToWorldMatrix;
            s.area = 0;
            sources.Add(s);
        }

        for (var i = 0; i < terrains.Count; ++i)
        {
            var t = terrains[i];
            if (t == null) continue;

            var s = new NavMeshBuildSource();
            s.shape = NavMeshBuildSourceShape.Terrain;
            s.sourceObject = t.terrainData;
            s.transform = Matrix4x4.TRS(t.transform.position, Quaternion.identity, Vector3.one);
            s.area = 0;
            sources.Add(s);
        }
    }
}