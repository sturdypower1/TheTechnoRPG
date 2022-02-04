using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "LevelUpData/basic")]
public class LevelUpRewardData : ScriptableObject
{
    public int startingLevel = 1;
    public List<LevelReward> levelRewards;
}
[System.Serializable]
public struct LevelReward
{
    public int healthBonus;
    public int attackBonus;
    public int defenceBonus;
    public int pointsBonus;
    public Skill skill;
}
