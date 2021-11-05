using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_UI_GPSDebug : MonoBehaviour
{
    public Text latitude;
    public Text longitude;
    public Text currZoom;
    public Text maxZoom;

    public void DebugUpdate(SH_GoogleMap gm)
    {
        latitude.text = gm.latitude.ToString();
        longitude.text = gm.longitude.ToString();
        currZoom.text = gm.curZoom.ToString();
        maxZoom.text = gm.maxZoom.ToString();
    }
}
