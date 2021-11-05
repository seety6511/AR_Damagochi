using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SH_UI_DamagochiBattle : MonoBehaviour
{
    public Slider hp;
    public Slider speed;
    SH_ActionDamagochi owner;

    public void SetOwner(SH_ActionDamagochi ad)
    {
        gameObject.SetActive(true);
        owner = ad;
        owner.battleUI = this;
        HpUpdate();
        SpeedUpdate();
    }
    public void HpUpdate()
    {
        var temp = owner.hp / owner.maxHp;
        hp.value = temp;
    }

    public void SpeedUpdate()
    {
        var temp = owner.currentTurnGage / owner.maxTurnGage;
        speed.value = temp;
    }
}
