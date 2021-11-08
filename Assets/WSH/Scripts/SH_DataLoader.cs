using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_DataLoader : MonoBehaviour
{
    public KHJ_DataManager dm;

    public SH_DamagochiTrainer trainer;
    void Start()
    {
        StartCoroutine(GetDataManager());
    }

    IEnumerator GetDataManager()
    {
        while (dm == null)
        {
            dm = FindObjectOfType<KHJ_DataManager>();
            yield return null;
        }

        ReadData();
    }

    void ReadData()
    {
        trainer = FindObjectOfType<SH_DamagochiTrainer>();
        trainer.gold = dm.sceneInfo.gold;
        trainer.dia = dm.sceneInfo.dia;

        var petData = dm.info;
        for(int i = 0; i < petData.Length; ++i)
        {
            var d = petData[i];

            if (!d.isGet)
                continue;

            var level = d.level+1;
            var atk = d.atk;
            var hp = d.hp;
            var speed = d.speed;

            trainer.damagoList[i].unlock = true;
            trainer.damagoList[i].level = level;
            trainer.damagoList[i].maxHp = hp * level * 5;
            trainer.damagoList[i].hp = hp * level * 5;
            trainer.damagoList[i].atk = atk*level;
            trainer.damagoList[i].atkSpeed = speed*level;
            trainer.damagoList[i].maxTurnGage = level * 20;
        }
        trainer.damagochi = trainer.damagoList[0];

        FindObjectOfType<SH_DamagochiStat>().StatUpdate(trainer.damagochi);
    }
}
