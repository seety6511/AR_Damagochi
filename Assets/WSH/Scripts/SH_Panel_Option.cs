using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SH_Panel_Option : MonoBehaviour
{
    public Dropdown occlusion;
    public InputField arHeight;

    AROcclusionManager om;

    private void OnEnable()
    {
        occlusion.value = (int)om.currentEnvironmentDepthMode;
        arHeight.text = ""+FindObjectOfType<SH_Player>().transform.position.y;
    }
    public void SaveAndApply()
    {
    }
}
