using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Bite : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.attackTarget.Damaged(owner.atk * damage);
        yield return null;
    }
}
