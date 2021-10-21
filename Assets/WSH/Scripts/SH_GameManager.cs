using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class SH_GameManager
{
    //CenterPos 중심으로 radius 범위 내의 무작위 좌표 하나를 리턴한다.
    public static Vector3 GetRandomInnerCirclePoint(Vector3 centerPos, float radius)
    {
        var x = Random.Range(-radius, radius);
        float temp = Mathf.Pow(radius, 2) - Mathf.Pow(x, 2);
        var z = Mathf.Sqrt(temp);

        Vector3 result = (new Vector3(x, 0f, z) * Random.Range(0, radius)) + new Vector3(centerPos.x, 0f, centerPos.z);

        return result;
    }
}
