using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_DamagochiTrainer : MonoBehaviour
{
    public SH_ActionDamagochi damagochi;
    public List<SH_ActionDamagochi> damagoList;
    public SH_DamagochiStat statUI;

    private void Start()
    {
        damagochi = damagoList[0];
        statUI = FindObjectOfType<SH_DamagochiStat>();
        statUI.StatUpdate(damagochi);
    }
}
