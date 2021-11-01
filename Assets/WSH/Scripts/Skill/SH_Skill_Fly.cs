using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SH_Skill_Fly : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        owner.transform.DOKill();
        owner.transform.DOMoveY(3f, 2f);
        owner.overPower = true;

        yield return new WaitForSecondsRealtime(3f);
        owner.overPower = false;
    }
}

