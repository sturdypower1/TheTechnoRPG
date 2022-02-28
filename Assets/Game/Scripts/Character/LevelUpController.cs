using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class LevelUpController : MonoBehaviour
{
    /// <summary>
    /// the current max level for the character
    /// </summary>
    public int LevelCAP;
    /// <summary>
    /// the character's current level
    /// </summary>
    public int currentLVL;
    /// <summary>
    /// how much the character has experiance
    /// </summary>
    public int currentEXP;
    /// <summary>
    /// how much experiance is needed for the next level
    /// </summary>
    public int requiredEXP;
    /// <summary>
    /// what the character gets when they reach a new level
    /// </summary>
    public LevelUpRewardData LevelUpRewardData;
    /// <summary>
    /// </summary>
    /// <param name="experince"></param>
    /// <returns>all the level ups that occur</returns>
    public List<PlayerLevelUpData> AddExperience(int experince)
    {
        currentEXP += experince;
        var playerLevelUpList = new List<PlayerLevelUpData>();
        var canLevelUp = currentEXP >= ((currentLVL * 20) + ((currentLVL - 1) * 10));
        while (canLevelUp)
        {
            var playerLevelUpData = LevelUp();
            playerLevelUpList.Add(playerLevelUpData);
            canLevelUp = currentEXP >= ((currentLVL * 20) + ((currentLVL - 1) * 10));
        }
        return playerLevelUpList;
    }
    
    private PlayerLevelUpData LevelUp()
    {
        currentEXP -= ((currentLVL * 20) + ((currentLVL - 1) * 10));
        currentLVL += 1;
        requiredEXP = ((currentLVL * 20) + ((currentLVL - 1) * 10));

        var levelReward = LevelUpRewardData.levelRewards[currentLVL - LevelUpRewardData.startingLevel - 1];
        var characterStats = GetComponent<CharacterStats>();

        characterStats.stats.attack += levelReward.attackBonus;
        characterStats.stats.defence += levelReward.defenceBonus;
        characterStats.stats.maxPoints += levelReward.pointsBonus;
        characterStats.stats.maxHealth += levelReward.healthBonus;

        var earnedNewSkill = levelReward.skill != null;
        if (earnedNewSkill)
        {
            characterStats.skills.Insert(1, levelReward.skill);
        }

        var playerLevelUpData = new PlayerLevelUpData();
        playerLevelUpData.levelReward = levelReward;
        playerLevelUpData.level = currentLVL;
        playerLevelUpData.playerName = characterStats.stats.characterName;

        return playerLevelUpData;
    }
}
