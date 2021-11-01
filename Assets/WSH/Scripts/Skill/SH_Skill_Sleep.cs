using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Sleep : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.Heal(owner.atk * damage);
        yield return null;
    }
}
