using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicBattler : Battler
{
    public override void ReturnToOverworld()
    {
        base.ReturnToOverworld();
        Destroy(gameObject);
    }
}
