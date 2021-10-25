using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BattleManager : MonoBehaviour
{
    public GameObject ambushedEffect;
    public SH_Panel_Battle battlePanel;
    public SH_Panel_BattleResult battleResultPanel;

    public SH_ActionDamagochi challenger;
    public SH_ActionDamagochi target;

    public SH_TextLogControl battleLog;

    public bool playBattle;
    
    public void StartBattle(SH_ActionDamagochi c, SH_ActionDamagochi t)
    {
        if(c == t)
            return;

        battleLog.LogText("Battle Start!", Color.black);

        challenger = c;
        target = t;

        target.ActionStateChange(SH_ActionDamagochi.ActionState.isBattle);
        target.battleState = SH_ActionDamagochi.BattleState.Ambushed;
        target.attackTarget = challenger;

        challenger.ActionStateChange(SH_ActionDamagochi.ActionState.isBattle);
        challenger.battleState = SH_ActionDamagochi.BattleState.Surprise;
        challenger.attackTarget = target;

        var pos = target.transform.position;
        pos.y += 1f;
        ambushedEffect.transform.position = pos;
        ambushedEffect.SetActive(true);

        battlePanel.gameObject.SetActive(true);
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

        battlePanel.SkillButtonUpdate();
        challenger.BattleStateAction();
        target.BattleStateAction();

        if(challenger.actionState == SH_ActionDamagochi.ActionState.isDead || target.actionState == SH_ActionDamagochi.ActionState.isDead)
        {
            winer = challenger.actionState == SH_ActionDamagochi.ActionState.isDead ? target : challenger;
            loser = winer == challenger ? target : challenger;
            BattleEnd();
        }
    }
    SH_ActionDamagochi winer;
    SH_ActionDamagochi loser;
    void BattleEnd()
    {
        winer.exp = loser.deadExp;
        Debug.Log(winer.name + " Get " + loser.deadExp +" Exp");
        winer.battleState = SH_ActionDamagochi.BattleState.End;
        loser.battleState = SH_ActionDamagochi.BattleState.End;
        playBattle = false;
    }

}
