using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamagochiAnim
{
    Idle,
    Walk,
    Run,
    Dead,
    Stand,
    Attack,
    Slap,
    Shouting,
    Bite,
    Sleep,
    NyangNyangPunch,
    SpeedAttack,
    Growling,
    Scratch,
    Sit,
    Look,
    Fly,
    Grooming,
    Peck,
    FriendCall,
    Eat,
    Wait,
}

[RequireComponent(typeof(Animator))]
public class SH_AnimeDamagochi : SH_SoundDamagochi
{
    public Sprite portrait;
    public bool canAnim;
    protected Animator animator;
    protected AnimatorControllerParameter[] animParam;
    public Action doEvent;
    public Action endEvent;

    public void AnimSpeedChange(float value)
    {
        animator.speed = value;
    }

    public virtual void SpeedChange(float value)
    {
        animator.speed = value;
    }

    public virtual void Do(string key)
    {
        canAnim = false;
        print("Do_" + name + "_" + key);
        doEvent?.Invoke();
        doEvent = null;
    }

    public virtual void End(string key)
    {
        canAnim = true;
        print("End_" + name + "_" + key);
        endEvent?.Invoke();
        endEvent = null;
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
        foreach (var p in animParam)
        {
            animator.SetBool(p.name, false);
        }
    }

    public void AnimationChange(string key, bool value = true)
    {
        AnimatorParamClear();
        foreach(var p in animParam)
        {
            if (p.name == key)
            {
                animator.SetBool(key, value);
                return;
            }
        }
    }
    public void AnimationChange(DamagochiAnim key, bool value = true)
    {
        AnimatorParamClear();

        animator.SetBool(key.ToString(), value);
    }

    //public void AnimationChange(string key)
    //{
    //    AnimatorParamClear();
    //    animator.SetTrigger(key);
    //}
}
