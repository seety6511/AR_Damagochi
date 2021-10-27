using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_NyangNyangPunch : SH_Skill
{
    public override void Active()
    {
        base.Active();
        StartCoroutine("SkillEffect");
    }

    IEnumerable SkillEffect()
    {
        owner.AnimSpeedChange(3f);
        for(int i = 0; i < 3; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            owner.AnimationChange(name);
            owner.attackTarget.currentTurnGage -= 20f;
        }
        owner.AnimSpeedChange(1f);
    }
}
