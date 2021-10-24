using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_AnimeDamagochi : SH_PoolDamagochi
{
    protected Animator animator;
    protected AnimatorControllerParameter[] animParam;

    public virtual void Do(string key)
    {
        print("Do_"+name +"_"+ key);
    }

    public virtual void End(string key)
    {
        print("End_"+name +"_"+ key);
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        animParam = animator.parameters;
    }

    protected override void Update()
    {
        base.Update();
    }
    public void AnimatorParamClear()
    {
        foreach (var p in animParam)
        {
            animator.SetBool(p.name, false);
        }
    }

    public void AnimationChange(string key, bool value = true)
    {
        AnimatorParamClear();
        animator.SetBool(key, value);
    }

    //public void AnimationChange(string key)
    //{
    //    AnimatorParamClear();
    //    animator.SetTrigger(key);
    //}
}
