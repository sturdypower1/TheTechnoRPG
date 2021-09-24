using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

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

    private void OnEnable()
    {
        _inventoryManager = target as InventoryManager;

        _RootElement = new VisualElement();

        _VisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/EditorUIElements/UXML/InventoryTemplate.uxml");
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/EditorUIElements/USS/EditorStyleSheet.uss");


    }

    public override VisualElement CreateInspectorGUI()
    {
        _RootElement.Clear();

        _VisualTree.CloneTree(_RootElement);

        return _RootElement;
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
