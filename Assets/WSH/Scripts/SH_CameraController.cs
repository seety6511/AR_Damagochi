using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SH_CameraController : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public float scrollSpeed;
    Vector3 rotOffset;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
        rotOffset = cam.transform.localEulerAngles;
    }

    private void Update()
    {
        CameraMove();
        CameraRot();
    }

    void CameraRot()
    {
        if (!Input.GetMouseButton(1))
            return;

        var h = Input.GetAxisRaw("Mouse X");
        var v = Input.GetAxisRaw("Mouse Y");
        var dir = new Vector3(-v,h,0);
        rotOffset += dir * rotSpeed * Time.deltaTime;
        cam.transform.localEulerAngles = rotOffset;
    }

    void CameraMove()
    {
        var dir = Vector3.zero;
        float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (Input.GetKey(KeyCode.A))
            dir -= transform.right;
        if (Input.GetKey(KeyCode.D))
            dir += transform.right;
        if (Input.GetKey(KeyCode.W))
            dir += transform.forward;
        if (Input.GetKey(KeyCode.S))
            dir -= transform.forward;

        dir.y = scroll;

        gameObject.transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
