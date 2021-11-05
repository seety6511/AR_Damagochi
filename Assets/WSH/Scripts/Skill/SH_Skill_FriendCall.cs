using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Skill_FriendCall : SH_Skill
{
    public GameObject flyDove;
    public int hitStack;

    private void Update()
    {
        if (owner.currentActiveSkill != null && owner.currentActiveSkill != this)
            hitStack = 0;
    }

    protected override IEnumerator SpecialEffect()
    {
        hitStack++;
        for(int i = 0; i < hitStack; ++i)
        {
            if (owner.attackTarget == null)
                break;

            var friend = Instantiate(flyDove).GetComponent<SH_FlyDove>();
            var pos = owner.gameObject.transform.position;
            pos.y += Random.Range(0.1f, 1f);
            pos.x += Random.Range(-1f, 1f);
            pos.z += Random.Range(-1f, 1f);
            friend.transform.position = pos;
            friend.Set(owner.attackTarget.GetRandomHitPoint().transform.position);
            owner.attackTarget.Damaged(owner.atk * damage);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
