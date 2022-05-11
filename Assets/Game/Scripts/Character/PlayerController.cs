using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    public LevelUpController levelUpController;
    public CharacterStats stats;
    public Battler battler;
    public Animator animator;
    public CharacterInventoryUI inventoryUI;
    
    private void Start()
    {
        Debug.Log("adding controller");
        inventoryUI.SetPlayerController(this);
    }
    public void EnableInventoryUI()
    {
        inventoryUI.EnableUI();
    }
    public void UpdateInventoryUI()
    {
        inventoryUI.UpdateUI();
    }
    public void DisableInventoryUI()
    {
        inventoryUI.DisableUI();
    }
}
