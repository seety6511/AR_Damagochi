using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Panel_Battle : MonoBehaviour
{
    public SH_Player owner;
    public SH_SkillButton[] skillButtons;
    List<SH_Skill> skills;

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void SkillButtonUpdate()
    {
        foreach(var b in skillButtons)
        {
            b.CoolTimeUpdate();
        }
    }
    public void SetSkills(SH_ActionDamagochi dama)
    {
        skills = dama.skillList;
        for(int i = 0; i < dama.skillList.Count; ++i)
        {
            skillButtons[i].SetSkill(skills[i]);
        }
    }
}
