using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill : MonoBehaviour
{
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
        if (timer >= coolTime)
            canActive = true;
    }

    public virtual void Active()
    {
        if (!canActive)
            return;
        activeEffect?.SetActive(true);
    }
}