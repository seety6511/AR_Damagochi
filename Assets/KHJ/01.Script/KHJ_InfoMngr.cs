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
    public Text[] stats;
    public Text level;
    public void SetInfo()
    {
        portrait.sprite = sprites[(int)KHJ_SceneMngr.instance.nowPet];
        petname.text = names[(int)KHJ_SceneMngr.instance.nowPet];
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].text = (KHJ_SceneMngr.instance.pet.stat[i] + KHJ_SceneMngr.instance.pet.Level * 2).ToString();
        }
        level.text = "Lv." + (KHJ_SceneMngr.instance.pet.Level + 1).ToString();
    }
}
