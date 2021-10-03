using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Technoblade : MonoBehaviour
{
    public static Technoblade instance;

    public CharacterStats stats;
    [HideInInspector]
    public Battler battler;

    public VisualElement technoSelectorUI;

    private void Awake() {
        if(instance == null || instance == this){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        battler = this.gameObject.GetComponent<Battler>();
    }
    private void Update()
    {
        if (BattleManager.instance.isInBattle)
        {
            InventoryManager inventory = InventoryManager.instance;
            // there are no items, so don't let them go into the menu
            if(inventory.items.Count == 0)
            {
                technoSelectorUI.Q<Button>("items").SetEnabled(false);
            }
            else
            {
                technoSelectorUI.Q<Button>("items").SetEnabled(true);
            }

            Label healthText = technoSelectorUI.Q<Label>("health_text");
            VisualElement healthBarBase = technoSelectorUI.Q<VisualElement>("health_bar_base");
            VisualElement healthBar = technoSelectorUI.Q<VisualElement>("health_bar");

            Label bloodText = technoSelectorUI.Q<Label>("blood_text");
            VisualElement bloodBarBase = technoSelectorUI.Q<VisualElement>("blood_bar_base");
            VisualElement bloodBar = technoSelectorUI.Q<VisualElement>("blood_bar");

            healthBar.style.width = healthBarBase.contentRect.width * (stats.stats.health / stats.stats.maxHealth);
            healthText.text = "HP: " + stats.stats.health.ToString() + "/" + stats.stats.maxHealth.ToString();

            bloodBar.style.width = bloodBarBase.contentRect.width * ((float)stats.stats.points / stats.stats.maxPoints);
            bloodText.text = "Blood: " + stats.stats.points.ToString() + "/" + stats.stats.maxPoints.ToString();


            if (battler.useTime < battler.maxUseTime)
            {
                VisualElement useBar = technoSelectorUI.Q<VisualElement>("use_bar");

                useBar.style.width = technoSelectorUI.contentRect.width * ((battler.useTime) / battler.maxUseTime);
                battler.useTime += Time.unscaledDeltaTime;
                if(battler.useTime >= battler.maxUseTime)
                {
                    technoSelectorUI.SetEnabled(true);
                    AudioManager.playSound("menuavailable");
                }
            }
            else if(battler.useTime >= battler.maxUseTime)
            {
                technoSelectorUI.SetEnabled(true);
            }
            
        }
    }
}
