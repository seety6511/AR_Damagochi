using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Sleep : SH_Skill
{
    public override void Active()
    {
        base.Active();
        owner.hp += owner.atk * damage;
        if (owner.hp >= owner.maxHp)
            owner.hp = owner.maxHp;
    }
}
