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
        stats[0].text = (KHJ_SceneMngr.instance.pet.hp + KHJ_SceneMngr.instance.pet.Level * 2).ToString();
        stats[1].text = (KHJ_SceneMngr.instance.pet.atk + KHJ_SceneMngr.instance.pet.Level * 2).ToString();
        stats[2].text = (KHJ_SceneMngr.instance.pet.speed + KHJ_SceneMngr.instance.pet.Level * 2).ToString();
        level.text = "Lv." + (KHJ_SceneMngr.instance.pet.Level + 1).ToString();
    }
}
