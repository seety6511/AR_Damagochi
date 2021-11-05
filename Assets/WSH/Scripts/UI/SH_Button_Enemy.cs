using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Button_Enemy : MonoBehaviour
{
    public Image bg;
    public Button battleStart;
    public Image enemyPortrait;
    public SH_ActionDamagochi enemy;
    public void SetEnemy(SH_ActionDamagochi ad)
    {
        enemy =ad;
        enemyPortrait.sprite = ad.portrait;
        battleStart.onClick.RemoveAllListeners();
        battleStart.onClick.AddListener(BattleStart);

        var player = FindObjectOfType<SH_DamagochiTrainer>().damagochi;
        var rgb = bg.color;
        rgb.g = 124;
        rgb.b = 124;
        var levelGap = player.level - ad.level;
        rgb.g = Mathf.Clamp(rgb.g + levelGap * 30, 0, 255);
        rgb.b = Mathf.Clamp(rgb.g + levelGap * 30, 0, 255);
        rgb.r = 255;
        bg.color = rgb;
        ad.gameObject.SetActive(false);
        //target.gameObject.SetActive(false);
    }

    void BattleStart()
    {
        if (FindObjectOfType<SH_DamagochiTrainer>().damagochi.hp == 0)
            return;
        FindObjectOfType<SH_BattleManager2>().BattleStart(enemy);
    }
}
