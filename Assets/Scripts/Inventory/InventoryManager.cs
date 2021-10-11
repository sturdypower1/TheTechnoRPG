using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armors = new List<Armor>();
    public List<Item> items = new List<Item>();
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        UIManager.instance.OnTitleReturn += DestroyInventory;
    }
    private void Update()
    {
    }

    public void Save(int saveFileNumber)
    {
        //save level data
        string savePath = Application.persistentDataPath + "/tempsave" + "/inventory";
        InventorySave inventorySave = new InventorySave();
        inventorySave.armorNames = new List<string>();
        inventorySave.weaponNames = new List<string>();
        inventorySave.itemNames = new List<string>();

        foreach (Weapon weapon in weapons)
        {
            inventorySave.weaponNames.Add(weapon.name);
        }
        foreach(Armor armor in armors)
        {
            inventorySave.armorNames.Add(armor.name);
        }
        foreach(Item item in items)
        {
            inventorySave.itemNames.Add(item.name);
        }

        string jsonString = JsonUtility.ToJson(inventorySave);
        File.WriteAllText(savePath, jsonString);
    }
    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/inventory";
        if (File.Exists(savePath))
        {
            string jsonString = File.ReadAllText(savePath);
            InventorySave saveData = JsonUtility.FromJson<InventorySave>(jsonString);

            weapons.Clear();
            armors.Clear();
            items.Clear();

            foreach(string weaponName in saveData.weaponNames)
            {
                weapons.Add( Resources.Load(weaponName) as Weapon);
            }
            foreach(string armorName in saveData.armorNames)
            {
                armors.Add(Resources.Load(armorName) as Armor);
            }
            foreach(string itemName in saveData.itemNames)
            {
                items.Add(Resources.Load(itemName) as Item);
            }
        }
    }
    
    public void AddItem(Item item)
    {
        items.Add(item);
        InkManager.instance.DisplayNewItem(item.name);
    }
    public void DestroyInventory()
    {
        Destroy(this.gameObject);
    }
}
[System.Serializable]
public struct InventorySave
{
    public List<string> weaponNames;
    public List<string> armorNames;
    public List<string> itemNames;
}
