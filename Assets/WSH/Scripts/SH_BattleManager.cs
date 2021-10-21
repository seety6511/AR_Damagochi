using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BattleManager : MonoBehaviour
{
    public GameObject ambushedEffect;

    public SH_ActionDamagochi challenger;
    public SH_ActionDamagochi target;
    public bool playBattle;
    public void BattleStart(SH_ActionDamagochi challenger, SH_ActionDamagochi target)
    {
        this.challenger = challenger;
        this.target = target;
        challenger.battleOn = true;
        target.battleOn = true;
        var pos = target.transform.position;
        pos.y += 1f;
        ambushedEffect.transform.position = pos;
        ambushedEffect.SetActive(true);

        challenger.battleState = SH_ActionDamagochi.BattleState.Surprise;
        target.battleState = SH_ActionDamagochi.BattleState.Ambushed;
        playBattle = true;
    }

    protected void Update()
    {
        BattleUpdate();
    }

    public void BattleUpdate()
    {
        if (!playBattle)
            return;

        challenger.BattleStateAction();
        target.BattleStateAction();
    }

}
