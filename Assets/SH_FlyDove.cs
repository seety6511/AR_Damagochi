using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SH_FlyDove : MonoBehaviour
{
    Animator animator;
    public Vector3 targetPos;
    public float flySpeed;

    bool go;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Fly", true);
    }

    public void Set(Vector3 pos)
    {
        targetPos = pos;
        go = true;
        transform.LookAt(targetPos);
        transform.DOMove(targetPos,flySpeed);
    }
}
