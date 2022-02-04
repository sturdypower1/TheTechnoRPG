using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
