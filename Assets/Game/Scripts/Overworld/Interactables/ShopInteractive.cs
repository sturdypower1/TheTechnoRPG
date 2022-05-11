using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractive : Interactable
{
    private ShopController shopController;
    public override void Interact()
    {
        shopController.Activate();
        if (IsSingleUse)
        {
            IsEnabled = false;
        }
    }

    private void Awake()
    {
        shopController = GetComponent<ShopController>();
    }
}
