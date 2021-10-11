using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ChestInteractive : Interactable
{
    public IObtainable obtainable;
    Animator animator;
    public override void Interact()
    {
        obtainable.Obtain();
        animator.Play("ChestOpen");
        if (IsSingleUse)
        {
            IsEnabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Save(int saveFileNumber)
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/chest";
    }
    void Load()
    {

    }
}

