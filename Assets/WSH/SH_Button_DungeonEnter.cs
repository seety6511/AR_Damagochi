using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Button_DungeonEnter : MonoBehaviour
{
    [SerializeField]
    Image dungeonPortrait;
    [SerializeField]
    Text dungeonName;
    [SerializeField]
    Button bt;

    private void Start()
    {
        dungeonPortrait = GetComponentInChildren<Image>();
        dungeonName = GetComponentInChildren<Text>();
        bt = GetComponentInChildren<Button>();
    }

    public void SetDungeon(SH_Dungeon info)
    {
        dungeonName.text = info.name;
        bt.onClick.RemoveAllListeners();
        bt.onClick.AddListener(delegate { info.Enter(); });
    }
}
