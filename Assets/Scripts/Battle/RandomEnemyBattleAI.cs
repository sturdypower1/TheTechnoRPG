using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Battler))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(EnemyRewardData))]
public class RandomEnemyBattleAI : MonoBehaviour
{
    Battler battler;
    CharacterStats characterStats;
    void Start()
    {
        battler = GetComponent<Battler>();
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        int numPlayer = BattleManager.instance.Players.Count;

        int Target = Random.Range((int)0, numPlayer);
        if(BattleManager.instance.isInBattle && !BattleManager.instance.IsWaitingForSkill)
        {
            if (!battler.isDown && battler.useTime >= battler.maxUseTime)
            {
                float randomValue = Random.Range(0, 1);
                foreach (Skill skill in characterStats.skills)
                {
                    if (skill.chance >= randomValue)
                    {
                        skill.UseSkill(BattleManager.instance.Players[Target], this.gameObject);
                        battler.useTime = 0;
                        float temp = Random.Range(1, 2);
                        battler.maxUseTime = skill.useTime * temp;
                    }
                }
            }
            else
            {
                battler.useTime += Time.unscaledDeltaTime;
            }
        }
        
    }
}
