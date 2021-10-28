using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIfAnimation : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public bool isEnd;
    public float colorA;
    public GameObject ExitBtn;
    void Start()
    {
        colorA = image.color.a;
    }

    void Update()
    {
        if (isEnd)
        {
            if (colorA < 0)
            {
                //결과 표시
                ShowResult();
                ExitBtn.SetActive(true);
                return;
            }
            colorA -= Time.deltaTime;
            Color tmp = image.color;
            tmp.a = colorA;
            image.color = tmp;
        }


        //if(image.sprite == sprites[sprites.Length - 1])
        //{
        //    isEnd = true;
        //    return;
        //}
        //image.sprite = sprites[(int)(Time.time * 30)%sprites.Length];
    }

    public int now;
    public void Reset()
    {
        GetComponent<Button>().enabled = true;
        ExitBtn.SetActive(false);
        now = 0;
        isEnd = false;
        colorA = 1;

        Color tmp = image.color;
        tmp.a = colorA;
        image.color = tmp;
        image.sprite = sprites[now];

        resultImg.enabled = false;
        resultTxt.enabled = false;
    }
    int r;
    public void Play()
    {
        r = Random.Range(0, results.Length);
        StartCoroutine(PlayGif());
    }
    IEnumerator PlayGif()
    {
        while (true)
        {
            image.sprite = sprites[now];
            yield return null;
            yield return null;
            yield return null;
            now++;

            if (image.sprite == sprites[sprites.Length - 1])
            {
                isEnd = true;
                break;
            }
        }
    }

    public Image resultImg;
    public Sprite[] results;
    public Text resultTxt;
    void ShowResult()
    {        
        resultImg.enabled = true;
        resultTxt.enabled = true;
        resultImg.sprite = results[r];
        switch (r)
        {
            case 0:
                resultTxt.text = "냥냥이";
                break;
            case 1:
                resultTxt.text = "곰돌이";                
                break;
            case 2:
                resultTxt.text = "구구";
                
                break;
        }
    }
}
