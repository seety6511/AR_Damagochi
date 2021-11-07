using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SH_Button_Home : MonoBehaviour
{
    public Button homeButton;

    private void Start()
    {
        homeButton.onClick.AddListener(GoHome);
    }

    public void GoHome()
    {
        var trainer = FindObjectOfType<SH_DamagochiTrainer>();
        var dataManager = KHJ_DataManager.instance;
        
        for(int i = 0; i < dataManager.info.Length; ++i)
        {
            var data = dataManager.info[i];
            data.level = trainer.damagoList[i].level;
            data.atk = trainer.damagoList[i].atk;
            data.hp = trainer.damagoList[i].hp;
            data.speed = trainer.damagoList[i].atkSpeed;
        }

        var info = dataManager.sceneInfo;
        info.dia = trainer.dia;
        info.gold = trainer.gold;

        dataManager.SaveBattleSceneData();
        SceneManager.LoadScene("KHJ_CatScene");
    }
}
