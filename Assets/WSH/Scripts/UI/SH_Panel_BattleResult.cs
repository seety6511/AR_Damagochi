using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Panel_BattleResult : MonoBehaviour
{
    public Image winerSprite;
    public Image loserSprite;
    public Slider winerExpBar;
    public Text winerLevel;
    public Text loserLevel;
    public Text winerExp;
    public Text getExp;

    public Text winerCoin;
    public Text winerDia;

    public void On(SH_BattleManager bm)
    {
        var winer = bm.winer;
        var loser = bm.loser;
        winerSprite.sprite = winer.portrait;
        loserSprite.sprite = loser.portrait;
        winerExpBar.transform.localScale = new Vector3(winer.exp / winer.maxExp, 1f, 1f);
        winerLevel.text = winer.level.ToString();
        loserLevel.text = loser.level.ToString();
        winerExp.text = winer.exp + " / " + winer.maxExp;
        //winerCoin.text = "+" + bm.winCoin;
        //winerDia.text = "+" + bm.winDia;
        gameObject.SetActive(true);
    }

    public void On(SH_BattleManager2 bm)
    {
        var winer = bm.winer;
        var loser = bm.loser;

        winerSprite.sprite = winer.portrait;
        loserSprite.sprite = loser.portrait;
        loserLevel.text = loser.level.ToString();

        winerExp.text = winer.maxExp.ToString();
        getExp.text = winer.exp + " + " + loser.deadExp;
        winer.exp = loser.deadExp;
        winerExpBar.value = winer.exp / winer.maxExp;
        winerLevel.text = winer.level.ToString();

        winerCoin.text = "+" + loser.level * 10;
        winerDia.text = "+" + loser.level * 1;

        gameObject.SetActive(true);
    }
}
