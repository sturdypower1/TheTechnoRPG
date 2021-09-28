using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Technoblade : MonoBehaviour
{
    public static Technoblade instance;

    public CharacterStats stats;
    private void Awake() {
        if(instance == null || instance == this){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
}
