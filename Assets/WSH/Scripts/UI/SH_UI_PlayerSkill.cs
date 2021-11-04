using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_UI_PlayerSkill : MonoBehaviour
{
    public SH_DamagochiTrainer owner;
    public SH_SkillButton[] skillButtons;
    SH_Skill[] skills;

    public void On(SH_DamagochiTrainer dt)
    {
        owner = dt;
        SetSkills();
        gameObject.SetActive(true);
    }

    void SetSkills()
    {
        skills = owner.damagochi.skillList;
        for(int i = 0; i < owner.damagochi.skillList.Length; ++i)
        {
            skillButtons[i].SetSkill(skills[i]);
        }
    }
}
