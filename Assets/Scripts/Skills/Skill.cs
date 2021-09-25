using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// special skills you can use in battle
/// </summary>
[System.Serializable]
public abstract class Skill
{
    public string skillType;
    public string name;
    public string description;

    /// <summary>
    /// how long the player has to wait after using the skill
    /// </summary>
    public float useTime;
    /// <summary>
    /// use the skill
    /// </summary>
    public abstract void UseSkill(GameObject target);
}
/// <summary>
/// deal regular damage
/// </summary>
[System.Serializable]
public class BasicSkill : Skill
{
    /// <summary>
    /// how much damage the attack does
    /// </summary>
    public int damage;
    /// <summary>
    /// attack the enemy 
    /// </summary>
    /// <param name="target"></param>
    public override void UseSkill(GameObject target)
    {
    }

}

