using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    public static PlayerPartyManager instance;
    List<GameObject> players= new List<GameObject>();
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
            currentPlayer = Technoblade.instance.gameObject;
        }
        if(playerName == "Steve")
        {
            currentPlayer = STEVE.instance.gameObject;
        }
        // adds player if there is one and if it does already have that player
        if(currentPlayer != null && !players.Contains(currentPlayer))
        {
            players.Add(currentPlayer);
            if(players.Count == 0)
            {
                Leader = currentPlayer;
            }
        }
    }

    public void RemovePlayer(string playerName)
    {
        if(playerName == "Technoblade")
        {
            if (players.Contains(Technoblade.instance.gameObject))
            {
                players.Remove(Technoblade.instance.gameObject);
            }
        }
        if(playerName == "Steve")
        {
            if (players.Contains(STEVE.instance.gameObject))
            {
                players.Remove(STEVE.instance.gameObject);
            }
        }
    }
}
