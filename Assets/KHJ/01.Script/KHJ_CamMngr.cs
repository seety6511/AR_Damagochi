using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_CamMngr : MonoBehaviour
{
    public GameObject[] pets;
    GameObject pet;
    public Vector3[] NormalPresets;
    public Vector3[] EatPresets;
    public Vector3[] PlayPresets;
    Vector3 Normal;
    Vector3 Eat;
    Vector3 Play;

    public Transform[] BallPos;

    void LateUpdate()
    {
        pet = pets[(int)KHJ_SceneMngr.instance.nowPet];
        Normal = NormalPresets[(int)KHJ_SceneMngr.instance.nowPet];
        Eat = EatPresets[(int)KHJ_SceneMngr.instance.nowPet];
        Play = PlayPresets[(int)KHJ_SceneMngr.instance.nowPet];

        int nowpet = (int)KHJ_SceneMngr.instance.nowPet;

        if (KHJ_SceneMngr.instance.useAR)
            return;

        if (KHJ_SceneMngr.instance.isEat)
        {
            //�������
            Camera.main.fieldOfView = 70;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Eat, 5f * Time.deltaTime);
        }
        else if (KHJ_SceneMngr.instance.isBall)
        {
            //�������Ҷ�
            KHJ_SceneMngr.instance.Ball[nowpet].GetComponent<DragAndThrow>().enabled = true;
            
            Camera.main.fieldOfView = 70;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Play, 5f * Time.deltaTime);
        }
        else
        {
            //���
            SetBallPos();

            Camera.main.fieldOfView = 30;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Normal, 5f*Time.deltaTime);

        }

        //ī�޶� ���� ����
        if (KHJ_SceneMngr.instance.nowPet == Pet.cat)
            Camera.main.backgroundColor = new Color(148f / 255f, 148f / 255f, 148f / 255f, 0.02f);
        if (KHJ_SceneMngr.instance.nowPet == Pet.bear)
            Camera.main.backgroundColor = new Color(231f / 255f, 253f / 255f, 153f / 255f, 0.02f);
        if (KHJ_SceneMngr.instance.nowPet == Pet.dove)
            Camera.main.backgroundColor = new Color(255f / 255f, 250f / 255f, 212f / 255f, 0.02f);

    }

    public void BallReset()
    {
        int nowpet = (int)KHJ_SceneMngr.instance.nowPet;

        KHJ_SceneMngr.instance.Ball[nowpet].GetComponent<DragAndThrow>().Reset();
    }
    public void BallCancel()
    {
        int nowpet = (int)KHJ_SceneMngr.instance.nowPet;
        KHJ_SceneMngr.instance.Ball[nowpet].GetComponent<DragAndThrow>().enabled = true;
        KHJ_SceneMngr.instance.Ball[nowpet].GetComponent<DragAndThrow>().Cancel();
    }
    void SetBallPos()
    {
        for(int i = 0; i < BallPos.Length; i++)
        {
            KHJ_SceneMngr.instance.Ball[i].GetComponent<DragAndThrow>().enabled = false;
            KHJ_SceneMngr.instance.Ball[i].transform.SetParent(null);
            KHJ_SceneMngr.instance.Ball[i].transform.position = BallPos[i].position;
            KHJ_SceneMngr.instance.Ball[i].transform.eulerAngles = new Vector3(0, -25, 0);
        }
    }
}
