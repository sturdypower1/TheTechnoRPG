using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheck : MonoBehaviour
{
    public Movement movement;
    public Direction direction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null && collision.gameObject.GetComponent<Interactable>().IsEnabled && !InkManager.instance.isCurrentlyDisplaying)
        {
            
            if (MainGameManager.instance.IsInteractButtonPressed())
            {
                collision.gameObject.GetComponent<Interactable>().Interact();
            }
            MainGameManager.instance.TryEnableInteractable();
        }
        
   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null && collision.gameObject.GetComponent<Interactable>().IsEnabled && !InkManager.instance.isCurrentlyDisplaying)
        {
            MainGameManager.instance.TryEnableInteractable();
            if (MainGameManager.instance.IsInteractButtonPressed())
            {
                collision.gameObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}
