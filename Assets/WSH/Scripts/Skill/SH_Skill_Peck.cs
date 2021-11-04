using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Peck : SH_Skill
{
    public int maxHit;
    protected override IEnumerator SpecialEffect()
    {
        owner.SpeedChange(3f);
        int totalCount = 0;
        for (int i = 0; i < 3; ++i)
        {
            hitEffect.SetActive(false);
            owner.attackTarget?.Damaged(owner.atk * damage);
            owner.AnimationChange(name);
            hitEffect.SetActive(true);
            totalCount++;
            if (Random.Range(0, 2) == 0)
                i--;
            if (totalCount == maxHit)
                break;
            yield return new WaitForSeconds(0.1f);

        }
        owner.SpeedChange(1f);
    }
}
