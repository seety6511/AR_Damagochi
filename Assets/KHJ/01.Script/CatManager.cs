using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public GameObject Ball;
    public GameObject shootPosition;
    void Start()
    {

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
        tObj.transform.position = GetMouseWorldPos() + mOffset;
    }

    void Update()
    {
        
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



    public bool solution(string s)
    {
        bool answer = true;
        if (s.Length == 4 || s.Length == 6)
        {
            int i = 0;
            answer = int.TryParse(s, out i);
        }
        return answer;
    }
}
