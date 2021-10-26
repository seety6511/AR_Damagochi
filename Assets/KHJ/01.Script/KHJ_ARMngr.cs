using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class KHJ_ARMngr : MonoBehaviour
{
    public ARRaycastManager arRayManager;
    public GameObject Indicator;
    public GameObject sphere;

    void Start()
    {
        arRayManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!KHJ_SceneMngr.instance.useAR)
            return;


        print("AR On");
        //1.ī�޶� ��ġ, ī�޶� �չ��⿡�� �߻�Ǵ� Ray�� �����
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (arRayManager.Raycast(ray, hits))
        {
            SetIndecator(hits[0].pose.position);
        }
        else
        {
            Indicator.SetActive(false);
        }
    }


    void SetIndecator(Vector3 pos)
    {
        //ī�޶󿡼� ������ ray�� ��� ��ġ�� Indicator�� ���´�
        Indicator.SetActive(true);
        Indicator.transform.position = pos;
        //Indecator�� ������ 
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;
        Indicator.transform.forward = dir;
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(sphere);
            sphere.transform.position = Indicator.transform.position;
            CatManager.instance.gameObject.transform.position = Indicator.transform.position;
        }
    }
}
