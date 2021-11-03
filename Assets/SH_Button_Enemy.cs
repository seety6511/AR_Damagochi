using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Button_Enemy : MonoBehaviour
{
    public Button battleStart;
    public Image enemyPortrait;
    public SH_ActionDamagochi target;
    public void SetEnemy(SH_ActionDamagochi ad)
    {
        target = Instantiate(ad, transform);
        enemyPortrait.sprite = ad.portrait;
        battleStart.onClick.RemoveAllListeners();
        battleStart.onClick.AddListener(StartBattle);
        target.gameObject.SetActive(false);
    }

    void StartBattle()
    {
        FindObjectOfType<SH_BattleManager2>().BattleStart(target);
    }
}
