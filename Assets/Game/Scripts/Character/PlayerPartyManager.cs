using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    public static PlayerPartyManager instance;
    
    [HideInInspector]public List<PlayerController> players= new List<PlayerController>();
    
    private PlayerController Leader;

    private List<PlayerLevelUpData> lastPlayerLevelUps;
    
    public List<PlayerLevelUpData> GetLastLevelUps()
    {
        return lastPlayerLevelUps;
    }
    public void HealPlayers()
    {
        foreach(PlayerController player in players)
        {
            CharacterStats characterStats = player.stats;

            characterStats.stats.health = characterStats.stats.maxHealth;
        }
    }
    public void AddPlayer(string playerName)
    {
        GameObject currentPlayer = null;

        if(playerName == "Technoblade")
        {
            Leader = Technoblade.instance;
            players.Insert(0, Technoblade.instance);
            currentPlayer = Technoblade.instance.gameObject;
        }
        if(playerName == "Steve" && STEVE.instance != null)
        {
            players.Insert((players.Count == 0 ? 0 : 1) , STEVE.instance);
            AIDestinationSetter aiSetter = STEVE.instance.gameObject.GetComponent<AIDestinationSetter>();
            aiSetter.target = Technoblade.instance.gameObject.transform;
            currentPlayer = STEVE.instance.gameObject;
        }
        // adds player if there is one and if it does already have that player
        /*if(currentPlayer != null && !players.Contains(currentPlayer))
        {
            
            players.Add(currentPlayer);
            if(players.Count == 0)
            {
                Leader = currentPlayer;
            }
        }*/
    }
    public void RemovePlayer(string playerName)
    {
        if(playerName == "Technoblade" && Technoblade.instance != null)
        {
            if (players.Contains(Technoblade.instance))
            {
                players.Remove(Technoblade.instance);
            }
        }
        if(playerName == "Steve" && STEVE.instance != null)
        {
            if (players.Contains(STEVE.instance))
            {
                players.Remove(STEVE.instance);
                
                
            }
        }
    }

    public bool HasPlayer(PlayerController player)
    {
        return players.Contains(player);
    }
    
    public void BattleEnd(int experience)
    {
        lastPlayerLevelUps = AddExperiance(experience);
        
        foreach(PlayerController player in players)
        {
            var battler = player.battler;
            battler.BattleEnd();
        }
    }
    public void BattleLose()
    {
        foreach (PlayerController player in players)
        {
            var battler = player.battler as PlayerBattler;
            battler.PlayerLose();
        }
    }
    public void DisablePlayerInventoryUI()
    {
        foreach (PlayerController player in players)
        {
            player.DisableInventoryUI();
        }
    }
    public void EnablePlayerInventoryUI()
    {
        foreach(PlayerController player in players)
        {
            player.EnableInventoryUI();
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// returns all the levelups that occur
    /// </summary>
    /// <param name="experiance"></param>
    private List<PlayerLevelUpData> AddExperiance(int experiance)
    {
        var playerLevelUpList = new List<PlayerLevelUpData>();
        foreach(PlayerController player in players)
        {
            var playerLevelUpController = player.levelUpController;
            var currentPlayerLevelUpList = playerLevelUpController.AddExperience(experiance);
            foreach(PlayerLevelUpData levelUpData in currentPlayerLevelUpList)
            {
                playerLevelUpList.Add(levelUpData);
            }
        }
        return playerLevelUpList;
    }

    
}

public struct PlayerLevelUpData
{
    public string playerName;
    public int level;
    public LevelReward levelReward;
}
