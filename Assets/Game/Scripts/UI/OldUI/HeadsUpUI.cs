using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// used to display the status of the character
/// </summary>
public class HeadsUpUI : MonoBehaviour
{
    [SerializeField] private Battler battler;
    [SerializeField] private BattleMessagesUI messageUI;
    [SerializeField] private StatusBar statusBar;
    public void SetStatus(int level, StatusEffectTypes statusType)
    {
        statusBar.SetStatus(level, statusType);
    }
    public void AddMessage(string text, MessageType messageType)
    {
        messageUI?.AddMessage(text, messageType);
    }
    public void EnableUI()
    {
        gameObject.SetActive(true);
    }
    public void DisableUI()
    {
        gameObject.SetActive(false);
    }

   
}
public enum MessageType
{
    PhysicalDamage,
    BleedingDamage,
    Skill,
    Healing
}
