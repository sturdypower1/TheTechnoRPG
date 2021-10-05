using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// is used with 
/// </summary>
public class EnemySelectorUI
{
    public VisualElement ui;
    public SpriteRenderer sprite;

    public Battler enemy;

    /// <summary>
    /// make the target glow
    /// </summary>
    public void SelectUI()
    {

        float factor = Mathf.Pow(2, 6);
        MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
        sprite.GetPropertyBlock(myMatBlock);
        myMatBlock.SetInt("IsSelected", 1);
        sprite.SetPropertyBlock(myMatBlock);
    }
    /// <summary>
    /// stop the target from glowing
    /// </summary>
    public void UnSelectUI()
    {
        float factor = Mathf.Pow(2, 1);
        MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
        sprite.GetPropertyBlock(myMatBlock);
        myMatBlock.SetInt("IsSelected", 0);
        sprite.SetPropertyBlock(myMatBlock);
    }

    public void Update()
    {
        if (BattleManager.instance.isInBattle)
        {
            if (enemy.isDown)
            {
                ui.SetEnabled(false);
            }
        }
        

    }
}
