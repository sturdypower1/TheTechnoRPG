using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private StatusUI bleedingUI;

    public void SetStatus(int level, StatusEffectTypes statusType)
    {
        var statusUI = GetStatusUIFromType(statusType);
        if (statusUI == null) return;

        statusUI.SetLevel(level);
    }
    public void ActivateStatus(StatusEffectTypes statusType)
    {
        var statusUI = GetStatusUIFromType(statusType);
        statusUI.EnableUI();
    }

    public void DeactivateStatus(StatusEffectTypes statusType)
    {
        var statusUI = GetStatusUIFromType(statusType);
        statusUI.DisableUI();
    }

    private StatusUI GetStatusUIFromType(StatusEffectTypes statusType)
    {
        switch (statusType) 
        {
            case StatusEffectTypes.Bleeding:
                return bleedingUI;
        }
        return null;
    }
}
