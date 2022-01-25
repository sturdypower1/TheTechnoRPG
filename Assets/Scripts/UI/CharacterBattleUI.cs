using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBattleUI : MonoBehaviour
{
    UIDocument UIDoc;
    VisualElement root;
    VisualElement baseUI;
    // the ui for selecting either an attack, skill, or other options
    VisualElement optionSelect;
    Button fightChoice;
    Button skillsChoice;
    Button itemsChoice;
    Button defendChoice;
    // the ui for selecting the enemy, it will be cleared every time an enemy is selected
    VisualElement selectionMenu;
    // the scroll for all the enemies to escape
    ScrollView selectionScroll;

    VisualElement useBar;

    Label healthText;
    VisualElement healthBarBase;
    VisualElement healthBar;

    Label pointText;
    VisualElement pointBarBase;
    VisualElement pointBar;
    

    Action cachedHandler;

    public event SelectedEventHandler targetSelected;

    Battler battler;

    private void Start()
    {
        UIDoc = GetComponent<UIDocument>();
        root = UIDoc.rootVisualElement;
        baseUI = root.Q<VisualElement>("character");

        optionSelect = root.Q<VisualElement>("choice_bar");
        fightChoice = optionSelect.Q<Button>("fight");
        fightChoice.clicked += AttackButton;
        skillsChoice = optionSelect.Q<Button>("items");
        defendChoice = optionSelect.Q<Button>("defend");

        selectionMenu = root.Q<VisualElement>("selection_menu");
        selectionScroll = selectionMenu.Q<ScrollView>("scroll_view");

        useBar = root.Q<VisualElement>("use_bar");

        healthText = root.Q<Label>("health_text");
        healthBarBase = root.Q<VisualElement>("health_bar_base");
        healthBar = root.Q<VisualElement>("health_bar");

        pointText = root.Q<Label>("blood_text");
        pointBarBase = root.Q<VisualElement>("blood_bar_base");
        pointBar = root.Q<VisualElement>("blood_bar");

        DisableUI();
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        healthBar.style.width = healthBarBase.contentRect.width * ((float)health / (float)maxHealth);
        healthText.text = "HP: " + health.ToString() + "/" + maxHealth.ToString();
    }

    public void UpdatePoints(int points, int maxPoints)
    {
        pointBar.style.width = pointBarBase.contentRect.width * ((float)points / maxPoints);
        pointText.text = "Blood: " + points.ToString() + "/" + maxPoints.ToString();
    }
    
    public void UpdateUseBar(float useTime, float maxUseTime)
    {
        useBar.style.width = baseUI.contentRect.width * ((useTime) / maxUseTime);
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
        EnableSelection();

        foreach (Battler enemyBattler in BattleManager.instance.Enemies)
        {
            Debug.Log("make it so the ui is custom");
            //TemplateContainer template = enemyBattler.GetSelectionTemplate();

            Button button = new Button();//template.Q<Button>("Base");
            button.clicked += () => AttackEnemySelectButton(enemyBattler, button);
            button.text = enemyBattler.characterStats.name;
            selectionScroll.Add(button);
        }
    }

    private void AttackEnemySelectButton(Battler enemy, Button selectButton)
    {
        AudioManager.playSound("menuchange");
        // reseting focus so you can't select the button again
        UIManager.instance.ResetFocus();

        targetSelected.Invoke(new OnSelectedEventArgs { target = enemy, skillNumber = 0 });
        // clearing the enemies since they aren't needed anymore
        DisableSelection();
    }
    /// <summary>
    /// plays an animation for disabling the option menu
    /// </summary>
    private void DisableOptionMenu()
    {
        // TODO: add ui animation
        optionSelect.style.display = DisplayStyle.None;
    }
    /// <summary>
    /// plays an animation for disabling the selection menu
    /// </summary>
    private void DisableSelection()
    {
        //TODO: add ui animation
        selectionMenu.style.display = DisplayStyle.None;
        selectionScroll.Clear();
    }

    private void EnableOptionMenu()
    {
        // TODO: add ui animation
        optionSelect.style.display = DisplayStyle.Flex;
    }

    private void EnableSelection()
    {
        //TODO: add ui animation
        selectionMenu.style.display = DisplayStyle.Flex;
    }
    public void EnableUI()
    {
        root.visible = true;
             
    }

    public void SetItemsOption(bool isEnabled)
    {
        Debug.Log("need to implement items disable");
    }

    public void DisableUI()
    {
        root.visible = false;
    }
    public delegate void SelectedEventHandler(OnSelectedEventArgs e);
    public class OnSelectedEventArgs : EventArgs
    {
        public Battler target { get; set; }
        public int skillNumber { get; set; }
    }
}
