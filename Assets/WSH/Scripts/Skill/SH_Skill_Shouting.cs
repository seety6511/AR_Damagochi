using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Shouting : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        var temp = owner.attackTarget.atkSpeed;
        owner.attackTarget.atkSpeed *= 0.5f;
        yield return new WaitForSecondsRealtime(10f);
        owner.attackTarget.atkSpeed = temp;
    }
}
