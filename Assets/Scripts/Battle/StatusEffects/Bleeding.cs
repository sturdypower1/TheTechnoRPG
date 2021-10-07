using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding : MonoBehaviour
{
    Battler battler;
    public int level = 1;
    public float timeFromLastDamageTick;
    // Start is called before the first frame update
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
            timeFromLastDamageTick = 0;
        }
    }
    private void RemoveBleeding_OnBattleEnd(OnBattleEndEventArgs e)
    {
        RemoveBleeding();
    }
    void RemoveBleeding()
    {
        Destroy(this);
    }
}
