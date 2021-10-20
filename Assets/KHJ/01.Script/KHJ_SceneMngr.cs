using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KHJ_SceneMngr : MonoBehaviour
{
    //재화
    public TMP_Text gold;
    public TMP_Text dia;
    //유대감
    public float currH = 0;
    float maxH = 100;
    public Image Hvalue;

    //공 던지기
    public bool isBall;
    public GameObject Ball;
    public GameObject shootPosition;

    void Start()
    {
        
    }

    void Update()
    {
        Hvalue.fillAmount = currH / maxH;
        BallPlayingCam();
    }




    void BallPlayingCam()
    {
        if (isBall)
        {
            Ball.GetComponent<DragAndThrow>().enabled = true;
            //Ball.GetComponent<DragAndThrow>().Reset();
            Camera.main.fieldOfView = 70;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -0.6f);
        }
        else
        {
            Ball.GetComponent<DragAndThrow>().enabled = false;
            Ball.transform.SetParent(null);
            Ball.transform.position = new Vector3(82.3f, 0, 0.47f);
            Ball.transform.eulerAngles = new Vector3(0, -25, 0);
            Camera.main.fieldOfView = 25;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -2.9f);
        }
    }





    public void ShootBall()
    {
        //클릭한 곳으로 물체 쏘기
        GameObject tObj = Instantiate(Ball);
        tObj.transform.position = shootPosition.transform.position;
        Vector3 force = (shootPosition.transform.position - Camera.main.transform.position).normalized;

        Rigidbody tR = tObj.GetComponent<Rigidbody>();

        tR.AddForce(force * 100f);
    }

}
