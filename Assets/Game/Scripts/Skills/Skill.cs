using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// special skills you can use in battle
/// </summary>
[System.Serializable]
public abstract class Skill : ScriptableObject
{
    public event EmptyEventHandler OnSkillFinished;
    /// <summary>
    /// the chance an attack is to happen from an enemy
    /// </summary>
    [Range(0, 1)]
    public float chance;

    [TextArea(1, 20)]
    public string description;
    /// <summary>
    /// how much it costs to use the skill
    /// </summary>
    public int cost;

    /// <summary>
    /// how long the player has to wait after using the skill
    /// </summary>
    public float useTime;
    /// <summary>
    /// use the skill
    /// </summary>
    public abstract void UseSkill(Battler target, Battler user);
}


