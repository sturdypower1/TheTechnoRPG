using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// the base clase for items
/// </summary>
[System.Serializable]
public abstract class Item : IObtainable
{
    public string ItemType;
    public string name;
    public string description;
    public void Obtain()
    {

    }
    /// <summary>
    /// update the description of the item
    /// </summary>
    /// <param name="label">the label with the description</param>
    public void UpdateDescription(Label label)
    {
        label.text = description;
    }
    public abstract void UseItem();

}
/// <summary>
/// will be used to heal the player
/// </summary>
[System.Serializable]
public class HealingItem : Item
{
    public int HealingAmount;
    public override void UseItem()
    {
        
    }
}

