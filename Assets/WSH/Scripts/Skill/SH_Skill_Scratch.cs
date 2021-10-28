using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Scratch : SH_Skill
{
    public GameObject bleedingEffect;
    public float bleedDamage;
    public override bool Active()
    {
        if (base.Active())
        {
            owner.attackTarget.Damaged(owner.atk * damage);
        }
        return true;
    }

    protected override IEnumerator SpecialEffect()
    {
        bleedingEffect.SetActive(true);
        bleedingEffect.transform.position = owner.attackTarget.GetRandomHitPoint().transform.position;
        for(int i = 0; i < 3; ++i)
        {
            owner.attackTarget.Damaged(owner.atk * bleedDamage);
            yield return new WaitForSecondsRealtime(1f);
        }

        bleedingEffect.SetActive(false);
    }
}
