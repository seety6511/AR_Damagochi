using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_PhoneCamera : MonoBehaviour
{
    bool camAvailable;
    WebCamTexture backCam;
    Texture defaultBackground;

    public RawImage backGround;
    public AspectRatioFitter fit;

    private void Start()
    {
        defaultBackground = backGround.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("NO Cam");
            camAvailable = false;
        }

        foreach(var d in devices)
        {
            if (!d.isFrontFacing)
                backCam = new WebCamTexture(d.name, Screen.width, Screen.height);
        }

        if (backCam == null)
        {
            Debug.Log("Cant Find Back Cam");
            camAvailable = false;
            return;
        }

        backCam.Play();
        backGround.texture = backCam;
        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;
    }
}