using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Panel_SkillInfo : MonoBehaviour
{
    public Image portrait;
    public new Text name;
    public Text damage;
    public Text info;
    public SH_Skill skill;

    public void SetSkill(SH_Skill s)
    {
        skill = s;
        portrait.sprite = s.skillSprite;
        name.text = s.name;
        damage.text = "°è¼ö : " + s.damage;
        info.text = s.info;
        gameObject.SetActive(true);
    }
}
