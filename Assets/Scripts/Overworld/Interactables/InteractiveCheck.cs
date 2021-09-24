using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCheck : MonoBehaviour
{
    public Movement movement;
    public Direction direction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null)
        {
            UIManager.instance.EnableInteractive();
            Debug.Log("ello");
            if (UIManager.instance.isInteractivePressed)
            {
                Debug.Log("interact");
            }
        }
        
   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (direction == movement.Direction && collision.gameObject.GetComponent<Interactable>() != null)
        {
            UIManager.instance.EnableInteractive();
            Debug.Log("ello");
            if (UIManager.instance.isInteractivePressed)
            {
                Debug.Log("interact");
            }
        }
    }
}
