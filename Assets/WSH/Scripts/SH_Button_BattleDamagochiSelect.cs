using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Button_BattleDamagochiSelect : MonoBehaviour
{
    public Image background;
    public Button select;
    public Image portrait;
    public Text level;
    public new Text name;
    public Slider hp;
    public Slider exp;

    public Sprite enableSprite;
    public Sprite disableSprite;

    public SH_ActionDamagochi owner;

    public void SetDamagochi(SH_ActionDamagochi dama)
    {
        owner = dama;
        portrait.sprite = owner.portrait;
        name.text = owner.name;
        level.text = "LV."+owner.level;
        hp.value = owner.hp / owner.maxHp;
        exp.value = owner.exp / owner.maxExp;

        if (dama.unlock)
            background.sprite = enableSprite;
        else
            background.sprite = disableSprite;

        select.onClick.RemoveAllListeners();
        select.onClick.AddListener(DamagochiChange);
    }

    void DamagochiChange()
    {
        if (!owner.unlock)
            return;

        FindObjectOfType<SH_DamagochiTrainer>().DamagochiChange(owner);
    }
}
