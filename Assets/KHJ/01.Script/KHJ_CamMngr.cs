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
   

    void Update()
    {
        pet = pets[(int)KHJ_SceneMngr.instance.nowPet];
        Normal = NormalPresets[(int)KHJ_SceneMngr.instance.nowPet];
        Eat = EatPresets[(int)KHJ_SceneMngr.instance.nowPet];
        Play = PlayPresets[(int)KHJ_SceneMngr.instance.nowPet];

        

        if (KHJ_SceneMngr.instance.useAR)
            return;

        if (KHJ_SceneMngr.instance.isEat)
        {
            //밥먹을때
            Camera.main.fieldOfView = 70;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Eat, 5f * Time.deltaTime);
        }
        else if (KHJ_SceneMngr.instance.isBall)
        {
            //공놀이할때
            KHJ_SceneMngr.instance.Ball.GetComponent<DragAndThrow>().enabled = true;
            
            Camera.main.fieldOfView = 70;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Play, 5f * Time.deltaTime);
        }
        else
        {
            //평소
            KHJ_SceneMngr.instance.Ball.GetComponent<DragAndThrow>().enabled = false;
            KHJ_SceneMngr.instance.Ball.transform.SetParent(null);
            KHJ_SceneMngr.instance.Ball.transform.position = new Vector3(82.3f, 0, 0.47f);
            KHJ_SceneMngr.instance.Ball.transform.eulerAngles = new Vector3(0, -25, 0);
            
            Camera.main.fieldOfView = 30;
            transform.position = Vector3.Lerp(transform.position, pet.transform.position + Normal, 5f*Time.deltaTime);

        }




        //카메라 배경색 설정
        if (KHJ_SceneMngr.instance.nowPet == Pet.cat)
            Camera.main.backgroundColor = new Color(148f / 255f, 148f / 255f, 148f / 255f, 0.02f);
        if (KHJ_SceneMngr.instance.nowPet == Pet.bear)
            Camera.main.backgroundColor = new Color(231f / 255f, 253f / 255f, 153f / 255f, 0.02f);
        if (KHJ_SceneMngr.instance.nowPet == Pet.dove)
            Camera.main.backgroundColor = new Color(255f / 255f, 250f / 255f, 212f / 255f, 0.02f);

    }

}
