using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxMngr : MonoBehaviour
{
    public List<LootBox> lootBoxesOrigin;
    public List<LootBox> lootBoxes;

    public LootBox ticket;
    public List<LootBox> G10;
    public List<LootBox> G5;
    public List<LootBox> G;
    void Start()
    {
        //Reset();
    }

    public void Reset()
    {
        lootBoxes.Clear();
        for (int i = 0; i < lootBoxesOrigin.Count; i++)
        {
            lootBoxesOrigin[i].text.text = (i + 1).ToString();
            lootBoxesOrigin[i].Reset();
            lootBoxes.Add(lootBoxesOrigin[i]);
        }

        int a = Random.Range(0, lootBoxes.Count);
        ticket = lootBoxes[a];
        ticket.prize = Prize.Ticket;
        lootBoxes.RemoveAt(a);


        G10 = RandomSet(3);
        foreach(LootBox loot in G10)
        {
            loot.prize = Prize.G10;
        }
        G5 = RandomSet(6);
        foreach (LootBox loot in G5)
        {
            loot.prize = Prize.G5;
        }
        G = RandomSet(10);
        foreach (LootBox loot in G)
        {
            loot.prize = Prize.G;
        }
    }

    List<LootBox> RandomSet(int num)
    {
        List<LootBox> result = new List<LootBox>();
        for (int i = 0; i < num; i++)
        {
            int a = Random.Range(0, lootBoxes.Count);
            result.Add(lootBoxes[a]);
            lootBoxes.RemoveAt(a);
        }
        return result;
    }
}
