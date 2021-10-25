using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class SH_AnimeDamagochi : SH_PoolDamagochi
{
    public Sprite portrait;
    public bool canAnim;
    protected Animator animator;
    protected AnimatorControllerParameter[] animParam;
    public Action onEvent;

    public virtual void SpeedChange(float value)
    {
        animator.speed = value;
    }

    public virtual void Do(string key)
    {
        canAnim = false;
        print("Do_" + name + "_" + key);
        onEvent?.Invoke();
    }

    public virtual void End(string key)
    {
        canAnim = true;
        print("End_" + name + "_" + key);
    }

    protected override void Awake()
    {
        base.Awake();
        canAnim = true;
        animator = GetComponent<Animator>();
        animParam = animator.parameters;
    }

    protected override void Update()
    {
        base.Update();
    }
    public void AnimatorParamClear()
    {
        animator.speed = 1f;
        foreach (var p in animParam)
        {
            animator.SetBool(p.name, false);
        }
    }

    public void AnimationChange(string key, bool value = true)
    {
        if (!canAnim)
        {
            Debug.Log("Already Animation");
            return;
        }
        AnimatorParamClear();

        animator.SetBool(key, value);
    }
}
