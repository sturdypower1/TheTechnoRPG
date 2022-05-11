using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : StatusEffect
{
    Battler battler;
    public float timeFromLastDamageTick;
    public override void TryApplyNextLevel()
    {
        AddBleedingLevel();
    }
    public override void ApplyStatus(Battler battler)
    {
        var bleed = battler.GetComponent<Bleeding>();
        if(bleed == null)
        {
            bleed = battler.gameObject.AddComponent<Bleeding>();
            bleed = this;
            bleed.battler = battler;
        }
        else
        {
            bleed.AddBleedingLevel();
            Destroy(this);
        }
    }
    public void AddBleedingLevel()
    {
        AddLevel();
    }
    void Start()
    {
        BattleManager.instance.OnBattleEnd += RemoveBleeding_OnBattleEnd;
        battler = GetComponent<Battler>();
    }

    // Update is called once per frame
    void Update()
    {
        timeFromLastDamageTick += Time.unscaledDeltaTime;
        //deal damage every other second
        if (timeFromLastDamageTick >= 2)
        {
            battler.TakeDamage(new Damage { damageAmount = level, damageType = DamageType.Bleeding });
            Technoblade.instance.AddBlood(level);
            timeFromLastDamageTick = 0;
        }
        
    }
    private void RemoveBleeding_OnBattleEnd(OnBattleEndEventArgs e)
    {
        RemoveStatus();
    }

}
