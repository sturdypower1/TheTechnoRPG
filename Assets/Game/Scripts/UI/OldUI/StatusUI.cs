using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
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
            levelText.text = level.ToString();
            EnableUI();
        }
    }
}
