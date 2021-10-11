using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// the base clase for items
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/item")]
public class Item : IObtainable
{
    public string description;
    public override void Obtain()
    {
        InventoryManager.instance.AddItem(this);

    }
    /// <summary>
    /// update the description of the item
    /// </summary>
    /// <param name="label">the label with the description</param>
    public void UpdateDescription(Label label)
    {
        label.text = description;
    }
    public virtual void UseItem(GameObject target) { }
    public virtual bool GetUseability() { return true; }

}


