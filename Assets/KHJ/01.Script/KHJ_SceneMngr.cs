using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KHJ_SceneMngr : MonoBehaviour
{
    public static KHJ_SceneMngr instance;
    CatManager cat;

    public GameObject Panel;

    //��ȭ
    public TMP_Text goldUI;
    public TMP_Text diaUI;
    int gold = 100;
    int dia = 100;

    //���밨
    public float currH = 0;
    float maxH = 100;
    public Image IntimacyImg;
    public Image IntimacyBar;
    public Sprite[] ImmoSprites;

    //�� ������
    public bool isBall;
    public GameObject Ball;
    public GameObject shootPosition;

    //��Ա�
    public bool isEat;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        cat = CatManager.instance;
    }
    void Update()
    {
        goldUI.text = gold.ToString();
        diaUI.text = dia.ToString();
        IntimacyBar.fillAmount = currH / maxH;
        BallPlayingCam();
        //FoodCam();
    }


    public void isBallChange()
    {
        isBall = !isBall;
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

    void FoodCam()
    {
        if (isEat)
        {
            Camera.main.fieldOfView = 70;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -0.6f);
        }
        else
        {
            Camera.main.fieldOfView = 25;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -2.9f);
        }
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

    
    public IEnumerator EatCoroutine(int i)
    {
        //�� �Ա� ���
        cat.GetComponent<SceneAnimatorController>().SetAnimatorString("isEatting");
        Panel.SetActive(false);
        yield return new WaitForSeconds(5);

        switch (i)
        {
            case 0:
                break;
            case 1:
                DecreaseGold(10);
                break;
            case 2:
                DecreaseDia(10);
                break;
        }
        cat.GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
        Panel.SetActive(true);
    }

    public void Eat(int i)
    {
        StartCoroutine(EatCoroutine(i));        
    }

    public bool DecreaseGold(int i)
    {
        if(gold-i < 0)
        {
            return false;
        }
        else
        {
            gold -= i;
            return true;
        }
    }
    public bool DecreaseDia(int i)
    {
        if (dia - i < 0)
        {
            return false;
        }
        else
        {
            dia -= i;
            return true;
        }
    }


}
