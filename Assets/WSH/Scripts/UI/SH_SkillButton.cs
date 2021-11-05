using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_SkillButton : MonoBehaviour
{
    public Image skillSprite;
    public Button activeButton;
    public Text skillName;
    public SH_Skill skill;

    public void CoolTimeUpdate()
    {
        skillSprite.fillAmount = skill.remainingCoolTime;
    }

    public void SetSkill(SH_Skill s)
    {
        skill = s;
        skillSprite.sprite = s.skillSprite;
        skillName.text = s.name;
        activeButton.onClick.RemoveAllListeners();
        activeButton.onClick.AddListener(delegate { ButtonActive(); });
    }
    
    void ButtonActive()
    {
        skill.Active();
        skillSprite.fillAmount = 0f;
    }
}
