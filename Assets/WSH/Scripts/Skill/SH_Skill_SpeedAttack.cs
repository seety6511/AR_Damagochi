using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_SpeedAttack : SH_Skill
{
    public override bool Active()
    {
        if (base.Active())
        {
            owner.attackTarget.Damaged(owner.currentTurnGage * damage);
            owner.currentTurnGage += owner.atkSpeed * 8f;
        }
        return true;
    }
}
