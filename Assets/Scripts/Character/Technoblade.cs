using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
public class Technoblade : MonoBehaviour
{
    public static Technoblade instance;

    public CharacterStats stats;
    [HideInInspector]
    public Battler battler;

    public VisualElement technoSelectorUI;

    LevelUpController levelUpController;
    Animator animator;


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
        levelUpController = this.gameObject.GetComponent<LevelUpController>();
        animator = this.gameObject.GetComponent<Animator>();
        battler = this.gameObject.GetComponent<Battler>();

        SaveAndLoadManager.instance.OnStartSave += Save;
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


            if (battler.useTime < battler.maxUseTime && !BattleManager.instance.IsWaitingForSkill)
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
            else if(battler.useTime >= battler.maxUseTime && !BattleManager.instance.IsWaitingForSkill)
            {
                technoSelectorUI.SetEnabled(true);
            }
            
        }
    }
    public void AddBlood(int bloodToAdd)
    {
        // makes it so if techno is down he doesn't continue to get blood
        if (battler.isDown)
        {
            return;
        }
        if(stats.stats.points + bloodToAdd >= stats.stats.maxPoints)
        {
            stats.stats.points = stats.stats.maxPoints;
        }
        else
        {
            stats.stats.points += bloodToAdd;
        }
    }

    public void Save(int saveFileNumber)
    {
        //save level data
        string savePath = Application.persistentDataPath + "/tempsave" + "/techno";
        AnimationSaveData animationSaveData = new AnimationSaveData
        {
            name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
            normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        };
        TechnoSaveData saveData = new TechnoSaveData { characterStats = stats, animationSave = animationSaveData, transform = transform.position};
        
        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, jsonString);
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/techno";
        string jsonString = File.ReadAllText(savePath);
        TechnoSaveData saveData = JsonUtility.FromJson<TechnoSaveData>(jsonString);
        stats = saveData.characterStats;
        animator.Play(saveData.animationSave.name, 0, saveData.animationSave.normilizedtime);
        transform.position = saveData.transform;
    }
}
[System.Serializable]
public struct TechnoSaveData{
    public CharacterStats characterStats;
    public AnimationSaveData animationSave;
    public Vector2 transform;
}

