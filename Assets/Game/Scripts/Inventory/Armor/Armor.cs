using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Armor :  IObtainable
{
    public string ArmorType;
    public string description;
    public int defence;
    public override void Obtain()
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
