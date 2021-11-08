using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Panel_DamagochiSelectList : MonoBehaviour
{
    public SH_Button_BattleDamagochiSelect bdsPrefab;

    public Transform buttonParent;
    public void OpenList()
    {
        var trainer = FindObjectOfType<SH_DamagochiTrainer>();
        var damagochis = trainer.damagoList;

        var trashs = buttonParent.GetComponentsInChildren<SH_Button_BattleDamagochiSelect>();

        List<SH_Button_BattleDamagochiSelect> buttons = new List<SH_Button_BattleDamagochiSelect>();

        foreach(var b in trashs)
        {
            buttons.Add(b);
        }

        if (damagochis.Count > buttons.Count)
        {
            for(int i = 0; i < damagochis.Count - trashs.Length; ++i)
            {
                var button = Instantiate(bdsPrefab, buttonParent);
                buttons.Add(button);
            }
        }

        for(int i = 0; i < damagochis.Count; ++i)
        {
            buttons[i].SetDamagochi(damagochis[i]);
            buttons[i].select.onClick.AddListener(delegate { gameObject.SetActive(false); });
        }
    }
}
