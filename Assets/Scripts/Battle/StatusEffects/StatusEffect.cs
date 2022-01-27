using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{


    public abstract void ApplyStatus(Battler battler);

    public virtual void RemoveStatus()
    {
        Destroy(this);
    }
}
