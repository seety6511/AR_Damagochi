using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KHJ_SceneMngr : MonoBehaviour
{
    //��ȭ
    public TMP_Text gold;
    public TMP_Text dia;
    //���밨
    public float currH = 0;
    float maxH = 100;
    public Image Hvalue;

    //�� ������
    public GameObject Ball;
    public GameObject shootPosition;

    void Start()
    {
        
    }

    void Update()
    {
        Hvalue.fillAmount = currH / maxH;
    }


    private Vector3 mOffset;
    float mZCoord;
    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        GameObject tObj = Instantiate(Ball);

    }

    private Vector3 GetMouseWorldPos()
    {
        //pixel coordinate (x,y)
        Vector3 mousePoint = Input.mousePosition;
        //z coordinate of game object on screen
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseUp()
    {

    }

    private void OnMouseDrag()
    {
        //tObj.transform.position = GetMouseWorldPos() + mOffset;
    }


    public void ShootBall()
    {
        //Ŭ���� ������ ��ü ���
        GameObject tObj = Instantiate(Ball);
        tObj.transform.position = shootPosition.transform.position;
        Vector3 force = (shootPosition.transform.position - Camera.main.transform.position).normalized;

        Rigidbody tR = tObj.GetComponent<Rigidbody>();

        tR.AddForce(force * 100f);
    }

}
