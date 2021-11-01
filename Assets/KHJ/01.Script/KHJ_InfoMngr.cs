using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KHJ_InfoMngr : MonoBehaviour
{

    public Image portrait;
    public Sprite[] sprites;
    public Text petname;
    public string[] names;

    public void SetInfo()
    {
        portrait.sprite = sprites[(int)KHJ_SceneMngr.instance.nowPet];
        petname.text = names[(int)KHJ_SceneMngr.instance.nowPet];
    }
}
