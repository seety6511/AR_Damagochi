using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SH_FirstSetting : MonoBehaviour
{
    SH_ARInputManager arInputManager;
    ARPlaneManager arPlaneManager;
    ARAnchorManager arAnchorManager;
    SH_SystemManager systemManager;

    public Text info;
    public Button reset;
    public Button ok;

    public GameObject anchor;
    public GameObject settingEffect;
    public bool firstSetting;

    private void Start()
    {
#if UNITY_EDITOR
        gameObject.SetActive(false);
        Debug.Log("First Plane Setting Start");
        arInputManager = FindObjectOfType<SH_ARInputManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arAnchorManager = FindObjectOfType<ARAnchorManager>();
        systemManager = FindObjectOfType<SH_SystemManager>();
        PlaneSetting();
#elif UNITY_ANDROID
        gameObject.SetActive(true);
        Debug.Log("First Plane Setting Start");
        arInputManager = FindObjectOfType<SH_ARInputManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arAnchorManager = FindObjectOfType<ARAnchorManager>();
        systemManager = FindObjectOfType<SH_SystemManager>();
        reset.onClick.AddListener(ResetPlane);
        ok.onClick.AddListener(PlaneSetting);
#endif

    }

    private void Update()
    {
        FirstAnchorSetting();
    }

    void FirstAnchorSetting()
    {
        if (firstSetting)
            return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (arInputManager.ARRayCast(ray))
        {
            if (arInputManager.hits.Count == 0)
                return;
            anchor.gameObject.SetActive(true);
            var pos = arInputManager.hits[0].pose.position;
            pos.y += 0.5f;
            anchor.transform.position = pos; 
        }
        else
            anchor.gameObject.SetActive(false);
    }

    void ResetPlane()
    {
        firstSetting = false;
        settingEffect.SetActive(false);
    }

    void PlaneSetting()
    {
        Debug.Log("First Plane Setting Done");
        systemManager.SetFirstPlane(anchor);
        firstSetting = true;
        settingEffect.transform.position = anchor.transform.position;
        settingEffect.SetActive(true);
        anchor.SetActive(false);
        gameObject.SetActive(false);
    }
}
