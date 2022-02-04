/*#if UNITY_EDITOR
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
[CustomEditor(typeof(CharacterStats))]
public class CharacterStatsInspector : Editor
{
    private Type[] _weaponInplementations;
    private int _weaponInplementationsTypeIndex;

    private Type[] _armorInplementations;
    private int _armorInplementationsTypeIndex;

    private Type[] _skillInplementations;

    private Type[] _itemInplementations;

    CharacterStats characterStats;

    private VisualElement _RootElement;
    private VisualTreeAsset _VisualTree;


    SerializedProperty _weaponProperty;

    private void OnEnable()
    {
        characterStats = target as CharacterStats;

        _RootElement = new VisualElement();

        _VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/EditorUIElements/UXML/CharacterStatsTemplate.uxml");
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/EditorUIElements/USS/EditorStyleSheet.uss");


    }
    public override VisualElement CreateInspectorGUI()
    {
        serializedObject.Update();
        _weaponProperty = serializedObject.FindProperty("equipedWeapon");
        SerializedProperty _armorProperty = serializedObject.FindProperty("equipedArmor");

        ForceRepaint();

        return _RootElement;
    }
    /// <summary>
    /// creates the ui for the characterstats
    /// </summary>
    public void ForceRepaint()
    {
        serializedObject.Update();
        _weaponProperty = serializedObject.FindProperty("equipedWeapon");
        SerializedProperty _armorProperty = serializedObject.FindProperty("equipedArmor");
        SerializedProperty _skillProperty = serializedObject.FindProperty("skills");
        SerializedProperty _statsProperty = serializedObject.FindProperty("stats");

        _RootElement.Clear();

        _VisualTree.CloneTree(_RootElement);

        PropertyField statsPropertyField = _RootElement.Q<PropertyField>("stats");
        statsPropertyField.BindProperty(_statsProperty);

        DropdownField weaponDropDownField = _RootElement.Q<DropdownField>("weapon_choices");
        weaponDropDownField.choices.Clear();
        weaponDropDownField.RegisterCallback<ChangeEvent<String>>(e => SetWeapon());

        DropdownField armorDropDownField = _RootElement.Q<DropdownField>("armor_choices");
        armorDropDownField.choices.Clear();
        armorDropDownField.RegisterCallback<ChangeEvent<string>>(e => SetArmor());

        PropertyField weaponProperty = _RootElement.Q<PropertyField>("equiped_weapon");
        weaponProperty.BindProperty(_weaponProperty);

        PropertyField armorProperty = _RootElement.Q<PropertyField>("equiped_armor");
        armorProperty.BindProperty(_armorProperty);

        DropdownField skillDropDownField = _RootElement.Q<DropdownField>("skill_choices");
        skillDropDownField.choices.Clear();

        Button skillAddButton = _RootElement.Q<Button>("add_skill_button");
        skillAddButton.clicked += AddSkill;

        PropertyField skillsProperty = _RootElement.Q<PropertyField>("skills");
        skillsProperty.BindProperty(_skillProperty);


        _weaponInplementations = GetImplementations<Weapon>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        
        foreach (Type type in _weaponInplementations)
        {
            weaponDropDownField.choices.Add(type.FullName);
            //weaponDropDownField.RegisterCallback<ChangeEvent<string>>(e => UpdateType(weaponDropDownField));
        }


         _armorInplementations = GetImplementations<Armor>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();

        foreach (Type type in _armorInplementations)
        {
            armorDropDownField.choices.Add(type.FullName);
        }

        _skillInplementations = GetImplementations<Skill>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();

        foreach(Type type in _skillInplementations)
        {
            skillDropDownField.choices.Add(type.FullName);
        }
        skillDropDownField.index = 0;
    }
    public void AddSkill()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("skill_choices");
        int index = dropdownField.index;
        var skill = (Skill)Activator.CreateInstance(_skillInplementations[index]);
        skill.skillType = _skillInplementations[index].FullName;
        characterStats.skills.Add(skill);
    }
    public void SetWeapon()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("weapon_choices");
        int index = dropdownField.index;
        var weapon = (Weapon)Activator.CreateInstance(_weaponInplementations[index]);
        weapon.WeaponType = _weaponInplementations[index].FullName;
        characterStats.equipedWeapon = weapon;

        ForceRepaint();

    }
    public void SetArmor()
    {
        DropdownField dropdownField = _RootElement.Q<DropdownField>("armor_choices");
        int index = dropdownField.index;
        var armor = (Armor)Activator.CreateInstance(_armorInplementations[index]);
        armor.ArmorType = _armorInplementations[index].FullName;
        characterStats.equipedArmor = armor;

        ForceRepaint();
        
    }
    private static Type[] GetImplementations<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());


        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}
#endif*/