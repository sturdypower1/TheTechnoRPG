using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{


    public abstract void ApplyStatus(Battler battler);

    public virtual void RemoveStatus()
    {
        Destroy(this);
    }

    public static Type EnumToStatus(StatusEffectTypes statusType)
    {
        switch (statusType)
        {
            case StatusEffectTypes.Bleeding:
                return typeof(Bleeding);
        }

        // only should be triggered if the status effect is none
        return null;
    }
}

public enum StatusEffectTypes
{
    None,
    Bleeding
}
