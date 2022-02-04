/*#if UNITY_EDITOR
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(InventoryManager))]
public class InventoryManagerInspector : Editor
{
    private Type[] _weaponInplementations;
    private int _weaponInplementationsTypeIndex;

    private Type[] _armorInplementations;
    private int _armorInplementationsTypeIndex;

    private Type[] _itemInplementations;

    InventoryManager _inventoryManager;

    private VisualElement _RootElement;
    private VisualTreeAsset _VisualTree;

    SerializedProperty _weaponListProperty;
    SerializedProperty _armorListProperty;
    SerializedProperty _itemListProperty;

    private void OnEnable()
    {
        _inventoryManager = target as InventoryManager;

        _RootElement = new VisualElement();

        _VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/EditorUIElements/UXML/InventoryTemplate.uxml");
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/EditorUIElements/USS/EditorStyleSheet.uss");


    }
    public void AddWeapon()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("weapon_choices");
        int index = dropdownField.index;
        var weapon = (Weapon)Activator.CreateInstance(_weaponInplementations[index]);
        weapon.WeaponType = _weaponInplementations[index].FullName;
        _inventoryManager.weapons.Add(weapon);
    }
    public void AddArmor()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("armor_choices");
        int index = dropdownField.index;
        var armor = (Armor)Activator.CreateInstance(_armorInplementations[index]);
        armor.ArmorType = _armorInplementations[index].FullName;
        _inventoryManager.armors.Add(armor);
    }
    public void AddItem()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("item_choices");
        int index = dropdownField.index;
        var item = (Item)Activator.CreateInstance(_itemInplementations[index]);
        item.ItemType = _itemInplementations[index].FullName;
        _inventoryManager.items.Add(item);
    }
    public override VisualElement CreateInspectorGUI()
    {
        _weaponListProperty = serializedObject.FindProperty("weapons");
        _armorListProperty = serializedObject.FindProperty("armors");
        _itemListProperty = serializedObject.FindProperty("items");

        _RootElement.Clear();
        
        _VisualTree.CloneTree(_RootElement);

        DropdownField weaponDropDownField = _RootElement.Q<DropdownField>("weapon_choices");
        weaponDropDownField.choices.Clear();

        PropertyField weaponsPropertyField = _RootElement.Q<PropertyField>("weapons_list");
        weaponsPropertyField.BindProperty(_weaponListProperty);

        Button weaponAddButton = _RootElement.Q<Button>("add_weapon_button");
        weaponAddButton.clicked += AddWeapon;

        DropdownField armorDropDownField = _RootElement.Q<DropdownField>("armor_choices");
        armorDropDownField.choices.Clear();

        PropertyField armorsPropertyField = _RootElement.Q<PropertyField>("armors_list");
        armorsPropertyField.BindProperty(_armorListProperty);

        Button armorAddButton = _RootElement.Q<Button>("add_armor_button");
        armorAddButton.clicked += AddArmor;

        DropdownField itemDropDownField = _RootElement.Q<DropdownField>("item_choices");
        itemDropDownField.choices.Clear();

        PropertyField itemsPropertyField = _RootElement.Q<PropertyField>("items_list");
        itemsPropertyField.BindProperty(_itemListProperty);

        Button itemAddButton = _RootElement.Q<Button>("add_item_button");
        itemAddButton.clicked += AddItem;

        if (_weaponInplementations == null)
        {
            _weaponInplementations = GetImplementations<Weapon>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        foreach(Type type in _weaponInplementations)
        {
            weaponDropDownField.choices.Add(type.FullName);
            //weaponDropDownField.RegisterCallback<ChangeEvent<string>>(e => UpdateType(weaponDropDownField));
        }

        if(_armorInplementations == null)
        {
            _armorInplementations = GetImplementations<Armor>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        foreach(Type type in _armorInplementations)
        {
            armorDropDownField.choices.Add(type.FullName);
            //armorDropDownField.RegisterCallback<ChangeEvent<string>>(e => UpdateType(armorDropDownField));
        }
        if(_itemInplementations == null)
        {
            _itemInplementations = GetImplementations<Item>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        foreach(Type type in _itemInplementations)
        {
            itemDropDownField.choices.Add(type.FullName);
        }

        itemDropDownField.index = 0;
        weaponDropDownField.index = 0;
        armorDropDownField.index = 0;
        return _RootElement;
        
    }

    private static Type[] GetImplementations<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());


        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}
#endif*/
