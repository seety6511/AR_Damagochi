using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Sleep : SH_Skill
{
    public override bool Active()
    {
        if (base.Active())
        {
        owner.Heal(owner.atk * damage);
        }
        return true;
    }
}
