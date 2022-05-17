using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicBattler : Battler
{
    [SerializeField]private bool destroyAfterBattle = true;
    public override void ReturnToOverworld()
    {
        base.ReturnToOverworld();
        if (destroyAfterBattle)
        {
            Destroy(gameObject);
        }
        
    }
}
