using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TechnoInventoryUI : CharacterInventoryUI
{
    protected override void UpdatePointsUI(CharacterStats currentStats)
    {
        VisualElement bloodBar = _background.Q<VisualElement>("blood_bar");
        VisualElement bloodBarBase = _background.Q<VisualElement>("blood_bar_base");
        Label bloodBarText = _background.Q<Label>("blood_text");

        bloodBar.style.width = bloodBarBase.contentRect.width * ((float)currentStats.stats.points / (float)currentStats.stats.maxPoints);
        bloodBarText.text = "Blood: " + currentStats.stats.points.ToString() + "/" + currentStats.stats.maxPoints.ToString();
    }
}
