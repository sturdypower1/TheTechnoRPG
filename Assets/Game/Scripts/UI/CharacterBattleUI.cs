using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBattleUI : MonoBehaviour
{
    public PlayerBattler battler;

    public event SelectedEventHandler targetSelected;

    UIDocument UIDoc;
    VisualElement root;
    VisualElement baseUI;
    // the ui for selecting either an attack, skill, or other options
    VisualElement optionSelect;
    Button fightChoice;
    Button skillsChoice;
    Button itemsChoice;
    Button defendChoice;
    // the ui for selecting the enemies, items, and skills
    VisualElement selectionMenu;
    Label selectionDescription;
    ScrollView selectionScroll;
    Button selectionBackButton;

    VisualElement bottomSpacingElement;

    VisualElement useBar;

    Label healthText;
    VisualElement healthBarBase;
    VisualElement healthBar;

    Label pointText;
    VisualElement pointBarBase;
    VisualElement pointBar;

    bool isBottomGrowing;
    Tween bottomTween;
    public void UpdateHealth(int health, int maxHealth)
    {
        var newWidth = healthBarBase.contentRect.width * ((float)health / (float)maxHealth);
        DOVirtual.Float(healthBar.contentRect.width, newWidth, .5f, v =>
        {
            healthBar.style.width = v;
        });
        //healthBar.style.width = healthBarBase.contentRect.width * ((float)health / (float)maxHealth);
        healthText.text = "HP: " + health.ToString() + "/" + maxHealth.ToString();
    }

    public void UpdatePoints(int points, int maxPoints)
    {
        pointBar.style.width = pointBarBase.contentRect.width * ((float)points / maxPoints);
        pointText.text = "Blood: " + points.ToString() + "/" + maxPoints.ToString();
    }
    
    public void StartRecoveryBarTween(float duration)
    {
        DOVirtual.Float(0, baseUI.contentRect.width, duration, v =>
        {
            useBar.style.width = v;
        });
    }
    
    public void GoToStartingState()
    {
        EnableOptionMenu();
        DisableSelection();
    }

    public void EnableUI()
    {
        root.visible = true;

    }

    public void SetItemsOption(bool isEnabled)
    {
        itemsChoice.SetEnabled(isEnabled);
    }

    public void DisableUI()
    {
        root.visible = false;
    }
    private void Start()
    {
        UIDoc = GetComponent<UIDocument>();
        root = UIDoc.rootVisualElement;
        baseUI = root.Q<VisualElement>("character");

        optionSelect = root.Q<VisualElement>("choice_bar");
        fightChoice = optionSelect.Q<Button>("fight");
        fightChoice.clicked += AttackButton;
        skillsChoice = optionSelect.Q<Button>("skills");
        skillsChoice.clicked += SkillsButton;
        itemsChoice = optionSelect.Q<Button>("items");
        itemsChoice.clicked += ItemsButton;
        defendChoice = optionSelect.Q<Button>("defend");
        defendChoice.clicked += DefendButton;

        selectionMenu = root.Q<VisualElement>("selection_menu");
        selectionScroll = selectionMenu.Q<ScrollView>("scroll_view");
        selectionDescription = selectionMenu.Q<Label>("description");
        selectionBackButton = selectionMenu.Q<Button>("back_button");
        selectionBackButton.clicked += SelectionBackButton;

        bottomSpacingElement = root.Q<VisualElement>("bottom_spacing");

        useBar = root.Q<VisualElement>("use_bar");

        healthText = root.Q<Label>("health_text");
        healthBarBase = root.Q<VisualElement>("health_bar_base");
        healthBar = root.Q<VisualElement>("health_bar");

        pointText = root.Q<Label>("blood_text");
        pointBarBase = root.Q<VisualElement>("blood_bar_base");
        pointBar = root.Q<VisualElement>("blood_bar");

        DisableUI();
    }

    private void UpdateSelectionDescription(string description)
    {
        selectionDescription.text = description;
    }
    
    //Attack needs to first wait to be pressed to do anything
    //after the attack button is pressed

    /// <summary>
    /// use the first skill in the list of the character's skills to target an enemy
    /// </summary>
    private void AttackButton()
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

        targetSelected?.Invoke(new OnSelectedEventArgs { target = enemy, skillNumber = 0 });
        // clearing the enemies since they aren't needed anymore
        DisableSelection();
    }
    
    public void SkillsButton()
    {
        AudioManager.playSound("menuchange");
        EnableSelection();
        DisableOptionMenu();
        UIManager.instance.ResetFocus();
        CharacterStats stats = battler.characterStats;
        int i = 1;
        while (i < stats.skills.Count)
        {
            Skill skill = stats.skills[i];
            int z = i;
            Button skillButton = new Button();
            skillButton.focusable = true;

            skillButton.text = skill.name.ToString();
            skillButton.clicked += () => SkillButton(z);
            skillButton.RegisterCallback<FocusEvent>(ev => UpdateSelectionDescription(skill.description.ToString()));
            skillButton.RegisterCallback<PointerEnterEvent>(ev => UpdateSelectionDescription(skill.description.ToString()));

            skillButton.AddToClassList("item_button");
            selectionScroll.Add(skillButton);
            if (i == 1)
            {
                UpdateSelectionDescription(skill.description.ToString());
                skillButton.Focus();
            }
            i++;
        }
    }
    private void SkillButton(int skillNumber)
    {
        CharacterStats stats = battler.characterStats;
        //checking if there have enough points to use the move
        selectionScroll.Clear();
        if (stats.stats.points >= stats.skills[skillNumber].cost)
        {
            AudioManager.playSound("menuchange");
            int i = 0;
            foreach(Battler enemy in BattleManager.instance.Enemies)
            {
                

                Button button = new Button();//template.Q<Button>("Base");
                button.text = enemy.characterStats.name;
                button.clicked += () => SkillEnemySelectButton(enemy, skillNumber);
                Debug.Log("make sure the enemy description updaetes");
                selectionScroll.Add(button);

                if (i == 0)
                {
                    UpdateSelectionDescription("");
                }
                i++;
            }
        }
    }
    private void SkillEnemySelectButton(Battler enemy, int currentSkill)
    {
        AudioManager.playSound("menuselect");
        DisableSelection();

        var stats = battler.characterStats;
        targetSelected.Invoke(new OnSelectedEventArgs { skillNumber = currentSkill, target = enemy });
    }
    
    private void ItemsButton()
    {
        EnableSelection();
        DisableOptionMenu();

        AudioManager.playSound("menuchange");

        InventoryManager inventory = InventoryManager.instance;
        int i = 0;
        selectionScroll.Clear();
        
        // changing the names of the items
        while (i < inventory.items.Count)
        {
            int z = i;
            Item item = inventory.items[i];
            Button button = new Button();
            button.focusable = true;
            button.text = item.name;
            //button.AddToClassList("item_button");
            button.clicked += () => ItemButton(z);
            selectionScroll.Add(button);
            //button.RegisterCallback<FocusEvent>(ev => UpdateSelectionDescription(item.description));
            //button.RegisterCallback<PointerEnterEvent>(ev => UpdateSelectionDescription(item.description));
            // if it isn't usable, don't let the player use the item
            //item.GetUseability()
            if (true)
            {
                button.SetEnabled(true);
            }
            else
            {
                button.SetEnabled(false);
            }
            if (i == 0)
            {
                button.Focus();
                UpdateSelectionDescription(item.description);
            }
            i++;
        }
    }
    private void ItemButton(int itemNumber)
    {
        AudioManager.playSound("menuchange");
        DisableSelection();
        EnableOptionMenu();

        InventoryManager inventory = InventoryManager.instance;

        Item item = inventory.items[itemNumber];

        // need to change this when multiple players are added
        item.UseItem(battler.gameObject);

    }

    private void DefendButton()
    {
        DisableOptionMenu();
        if (battler is TechnobladeBattler)
        {
            TechnobladeBattler technoBattler = battler as TechnobladeBattler;
            if (technoBattler.isInCarnageMode) return;
        }
        battler.Defend();
    }
    
    private void SelectionBackButton()
    {
        EnableOptionMenu();
        DisableSelection();
    }

    /// <summary>
    /// plays an animation for disabling the option menu
    /// </summary>
    private void DisableOptionMenu()
    {
        optionSelect.SetEnabled(false);

        var tween = DOVirtual.Float(0, optionSelect.contentRect.height, .25f, v =>
        {
            optionSelect.style.top = v;
        });
        tween.onComplete += () => optionSelect.style.display = DisplayStyle.None;

        if (bottomTween == null)
        {
            bottomTween = DOVirtual.Float(optionSelect.contentRect.height, 0, .25f, v =>
            {
                ChangeBottomSpacing(v);
            });
            bottomTween.onComplete += () => bottomTween = null;
        }
        
    }
    /// <summary>
    /// plays an animation for disabling the selection menu
    /// </summary>
    private void DisableSelection()
    {
        selectionMenu.SetEnabled(false);
        var tween = DOVirtual.Float(0, selectionMenu.contentRect.height, .25f, v =>
        {
            if(!isBottomGrowing) ChangeBottomSpacing(selectionMenu.contentRect.height - v);
            selectionMenu.style.top = v;
        });
        tween.onComplete += () =>
        {
            selectionMenu.style.display = DisplayStyle.None;
            selectionScroll.Clear();
        };

        if (bottomTween == null)
        {
            bottomTween = DOVirtual.Float(optionSelect.contentRect.height, 0, .25f, v =>
            {
                ChangeBottomSpacing(v);
            });
            bottomTween.onComplete += () => bottomTween = null;
        }
    }

    private void EnableOptionMenu()
    {
        optionSelect.SetEnabled(true);
        optionSelect.style.display = DisplayStyle.Flex;
        isBottomGrowing = true;
        var tween = DOVirtual.Float(optionSelect.contentRect.height, 0, .25f, v =>
        {
            optionSelect.style.top = v;
        });
        bottomTween?.Kill();
        bottomTween = DOVirtual.Float(bottomSpacingElement.contentRect.height, 84, .25f, v =>
        {
            ChangeBottomSpacing(v);
        });
        bottomTween.onComplete += () => bottomTween = null;
    }

    private void EnableSelection()
    {
        selectionMenu.SetEnabled(true);
        selectionMenu.style.display = DisplayStyle.Flex;
        var tween = DOVirtual.Float(selectionMenu.contentRect.height, 0, .25f, v =>
        {
            selectionMenu.style.top = v;
        });

        bottomTween?.Kill();
        // goes from the 
        bottomTween = DOVirtual.Float(bottomSpacingElement.contentRect.height, 120, .25f, v =>
        {
            //Debug.Log(selectionMenu.contentRect.height);
            ChangeBottomSpacing(v);
        });
        bottomTween.onComplete += () => bottomTween = null;
    }
    
    private void ChangeBottomSpacing(float newSize)
    {
        //Debug.Log(newSize);
        bottomSpacingElement.style.height = new StyleLength(newSize);
    }

    public delegate void SelectedEventHandler(OnSelectedEventArgs e);
    public class OnSelectedEventArgs : EventArgs
    {
        public Battler target { get; set; }
        public int skillNumber { get; set; }
    }
}
