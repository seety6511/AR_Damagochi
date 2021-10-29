using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Slap : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.attackTarget.Damaged(owner.atk * damage);
        yield return null;
    }
}
