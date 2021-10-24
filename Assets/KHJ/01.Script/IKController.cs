using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    public Transform lookedTarget;
    public void OnAnimatorIK(int layerIndex)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetLookAtPosition(lookedTarget.position);
        anim.SetLookAtWeight(1);
    }
    private void Update()
    {
        OnAnimatorIK(1);
    }
}
