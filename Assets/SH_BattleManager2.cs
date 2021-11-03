using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BattleManager2 : MonoBehaviour
{
    public SH_BattleMap battleMap;
    public SH_DamagochiTrainer trainer;

    public SH_ActionDamagochi playerDama;
    public SH_ActionDamagochi enemyDama;

    public List<GameObject> nonBattleUI;
    public List<GameObject> battleUI;

    private void Start()
    {
        battleMap = FindObjectOfType<SH_BattleMap>();
        battleMap.gameObject.SetActive(false);
        trainer = FindObjectOfType<SH_DamagochiTrainer>();
    }
    public void BattleStart(SH_ActionDamagochi ad)
    {
        foreach (var i in nonBattleUI)
            i.SetActive(false);

        foreach (var i in battleUI)
            i.SetActive(true);

        playerDama = trainer.damagochi;
        enemyDama = ad;
        battleMap.On(playerDama, enemyDama);
    }
}
