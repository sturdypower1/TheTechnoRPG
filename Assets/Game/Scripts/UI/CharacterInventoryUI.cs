using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(UIDocument))]
public class CharacterInventoryUI : MonoBehaviour
{
    private PlayerController playerController;

    private UIDocument _UIDoc;
    protected VisualElement _background;

    public void EnableUI()
    {
        _background.visible = true;
       
        UpdateUI();
    }
    public void DisableUI()
    {
        _background.visible = false;
    }
    public void SetPlayerController(PlayerController PlayerController)
    {
        playerController = PlayerController;
    }

    public void UpdateUI()
    {
        CharacterStats currentStats = playerController.stats;
        UpdateHPUI(currentStats);
        UpdatePointsUI(currentStats);
        UpdateStats(currentStats);
    }
    protected virtual void UpdateStats(CharacterStats currentStats)
    {
        var levelData = playerController.levelUpController;

        Label currentLevel = _background.Q<Label>("current_level");
        Label currentAttack = _background.Q<Label>("current_attack");
        Label currentDefence = _background.Q<Label>("current_defence");
        Label neededEXP = _background.Q<Label>("EXP");

        currentLevel.text = "LVL " + levelData.currentLVL.ToString();
        currentAttack.text = "ATK: " + (currentStats.stats.attack + currentStats.equipedWeapon.attack);
        currentDefence.text = "DEF: " + (currentStats.stats.defence + currentStats.equipedArmor.defence);
        neededEXP.text = "EXP needed: " + (levelData.requiredEXP - levelData.currentEXP).ToString();
    }
    protected virtual void UpdatePointsUI(CharacterStats currentStats)
    {
        VisualElement manaBar = _background.Q<VisualElement>("mana_bar");
        VisualElement manaBarBase = _background.Q<VisualElement>("mana_bar_base");
        Label manaBarText = _background.Q<Label>("mana_text");

        manaBar.style.width = manaBarBase.contentRect.width * ((float)currentStats.stats.points / (float)currentStats.stats.maxPoints);
        manaBarText.text = "Mana: " + currentStats.stats.points.ToString() + "/" + currentStats.stats.maxPoints.ToString();
    }
    protected virtual void UpdateHPUI(CharacterStats currentStats)
    {
        Label healthBarText = _background.Q<Label>("health_text");
        VisualElement healthBarBase = _background.Q<VisualElement>("health_bar_base");
        VisualElement healthBar = _background.Q<VisualElement>("health_bar");

        healthBar.style.width = healthBarBase.contentRect.width * ((float)currentStats.stats.health / (float)currentStats.stats.maxHealth);
        healthBarText.text = "HP: " + currentStats.stats.health.ToString() + "/" + currentStats.stats.maxHealth.ToString();
    }
    private void Start()
    {
        _UIDoc = GetComponent<UIDocument>();
        var root = _UIDoc.rootVisualElement;
        _background = root.Q<VisualElement>("background");
    }
}
