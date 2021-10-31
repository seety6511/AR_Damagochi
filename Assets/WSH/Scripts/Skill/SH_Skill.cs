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
    public AudioClip activeSound;
    public AudioClip hitSound;
    public float remainingCoolTime
        => timer / coolTime;

    public void CoolTimeUpdate()
    {
        timer += Time.deltaTime;
        if (timer > coolTime)
            canActive = true;
    }

    protected virtual void HitEvent()
    {
        if (hitEffect != null)
        {
            if (owner.attackTarget.hitPoints.Length != 0)
                hitEffect.transform.position = owner.attackTarget.GetRandomHitPoint().transform.position;
            else
                hitEffect.transform.position = owner.attackTarget.transform.position;

            hitEffect.SetActive(true);
        }
        if (hitSound != null)
        {
            owner.PlaySound(hitSound);
        }
    }

    protected virtual void EndEvent()
    {
    }

    public virtual void Active()
    {
        if (owner.currentTurnGage < owner.maxTurnGage)
            return;

        if (!canActive)
            return;

        if (activeEffect != null)
            activeEffect.SetActive(false);

        if (hitEffect != null)
            hitEffect.SetActive(false);

        owner.doEvent += HitEvent;
        owner.endEvent += EndEvent;

        canActive = false;
        timer = 0f;

        owner.currentTurnGage = 0f;
        owner.battleUI.UpdateTurnGage();
        SH_BattleLogger.Instance.LogText("Active Skill_" + owner.name + "_" + name, Color.black);

        owner.battleState = SH_ActionDamagochi.BattleState.TurnWaiting;

        owner.AnimationChange(name);
        owner.currentActiveSkill = this;
        
        if (activeEffect != null)
            activeEffect.SetActive(true);

        if (activeSound != null)
            owner.PlaySound(activeSound);
        
        Debug.Log("ActiveSkill : " + name);
        StartCoroutine("SpecialEffect");
    }

    protected virtual IEnumerator SpecialEffect()
    {
        yield return null;
    }
}