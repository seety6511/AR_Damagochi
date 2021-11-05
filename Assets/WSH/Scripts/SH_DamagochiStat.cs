using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_DamagochiStat : MonoBehaviour
{
    public new Text name;
    public Text hp;
    public Text atk;
    public Text speed;
    public Text message;
    public Image immoticon;
    public SH_Button_SkillInfo[] skillInfoButtons;

    public Sprite[] immos;

    private void Awake()
    {
        skillInfoButtons = GetComponentsInChildren<SH_Button_SkillInfo>();
    }

    public void StatUpdate(SH_ActionDamagochi ad)
    {
        name.text = "LV." + ad.level + " " + ad.name;
        hp.text = "HP : " + ad.hp + " / " + ad.maxHp;
        atk.text = "ATK : " + ad.atk;
        speed.text = "SPEED : " + ad.atkSpeed;

        for (int i = 0; i < ad.skillList.Length; ++i)
        {
            skillInfoButtons[i].SetSkill(ad.skillList[i]);
        }

        var immo = ad.hp/ ad.maxHp;
        Sprite result;

        if (immo > 0.9f)
            result = immos[0];
        else if (immo > 0.5f)
            result = immos[1];
        else if (immo > 0.3f)
            result = immos[2];
        else if (immo > 0.1f)
            result = immos[3];
        else
            result = immos[4];

        immoticon.sprite = result;
            
    }

}
