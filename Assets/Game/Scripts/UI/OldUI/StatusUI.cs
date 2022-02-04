using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    public void EnableUI()
    {
        gameObject.SetActive(true);
    }
    public void DisableUI()
    {
        gameObject.SetActive(false);
    }

    public void SetLevel(int level)
    {
        if(level == 0)
        {
            DisableUI();
        }
        else
        {
            EnableUI();
        }
    }
}
