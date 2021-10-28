using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Shouting : SH_Skill
{
    public override bool Active()
    {
        base.Active();
        return true;
    }

    protected override IEnumerator SpecialEffect()
    {
        var temp = owner.attackTarget.atkSpeed;
        owner.attackTarget.atkSpeed *= 0.5f;
        yield return new WaitForSecondsRealtime(2f);
        owner.attackTarget.atkSpeed = temp;
    }
}
