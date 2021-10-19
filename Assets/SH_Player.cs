using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Player : MonoBehaviour
{
    Camera cam;
    public Damagochi controlTarget;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        MouseAction();
    }

    public void MouseAction()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (controlTarget == null)
            return;

        Vector3 mos = Input.mousePosition;
        mos.z = cam.farClipPlane; 

        Vector3 dir = cam.ScreenToWorldPoint(mos);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, mos.z))
        {
            controlTarget.gameObject.transform.position = hit.point; 
        }
    }
}
