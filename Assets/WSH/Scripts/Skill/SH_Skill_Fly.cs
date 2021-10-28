using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SH_Skill_Fly : SH_Skill
{
    public override bool Active()
    {
        if (base.Active())
        {
            owner.transform.DOKill();
            owner.transform.DOMoveY(3f, 2f);
            owner.overPower = true;
        }
        return true;
    }

    protected override IEnumerator SpecialEffect()
    {
        owner.TurnGageChange(1f);
        yield return new WaitForSecondsRealtime(3f);
        owner.transform.DOMoveY(-3f, 2f);
        owner.overPower = false;
    }
}

