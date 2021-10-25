using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Panel_MyStatus : MonoBehaviour
{
    public SH_ActionDamagochi target;

    public Text damagoName;
    public Text hp;
    public Text atk;
    public Text critical;
    public Text criticalDamage;
    public Text atkSpeed;

    public List<Image> skillImages;
    public List<Text> skillNames;
    public void SetDamagochi(SH_Player player)
    {
        target = player.controlDamagochi;

        damagoName.text = target.name;
        hp.text = target.hp + " / " + target.maxHp;
        atk.text = target.atk.ToString();
        critical.text = target.critical.ToString();
        criticalDamage.text = target.criticalDamage.ToString();
        atkSpeed.text = target.atkSpeed.ToString();

        for(int i = 0; i < skillImages.Count; ++i)
        {
            skillImages[i].sprite = target.skillList[i].skillSprite;
            skillNames[i].text = target.skillList[i].name;
        }
        gameObject.SetActive(true);
    }
}
