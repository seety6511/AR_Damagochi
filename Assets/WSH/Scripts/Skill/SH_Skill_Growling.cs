using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Growling : SH_Skill
{
    public override bool Active()
    {
        if (base.Active())
            owner.currentTurnGage += 50f;
        return true;
    }

    protected override IEnumerator SpecialEffect()
    {
        var temp = owner.atk;
        owner.atk *= 2f;
        while (owner.currentTurnGage != 0)
        {
            yield return null;
        }
        owner.atk = temp;
    }
}
