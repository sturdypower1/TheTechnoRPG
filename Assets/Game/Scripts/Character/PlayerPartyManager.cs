using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    public static PlayerPartyManager instance;
    public List<GameObject> players= new List<GameObject>();
    GameObject Leader;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// makes all of the player healths equal the max health they can have
    /// </summary>
    public void HealPlayers()
    {
        foreach(GameObject player in players)
        {
            CharacterStats characterStats = player.GetComponent<CharacterStats>();

            characterStats.stats.health = characterStats.stats.maxHealth;
        }
    }
    public void AddPlayer(string playerName)
    {
        GameObject currentPlayer = null;

        if(playerName == "Technoblade")
        {
            Leader = Technoblade.instance.gameObject;
            players.Insert(0, Technoblade.instance.gameObject);
            currentPlayer = Technoblade.instance.gameObject;
        }
        if(playerName == "Steve" && STEVE.instance != null)
        {
            players.Insert((players.Count == 0 ? 0 : 1) , STEVE.instance.gameObject);
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
            if (players.Contains(Technoblade.instance.gameObject))
            {
                players.Remove(Technoblade.instance.gameObject);
            }
        }
        if(playerName == "Steve" && STEVE.instance != null)
        {
            if (players.Contains(STEVE.instance.gameObject))
            {
                players.Remove(STEVE.instance.gameObject);
                
                
            }
        }
    }

    public bool HasPlayer(GameObject player)
    {
        return players.Contains(player);
    }
    /// <summary>
    /// returns all the levelups that occur
    /// </summary>
    /// <param name="experiance"></param>
    public List<PlayerLevelUpData> AddExperiance(int experiance)
    {
        var playerLevelUpList = new List<PlayerLevelUpData>();
        foreach(GameObject player in players)
        {
            var playerLevelUpController = player.GetComponent<LevelUpController>();
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
