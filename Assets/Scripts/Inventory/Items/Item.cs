using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// the base clase for items
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/item")]
public class Item : ScriptableObject, IObtainable
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
    public void UseItem(GameObject target) { }
    public bool GetUseability() { return true; }

}


