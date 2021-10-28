using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Prize
{
    Ticket,
    G10,
    G5,
    G
};

public class LootBox : MonoBehaviour
{
    public Prize prize;

    public bool OnClicked;
    public bool OnDisappear;
    public Image image;
    public Text text;


    float rotZ;
    float i = 50;
    public float colorA;
    public float colorR;
    public float colorG;
    public float colorB;
    void Start()
    {
        image = GetComponent<Image>();
        colorR = image.color.r;
        colorG = image.color.g;
        colorB = image.color.b;
    }
    void Update()
    {
        if (OnClicked)
        {
            rotZ += i * Time.deltaTime;
            if (rotZ > 7)
            {
                i = -50;
            }
            if (rotZ < -7)
            {
                i = 50;
            }

            transform.eulerAngles = new Vector3(0, 0, rotZ);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if (OnDisappear)
        {
            OnClicked = false;
            GetComponent<Button>().enabled = false;
            colorA -= Time.deltaTime;
            image.color = new Color(colorR, colorG, colorB, colorA);
            if (colorA < 0)
            {
                ShowResult();
            }
        }
    }
    public void Reset()
    {
        GetComponent<Button>().enabled = true;
        OnClicked = false;
        OnDisappear = false;
        colorA = 1;
        image.color = new Color(colorR, colorG, colorB, colorA);
    }
    void ShowResult()
    {
        OnDisappear = false;
        text.text = "";
        image.color = new Color(0.8f, 0.8f, 0.8f, 1);
        transform.eulerAngles = Vector3.zero;
        switch (prize)
        {
            case Prize.Ticket:
                text.text = "Æê »Ì±â±Ç";
                KHJ_SceneMngr.instance.Ticket += 1;
                break;
            case Prize.G10:
                text.text = "10 Gold";
                KHJ_SceneMngr.instance.gold += 10;
                break;
            case Prize.G5:
                text.text = "5 Gold";
                KHJ_SceneMngr.instance.gold += 5;
                break;
            case Prize.G:
                text.text = "1 Gold";
                KHJ_SceneMngr.instance.gold += 1;
                break;
        }
    }
    IEnumerator ClickEvent()
    {
        yield return new WaitForSeconds(1f);
        OnDisappear = true;
        OnClicked = false;
    }

    public void Click()
    {
        if (KHJ_SceneMngr.instance.gold < 5)
            return;
        KHJ_SceneMngr.instance.gold -= 5;
        OnClicked = true;
        GetComponent<Button>().enabled = false;
        StartCoroutine(ClickEvent());
    }
}
