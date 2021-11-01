using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SH_DamgochiBattleUI : MonoBehaviour
{
    public SH_ActionDamagochi owner;
    public Image hpBar;
    public Image turnGage;

    public void UpdateHpBar()
    {
        var percent = owner.hp / owner.maxHp *0.1f;
        turnGage.gameObject.transform.DOKill();
        hpBar.gameObject.transform.DOScale(new Vector3(percent, 0.1f, 0.1f), 1f);
        //hpBar.gameObject.transform.localScale = new Vector3(percent, 0.1f, 0.1f);
    }

    public void UpdateTurnGage()
    {
        var percent = owner.currentTurnGage / owner.maxTurnGage * 0.1f;
        turnGage.gameObject.transform.DOKill();
        turnGage.gameObject.transform.DOScale(new Vector3(percent, 0.1f, 0.1f), 1f);
        //turnGage.transform.localScale = new Vector3(percent, 0.1f, 0.1f);
    }
}
