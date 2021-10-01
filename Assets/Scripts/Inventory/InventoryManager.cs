using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeReference]
    public List<Weapon> weapons = new List<Weapon>();
    [SerializeReference]
    public List<Armor> armors = new List<Armor>();
    
    [SerializeReference]
    public List<Item> items = new List<Item>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
    }
}
