using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Button_SkillInfo : MonoBehaviour
{
    Button button;
    Image skillImage;
    Text skillName;
    public SH_Skill skill;
    public SH_Panel_SkillInfo ps;

    private void Start()
    {
        button = GetComponent<Button>();
        skillImage = GetComponent<Image>();
        skillName = GetComponentInChildren<Text>();
        button.onClick.AddListener(OpenSkillInfo);
    }

    public void SetSkill(SH_Skill skill)
    {
        this.skill = skill;
        skillImage.sprite = skill.skillSprite;
        //skillName.text = skill.name;
    }
    void OpenSkillInfo()
    {
        ps.SetSkill(skill);
    }
}
