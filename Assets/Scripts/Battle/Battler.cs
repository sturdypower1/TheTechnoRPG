using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// component that'll be used for all the battlers in a battle
/// </summary>
[RequireComponent(typeof(CharacterStats))]
public class Battler : MonoBehaviour
{
    /// <summary>
    /// when true, disables all battle ai and other actions
    /// </summary>
    public bool isPaused;

    /// <summary>
    /// how much long it has left before it can do another attack
    /// </summary>
    public float useTime;

    public float maxUseTime;

    public HeadsUpUI headsUpUI;
    [HideInInspector]

    public Vector3 oldPosition;
    [HideInInspector]

    public bool isDown;

    public AudioSource damagedSound;

    public CharacterStats characterStats;

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    public virtual void TakeDamage(Damage damage)
    {
        characterStats.stats.health -= damage.damageAmount;

        Label label = new Label();
        label.text = damage.damageAmount.ToString();
        switch (damage.damageType)
        {
            case DamageType.Bleeding:
                label.AddToClassList("message_red");
                break;
            case DamageType.Physical:
                if (damagedSound != null)
                {
                    damagedSound.Play();
                }
                label.AddToClassList("message_white");
                break;
        }
        headsUpUI.ui.Q<VisualElement>("messages").Add(label);

        float random = Random.value * 360;
        Vector2 messageDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random));

        Message newMessage = new Message { timePassed = 0, label = label, direction = messageDirection};
        headsUpUI.messages.Add(newMessage);

        // character should be down
        if(characterStats.stats.health <= 0)
        {
            isDown = true;

            foreach(Message message in headsUpUI.messages)
            {
                headsUpUI.ui.Q<VisualElement>("messages").Remove(message.label);
            }

            headsUpUI.messages.Clear();
        }
        

    }

}
