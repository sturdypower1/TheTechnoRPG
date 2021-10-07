using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Armor : ScriptableObject, IObtainable
{
    public string ArmorType;
    public new string name;
    public string description;
    public int defence;
    public void Obtain()
    {

    }
    public abstract Damage CalculateDamage(Damage damage);
}
public class SpecialArmor : Armor
{
    public override Damage CalculateDamage(Damage damage)
    {
        return new Damage();
    }
}
