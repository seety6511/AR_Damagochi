using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SH_FlyDove : MonoBehaviour
{
    Animator animator;
    public Vector3 targetPos;
    public float flySpeed;
    void Do(string key) { }
    void End(string key) { }
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Fly", true);
    }
    public void Set(Vector3 pos)
    {
        targetPos = pos;
        transform.LookAt(targetPos);
        transform.DOMove(targetPos,flySpeed);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
