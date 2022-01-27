using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class STEVEBattler : PlayerBattler
{
    private void Update()
    {
        Debug.Log("need to set up steve battle ui");
        
    }
    public override void ReEnableMenu()
    {
        animator.SetBool("Defending", false);
        isDefending = false;
        base.ReEnableMenu();


        AudioManager.playSound("menuavailable");
    }



}
