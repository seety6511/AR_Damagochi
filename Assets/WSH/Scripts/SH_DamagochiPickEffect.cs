using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_DamagochiPickEffect : MonoBehaviour
{
    public Damagochi target;
    public void FollowTarget(Damagochi target)
    {
        this.target = target;
        gameObject.SetActive(true);
    }

    public void Off()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
            transform.position = target.transform.position;
    }
}
