using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool IsEnabled = true;
    public abstract void Interact();
    
}
