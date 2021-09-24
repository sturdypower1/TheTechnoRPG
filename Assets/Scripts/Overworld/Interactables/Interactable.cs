using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsEnabled;
    public void Interact() {
        Debug.Log("interacted");
    }
    
}
