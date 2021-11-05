using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_NyangNyangPunch : SH_Skill
{
    public float turnGageDamage;
    protected override IEnumerator SpecialEffect()
    {
        owner.SpeedChange(3f);
        for(int i = 0; i < 3; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            owner.attackTarget.Damaged(owner.atk * damage);
            owner.AnimationChange(name);
            owner.attackTarget.TurnGageChange(turnGageDamage);
        }
        owner.SpeedChange(1f);
    }
}
