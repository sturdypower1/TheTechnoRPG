using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
public class Technoblade : PlayerController
{
    public static Technoblade instance;

    private bool isSetup =false;

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
        inventoryUI.SetPlayerController(this);
        isSetup = true;
        PlayerPartyManager.instance.AddPlayer("Technoblade");
        FileSaveManager.instance.OnStartSave += Save;
        SaveAndLoadManager.instance.OnReLoadSave += Load;

        MainGameManager.instance.OnTitleReturn += DestroyTechno;
        
        levelUpController = this.gameObject.GetComponent<LevelUpController>();
        animator = this.gameObject.GetComponent<Animator>();
        battler = this.gameObject.GetComponent<Battler>();
        stats = this.gameObject.GetComponent<CharacterStats>();
        Load();
        
    }
    private void Update()
    {
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

        List<string> skillNames = new List<string>();
        foreach(Skill skill in stats.skills)
        {
            skillNames.Add(skill.name);
        }
        TechnoSaveData saveData = new TechnoSaveData { 
            characterStats = new CharacterStatsSaveData {stats = stats.stats, 
                armorName = stats.equipedArmor.name, 
                weaponName = stats.equipedWeapon.name, 
                skillNames = skillNames
            }, 
            animationSave = animationSaveData, 
            transform = transform.position,
            levelUpSave = new LevelUpSave { currentEXP = levelUpController.currentEXP, currentLVL = levelUpController.currentLVL, LevelCap = levelUpController.LevelCAP, requiredEXP = levelUpController.requiredEXP}
        };


        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, jsonString);
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/techno";
        if (File.Exists(savePath))
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = true;

            string jsonString = File.ReadAllText(savePath);
            TechnoSaveData saveData = JsonUtility.FromJson<TechnoSaveData>(jsonString);
            stats.stats = saveData.characterStats.stats;

            stats.skills.Clear();
            foreach(string skillName in saveData.characterStats.skillNames)
            {
                stats.skills.Add(Resources.Load(skillName) as Skill);
            }
            
            stats.equipedWeapon = Resources.Load(saveData.characterStats.weaponName) as Weapon;
            stats.equipedArmor = Resources.Load(saveData.characterStats.armorName) as Armor;

            //Debug.Log(saveData.characterStats.skills.Count);

            animator.Play(saveData.animationSave.name, 0, saveData.animationSave.normilizedtime);
            transform.position = saveData.transform;

            levelUpController.LevelCAP = saveData.levelUpSave.LevelCap;
            levelUpController.currentLVL = saveData.levelUpSave.currentLVL;
            levelUpController.requiredEXP = saveData.levelUpSave.requiredEXP;
            levelUpController.currentEXP = saveData.levelUpSave.currentEXP;
        }
    }
    private void OnDestroy()
    {
        if (!isSetup) return;
        // makes sure that all subribed events are unsubscribed
        PlayerPartyManager.instance.RemovePlayer("Technoblade");
        FileSaveManager.instance.OnStartSave -= Save;
        //UIManager.instance.OnTitleReturn -= DestroyTechno;
        SaveAndLoadManager.instance.OnReLoadSave -= Load;
    }
    public void DestroyTechno()
    {
        Destroy(this.gameObject);
    }
}
[System.Serializable]
public struct TechnoSaveData{
    public CharacterStatsSaveData characterStats;
    public AnimationSaveData animationSave;
    public Vector2 transform;
    public LevelUpSave levelUpSave;
}
[System.Serializable]
public struct CharacterStatsSaveData
{
    public Stats stats;
    public string weaponName;
    public string armorName;
    public List<string> skillNames;
}
[System.Serializable]
public struct LevelUpSave
{
    public int LevelCap;
    public int currentLVL;
    public int requiredEXP;
    public int currentEXP;
}

