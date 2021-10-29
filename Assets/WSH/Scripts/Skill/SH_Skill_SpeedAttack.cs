using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_SpeedAttack : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.attackTarget.Damaged(owner.currentTurnGage * damage);
        owner.currentTurnGage += owner.maxTurnGage * 0.8f;
        yield return null;
    }
}
