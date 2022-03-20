using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(UIDocument))]
public class CharacterInventoryUI : MonoBehaviour
{
    private UIDocument _UIDoc;
    private VisualElement _background;

    public void EnableUI()
    {
        _background.visible = true;
       
        UpdateUI();
    }
    public void DisableUI()
    {
        _background.visible = false;
    }

    public void UpdateUI()
    {

    }
    private void Start()
    {
        _UIDoc = GetComponent<UIDocument>();
        var root = _UIDoc.rootVisualElement;
        _background = root.Q<VisualElement>("background");
    }
}
