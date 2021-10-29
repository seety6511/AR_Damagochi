using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Growling : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.currentTurnGage += owner.maxTurnGage*0.5f;
        var temp = owner.atk;
        owner.atk *= 2f;
        while (owner.currentTurnGage != 0)
        {
            yield return null;
        }
        owner.atk = temp;
    }
}
