using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheck : MonoBehaviour
{
    public Movement movement;
    public Direction direction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null && !InkManager.instance.isCurrentlyDisplaying)
        {
            UIManager.instance.EnableInteractive();
            if (UIManager.instance.isInteractivePressed)
            {
                collision.gameObject.GetComponent<Interactable>().Interact();
            }
        }
        
   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null &&!InkManager.instance.isCurrentlyDisplaying)
        {
            UIManager.instance.EnableInteractive();
            if (UIManager.instance.isInteractivePressed)
            {
                collision.gameObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}
