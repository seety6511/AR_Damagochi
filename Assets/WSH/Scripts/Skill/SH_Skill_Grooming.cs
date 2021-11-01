using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Grooming : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        var tempAtk = owner.atk;
        var tempSpeed = owner.atkSpeed;
        owner.atk += owner.atk * 0.1f;
        owner.atkSpeed += owner.atkSpeed * 0.1f;
        while (owner.actionState == SH_ActionDamagochi.ActionState.Battle)
        {
            yield return null;
        }

        owner.atk = tempAtk;
        owner.atkSpeed = tempSpeed;
    }
}
