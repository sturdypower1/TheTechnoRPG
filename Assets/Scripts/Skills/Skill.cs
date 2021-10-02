using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// special skills you can use in battle
/// </summary>
[System.Serializable]
public abstract class Skill : ScriptableObject
{
    public string skillType;
    public new string name;
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
    public abstract void UseSkill(GameObject target, GameObject user);
}


