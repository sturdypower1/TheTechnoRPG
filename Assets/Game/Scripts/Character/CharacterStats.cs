using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// the class that represents a characters stats
/// </summary>
//[RequireComponent(typeof(Battler))]
[System.Serializable]
public class CharacterStats : MonoBehaviour
{
    public Stats stats;
    /// <summary>
    /// the currently equiped weapon
    /// </summary>
    public Weapon equipedWeapon;
    /// <summary>
    /// the currently equiped armor
    /// </summary>
    public Armor equipedArmor;
    /// <summary>
    /// all the skills the character has earned
    /// </summary>
    public List<Skill> skills;

    
}
[System.Serializable]
public struct Damage{
    public int damageAmount;
    public DamageType damageType;
}
[System.Serializable]
public enum DamageType
{
    Physical,
    Bleeding,
    Butchering
}


[System.Serializable]
public struct Stats
{
    /// <summary>
    /// the name of the character
    /// </summary>
    public string characterName;
    /// <summary>
    /// used in the calculation for attacks
    /// </summary>
    public int attack;
    /// <summary>
    /// used in the calculation for taking damage(specifically in the armor
    /// </summary>
    public int defence;
    /// <summary>
    /// the max health for a character
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// the current health
    /// </summary>
    public int health;
    /// <summary>
    /// the max points the character can have
    /// </summary>
    public int maxPoints;
    /// <summary>
    /// the current points the character has
    /// </summary>
    public int points;
}
