using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelUpController levelUpController;
    public CharacterStats stats;
    public Battler battler;
    public Animator animator;
    public CharacterInventoryUI inventoryUI;
    public void EnableInventoryUI()
    {
        inventoryUI.EnableUI();
    }
}
