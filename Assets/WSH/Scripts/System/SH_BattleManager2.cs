using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SH_BattleManager2 : MonoBehaviour
{
    public SH_BattleMap battleMap;
    public SH_DamagochiTrainer trainer;

    public SH_ActionDamagochi playerDama;
    public SH_ActionDamagochi enemyDama;

    public GameObject nonBattleUI;
    public GameObject battleUI;

    public SH_UI_DamagochiBattle playerUI;
    public SH_UI_DamagochiBattle enemyUI;

    public SH_UI_PlayerSkill ups;

    public SH_SoundManager sm;
    public SH_Panel_BattleResult battleResult;

    public bool battle;
    private void Start()
    {
        sm = FindObjectOfType<SH_SoundManager>();
        battleMap = FindObjectOfType<SH_BattleMap>();
        battleMap.gameObject.SetActive(false);
        trainer = FindObjectOfType<SH_DamagochiTrainer>();
        battle = false;
    }

    private void Update()
    {
        if (!battle)
            return;

        foreach (var b in ups.skillButtons)
        {
            b.CoolTimeUpdate();
        }
    }
    public void BattleStart(SH_ActionDamagochi ad)
    {
        battleUI.SetActive(true);
        enemyDama = ad;

        playerDama = trainer.damagochi;
        playerDama.gameObject.SetActive(true);
        playerDama.attackTarget = enemyDama;
        playerDama.ActionStateChange(SH_ActionDamagochi.ActionState.Battle);
        playerDama.battleState = SH_ActionDamagochi.BattleState.Ambushed;
        playerUI.SetOwner(playerDama);
        playerDama.playerble = true;
        
        enemyDama.gameObject.SetActive(true);
        enemyDama.attackTarget = playerDama;
        enemyDama.ActionStateChange(SH_ActionDamagochi.ActionState.Battle);
        enemyDama.battleState = SH_ActionDamagochi.BattleState.Ambushed;
        enemyUI.SetOwner(enemyDama);

        sm.PlayBattleBGM();
        battleMap.On(playerDama, enemyDama);

        ups.On(trainer);
        battle = true;
        nonBattleUI.transform.DOLocalMoveY(1920f, 3f);
    }

    public SH_ActionDamagochi winer;
    public SH_ActionDamagochi loser;
    public void BattleEnd()
    {
        winer = playerDama.actionState == SH_ActionDamagochi.ActionState.Dead ? enemyDama : playerDama;
        loser = winer == playerDama ? enemyDama : playerDama;
        StartCoroutine(BattleResult());
    }

    IEnumerator BattleResult()
    {
        trainer.statUI.StatUpdate(trainer.damagochi);

        if (loser != trainer.damagochi)
            battleResult.On(this);

        yield return new WaitForSeconds(5f);
        trainer.damagochi.ActionStateChange(SH_ActionDamagochi.ActionState.Idle);
        nonBattleUI.transform.DOLocalMoveY(0f, 3f);
        battleUI.SetActive(false);
        sm.StopBGM();
        battleResult.gameObject.SetActive(false);
        FindObjectOfType<SH_GPSEnemySpawner>().ResetEnemy();
    }
}
