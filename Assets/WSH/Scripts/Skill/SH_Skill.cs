using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill : MonoBehaviour
{
    public SH_ActionDamagochi owner;
    public new string name;
    public float coolTime;
    public float timer;
    public bool canActive;
    public float damage;

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

    public virtual void HitEvent()
    {
    }

    public virtual void Active()
    {
        if (owner.currentTurnGage < owner.maxTurnGage)
            return;

        if (!canActive)
            return;

        if (!owner.canAnim)
            return;

        if (activeEffect != null)
            activeEffect.SetActive(false);

        if (hitEffect != null)
            hitEffect.SetActive(false);

        canActive = false;
        timer = 0f;

        owner.currentTurnGage = 0f;
        owner.battleUI.UpdateTurnGage();
        SH_BattleLogger.Instance.LogText("Active Skill_" + owner.name + "_" + name, Color.black);

        owner.battleState = SH_ActionDamagochi.BattleState.TurnWaiting;

        owner.AnimationChange(name);
        if (activeEffect != null)
            activeEffect.SetActive(true);
    }
}