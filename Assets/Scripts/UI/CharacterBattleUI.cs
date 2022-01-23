using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBattleUI : MonoBehaviour
{
    UIDocument UIDoc;
    VisualElement root;
    // the ui for selecting either an attack, skill, or other options
    VisualElement optionSelect;
    // the ui for selecting the enemy, it will be cleared every time an enemy is selected
    VisualElement selectionMenu;
    // the scroll for all the enemies to escape
    ScrollView selectionScroll;


    Action cachedHandler;
    Battler battler;

    private void Start()
    {
        UIDoc = GetComponent<UIDocument>();
        root = UIDoc.rootVisualElement;
        DisableUI();

    }

    //Attack needs to first wait to be pressed to do anything
    // after the attack button is pressed

    /// <summary>
    /// use the first skill in the list of the character's skills to target an enemy
    /// </summary>
    public void AttackButton()
    {
        AudioManager.playSound("menuchange");
        DisableOptionMenu();

        foreach (Battler enemyBattler in BattleManager.instance.Enemies)
        {
            TemplateContainer template = enemyBattler.GetSelectionTemplate();

            Button button = template.Q<Button>("Base");
            button.clicked += () => AttackEnemySelectButton(enemyBattler, button);
        }
    }

    private void AttackEnemySelectButton(Battler enemy, Button selectButton)
    {
        AudioManager.playSound("menuchange");
        // reseting focus so you can't select the button again
        UIManager.instance.ResetFocus();
        CharacterStats stats = battler.characterStats;

        // the first skill is the default attack
        
        optionSelect.visible = true;
        optionSelect.SetEnabled(false);

        // clearing the enemies since they aren't needed anymore
        DisableSelection();

        Skill skill = stats.skills[0];
        skill.UseSkill(enemy, battler);
    }
    /// <summary>
    /// plays an animation for disabling the option menu
    /// </summary>
    private void DisableOptionMenu()
    {
        // TODO: add ui animation
        optionSelect.visible = false;
    }
    /// <summary>
    /// plays an animation for disabling the selection menu
    /// </summary>
    private void DisableSelection()
    {
        //TODO: add ui animation
        selectionMenu.visible = false;
        selectionScroll.Clear();
    }

    private void EnableOptionMenu()
    {
        // TODO: add ui animation
        optionSelect.visible = true;
    }

    private void EnableSelection()
    {
        //TODO: add ui animation
        selectionMenu.visible = false;
    }
    public void EnableUI()
    {
        root.visible = true;

    }

    public void DisableUI()
    {
        root.visible = false;
    }
}
