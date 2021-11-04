using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SH_Skill_Fly : SH_Skill
{
    protected override IEnumerator SpecialEffect()
    {
        float temp = owner.transform.position.y;
        owner.transform.DOKill();
        owner.transform.DOMoveY(12f, 2f);
        owner.overPower = true;

        yield return new WaitForSecondsRealtime(3f);
        owner.transform.DOMoveY(temp, 2f);
        owner.overPower = false;
    }
}

