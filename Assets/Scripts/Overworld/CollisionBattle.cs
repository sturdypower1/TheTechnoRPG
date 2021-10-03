using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBattle : MonoBehaviour
{
    public EnemyBattlers enemyBattlers;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BattleManager.instance.SetupBattle(enemyBattlers.Enemies, enemyBattlers.battleBackground);
        }
    }

}
