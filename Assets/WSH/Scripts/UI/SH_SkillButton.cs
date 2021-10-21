using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_SkillButton : MonoBehaviour
{
    Image skillSprite;
    Button activeButton;
    Text skillName;
    SH_Skill skill;
    private void Awake()
    {
        skillSprite = GetComponent<Image>();
        activeButton = GetComponent<Button>();
        skillName = GetComponentInChildren<Text>();
    }

    public void CoolTimeUpdate()
    {
        skillSprite.fillAmount = skill.remainingCoolTime;
    }

    public void SetSkill(SH_Skill s)
    {
        skill = s;
        skillSprite.sprite = s.skillSprite;
        skillName.text = s.name;
        activeButton.onClick.AddListener(delegate { s.Active(); });
    }
}
