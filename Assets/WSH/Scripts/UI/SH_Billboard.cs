using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Billboard : MonoBehaviour
{
    private void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
    }
}
