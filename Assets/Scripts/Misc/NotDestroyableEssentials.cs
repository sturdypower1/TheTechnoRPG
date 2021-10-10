using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyableEssentials : MonoBehaviour
{
    public static NotDestroyableEssentials instance;
    private void Awake()
    {
        //singleton pattern
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
