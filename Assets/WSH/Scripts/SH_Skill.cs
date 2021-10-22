using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill : MonoBehaviour
{
    public SH_ActionDamagochi owner;
    public new string name;
    public float coolTime;
    float timer;
    public bool canActive;

    public Sprite skillSprite;
    public GameObject activeEffect;
    public GameObject hitEffect;

    public float remainingCoolTime
        => timer / coolTime;

    public void CoolTimeUpdate()
    {
        timer += Time.deltaTime;
        if (timer > coolTime)
            canActive = true;
    }

    public virtual void Active()
    {
        if (owner.currentTurnGage < owner.maxTurnGage)
            return;

        if (!canActive)
            return;

        canActive = false;
        timer = 0f;

        owner.currentTurnGage = 0f;
        owner.battleUI.UpdateTurnGage();
        FindObjectOfType<SH_TextLogControl>().LogText("Active Skill_" + owner.name + "_" + name, Color.black);

        owner.battleState = SH_ActionDamagochi.BattleState.Attaking;

        if (activeEffect != null)
            activeEffect.SetActive(true);
    }
}