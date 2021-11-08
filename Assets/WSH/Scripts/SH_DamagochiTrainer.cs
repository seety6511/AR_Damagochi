using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_DamagochiTrainer : MonoBehaviour
{
    public SH_ActionDamagochi damagochi;
    public List<SH_ActionDamagochi> damagoList;
    public SH_DamagochiStat statUI;

    public int gold;
    public int dia;

    private void Start()
    {
        damagochi = damagoList[0];
        statUI = FindObjectOfType<SH_DamagochiStat>();
        statUI.StatUpdate(damagochi);
    }

    public void DamagochiChange(SH_ActionDamagochi dama)
    {
        damagochi.gameObject.SetActive(false);
        damagochi = dama;
        damagochi.gameObject.SetActive(true);

        statUI.StatUpdate(damagochi);
    }
}
