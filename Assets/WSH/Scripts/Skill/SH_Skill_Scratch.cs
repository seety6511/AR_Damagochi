using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_Scratch : SH_Skill
{

    public override void Active()
    {
        base.Active();
        owner.AnimationChange("Scratch");
    }
}
