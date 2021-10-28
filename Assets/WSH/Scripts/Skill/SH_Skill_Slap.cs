using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Slap : SH_Skill
{
    public override bool Active()
    {
        if (base.Active())
            owner.attackTarget.Damaged(owner.atk * damage);

        return true;
    }
}
