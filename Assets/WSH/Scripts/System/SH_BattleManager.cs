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

    public SH_BattleLogger battleLog;
    public AudioSource bgmPlayer;

    public AudioClip[] battleBGM;

    public bool playBattle;

    private void Start()
    {
        if(!TryGetComponent(out bgmPlayer))
            bgmPlayer = gameObject.AddComponent<AudioSource>();
    }

    void PlayBattleBGM()
    {
        var i = Random.Range(0, battleBGM.Length);
        var bgm = battleBGM[i];
        bgmPlayer.clip = bgm;
        bgmPlayer.loop = true;
        bgmPlayer.volume = 1f;
        bgmPlayer.Play();
    }

    public void StartBattle(SH_ActionDamagochi c, SH_ActionDamagochi t)
    {
        if(c == t)
            return;

        battleLog.LogText("Battle Start!", Color.black);
        PlayBattleBGM();
        challenger = c;
        target = t;

        target.ActionStateChange(SH_ActionDamagochi.ActionState.Battle);
        target.battleState = SH_ActionDamagochi.BattleState.Ambushed;
        target.attackTarget = challenger;

        challenger.ActionStateChange(SH_ActionDamagochi.ActionState.Battle);
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

        if(challenger.actionState == SH_ActionDamagochi.ActionState.Dead || target.actionState == SH_ActionDamagochi.ActionState.Dead)
        {
            winer = challenger.actionState == SH_ActionDamagochi.ActionState.Dead ? target : challenger;
            loser = winer == challenger ? target : challenger;
            BattleEnd();
        }
    }
    public SH_ActionDamagochi winer;
    public SH_ActionDamagochi loser;
    void BattleEnd()
    {
        winer.exp = loser.deadExp;
        Debug.Log(winer.name + " Get " + loser.deadExp +" Exp");
        winer.battleState = SH_ActionDamagochi.BattleState.End;
        loser.battleState = SH_ActionDamagochi.BattleState.End;
        playBattle = false;
        battleResultPanel.On(this);
        StartCoroutine("BGMDown");
    }

    IEnumerator BGMDown()
    {
        float timer = 0f;

        while (timer < 3f)
        {
            timer += Time.deltaTime;
            bgmPlayer.volume *= 0.9f;
            yield return null;
        }
        bgmPlayer.volume = 0f;
    }
}
