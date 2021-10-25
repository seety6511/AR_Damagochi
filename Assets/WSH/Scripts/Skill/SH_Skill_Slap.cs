using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Slap : SH_Skill
{
    public override void Active()
    {
        base.Active();
        owner.onEvent = null;
        owner.onEvent += HitEvent;
    }

    public override void HitEvent()
    {
        base.HitEvent();
        hitEffect.transform.position = owner.hitPoints[Random.Range(0, owner.hitPoints.Length)].transform.position;
        hitEffect.gameObject.SetActive(true);
    }
}
