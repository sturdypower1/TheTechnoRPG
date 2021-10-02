using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// component that'll be used for all the battlers in a battle
/// </summary>
[RequireComponent(typeof(CharacterStats))]
public class Battler : MonoBehaviour
{
    /// <summary>
    /// when true, disables all battle ai and other actions
    /// </summary>
    public bool isPaused;

    /// <summary>
    /// how much long it has left before it can do another attack
    /// </summary>
    public float recoveryTime;

    public HeadsUpUI headsUpUI;

    public Vector3 oldPosition;

}
