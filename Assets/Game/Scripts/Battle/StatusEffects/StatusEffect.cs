using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public event LevelChangeEventHandler OnLevelChange;
    protected int level = 1;
    public abstract void TryApplyNextLevel();
    public abstract void ApplyStatus(Battler battler);

    public virtual void RemoveStatus()
    {
        Destroy(this);
    }
    
    protected void AddLevel()
    {
        level++;
        OnLevelChange?.Invoke(this, level);
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
    public static StatusEffectTypes StatusToEnum(StatusEffect statusEffect)
    {
        switch (statusEffect)
        {
            case Bleeding:
                return StatusEffectTypes.Bleeding;
        }

        // only should be triggered if the status effect is none
        return StatusEffectTypes.None;
    }
}

public enum StatusEffectTypes
{
    None,
    Bleeding
}

public delegate void LevelChangeEventHandler(StatusEffect statusEffect, int newLevel);
