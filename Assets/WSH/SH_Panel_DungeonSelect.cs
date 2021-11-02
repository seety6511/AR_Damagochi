using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Panel_DungeonSelect : MonoBehaviour
{
    SH_Button_DungeonEnter[] enterButtons;

    private void Start()
    {
        enterButtons = GetComponentsInChildren<SH_Button_DungeonEnter>();
    }

    public void SetDungeonInfos(SH_DungeonManager dm)
    {
        for(int i = 0; i < enterButtons.Length; ++i)
        {
            enterButtons[i].SetDungeon(dm.dungeons[i]);
        }
    }
}
