using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SH_FirstSetting : MonoBehaviour
{
    public Text info;
    public Button reset;
    public Button ok;

    [SerializeField]
    SH_ARInputManager arInputManager;
    [SerializeField]
    ARPlaneManager arPlaneManager;
    [SerializeField]
    ARAnchorManager arAnchorManager;

    public GameObject anchor;
    private void Start()
    {
        arInputManager = FindObjectOfType<SH_ARInputManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arAnchorManager = FindObjectOfType<ARAnchorManager>();
        reset.onClick.AddListener(ResetPlane);
        ok.onClick.AddListener(PlaneSetting);
    }

    private void Update()
    {
    }

    void ResetPlane()
    {
    }

    void PlaneSetting()
    {
    }
}
