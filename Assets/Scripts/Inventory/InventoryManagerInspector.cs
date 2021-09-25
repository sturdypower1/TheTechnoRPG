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

    InventoryManager _inventoryManager;

    private VisualElement _RootElement;
    private VisualTreeAsset _VisualTree;

    SerializedProperty _weaponListProperty;
    SerializedProperty _armorListProperty;

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
    public override VisualElement CreateInspectorGUI()
    {
        _weaponListProperty = serializedObject.FindProperty("weapons");
        _armorListProperty = serializedObject.FindProperty("armors");

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

        if (_weaponInplementations == null)
        {
            _weaponInplementations = GetImplementations<Weapon>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        foreach(Type type in _weaponInplementations)
        {
            weaponDropDownField.choices.Add(type.FullName);
            weaponDropDownField.RegisterCallback<ChangeEvent<string>>(e => UpdateType(weaponDropDownField));
        }

        if(_armorInplementations == null)
        {
            _armorInplementations = GetImplementations<Armor>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        foreach(Type type in _armorInplementations)
        {
            armorDropDownField.choices.Add(type.FullName);
            armorDropDownField.RegisterCallback<ChangeEvent<string>>(e => UpdateType(armorDropDownField));
        }
        weaponDropDownField.index = 0;
        armorDropDownField.index = 0;
        return _RootElement;
        
    }

    public void UpdateType(DropdownField dropdownField)
    {
    }

    /*
    public override void OnInspectorGUI()
    {
        InventoryManager inventoryManager = target as InventoryManager;

        if(inventoryManager == null)
        {
            return;
        }
        if(_weaponInplementations == null || GUILayout.Button("Refresh implementations"))
        {
            _weaponInplementations = GetImplementations<Weapon>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }
        EditorGUILayout.LabelField($"Found{_weaponInplementations.Length} implementations");

        _weaponInplementationsTypeIndex = EditorGUILayout.Popup(new GUIContent("WeaponImplementation"),
           _weaponInplementationsTypeIndex, _weaponInplementations.Select(impl => impl.FullName).ToArray());

        if (GUILayout.Button("Create instance"))
        {
            var weapon = (Weapon)Activator.CreateInstance(_weaponInplementations[_weaponInplementationsTypeIndex]);

            inventoryManager.weapons.Add(weapon);
            //.typeId = (PolyInteractiveData.TypeId)Enum.Parse(typeof(PolyInteractiveData.TypeId), _implementations[_implementationTypeIndex].FullName);
        }

        if(_armorInplementations == null || GUILayout.Button("Refresh implementations"))
        {
            _armorInplementations = GetImplementations<Armor>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        _armorInplementationsTypeIndex = EditorGUILayout.Popup(new GUIContent("ArmorImplementation"),
           _armorInplementationsTypeIndex, _armorInplementations.Select(impl => impl.FullName).ToArray());

        if (GUILayout.Button("Create instance"))
        {
            var armor = (Armor)Activator.CreateInstance(_armorInplementations[_armorInplementationsTypeIndex]);

            inventoryManager.armor.Add(armor);
            //.typeId = (PolyInteractiveData.TypeId)Enum.Parse(typeof(PolyInteractiveData.TypeId), _implementations[_implementationTypeIndex].FullName);
        }

        base.OnInspectorGUI();
    }*/
    private static Type[] GetImplementations<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());


        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}
