using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BattleMap : MonoBehaviour
{
    public GameObject playerPlat;
    public GameObject enemyPlat;

    public void On(SH_ActionDamagochi player, SH_ActionDamagochi enemy)
    {
        gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);

        player.gameObject.transform.position = playerPlat.transform.position;
        enemy.gameObject.transform.position = enemyPlat.transform.position;
        player.gameObject.transform.LookAt(enemy.transform);
        enemy.gameObject.transform.LookAt(player.transform);
        
    }
}
