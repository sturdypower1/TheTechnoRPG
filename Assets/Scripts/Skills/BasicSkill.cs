using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// deal regular damage
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Skill/Basic Skill")]
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
    public override void UseSkill(GameObject target, GameObject user)
    {

    }

}
