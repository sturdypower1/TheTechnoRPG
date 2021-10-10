using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// used to display the status of the character
/// </summary>
public class HeadsUpUI : MonoBehaviour
{
    public static VisualTreeAsset overHeadUITemplate;

    public TemplateContainer ui;

    public CharacterStats characaterStats;

    public List<Message> messages = new List<Message>();

    public Battler battler;

    private void Awake()
    {
        overHeadUITemplate = Resources.Load<VisualTreeAsset>("UIDocuments/OverHeadBattleStats");
    }

    // Update is called once per frame
    void Update()
    {
        if (ui == null)
        {
            ui = overHeadUITemplate.CloneTree();
            ui.visible = false;
            UIManager.instance.root.Add(ui);
        }
        // update the messages

        if (BattleManager.instance.isInBattle && !battler.isDown)
        {
            ui.visible = true;


            if (battler.gameObject.GetComponent<Bleeding>() != null)
            {
                ui.Q<VisualElement>("bleeding_icon").visible = true;
            }
            else
            {
                ui.Q<VisualElement>("bleeding_icon").visible = false;
            }

            VisualElement healthbarBase = ui.Q<VisualElement>("healthbar_base");
            //healthbarBase.visible = true;
            VisualElement healthbar = healthbarBase.Q<VisualElement>("healthbar");
            healthbar.style.width = healthbarBase.contentRect.width * ((float)characaterStats.stats.health / characaterStats.stats.maxHealth);

            // loop through all of the messages
            for (int i = 0; i < messages.Count; i++)
            {
                Message message = messages[i];
                message.timePassed += Time.unscaledDeltaTime;
                messages[i] = message;
                if (message.timePassed > 2)
                {
                    ui.Q<VisualElement>("messages").Remove(message.label);
                    messages.RemoveAt(i);
                    i--;
                }
                else if (message.timePassed > 1)
                {
                    messages[i].label.style.opacity = Mathf.Lerp(1, 0, message.timePassed - 1);
                }
                else
                {
                    Vector3 newPosition = messages[i].label.transform.position;
                    newPosition.y += 20 * Time.unscaledDeltaTime * messages[i].direction.y;
                    newPosition.x += 20 * Time.unscaledDeltaTime * messages[i].direction.x;
                    messages[i].label.transform.position = newPosition;
                }
            }
        }
        else
        {
            ui.visible = false;
            ui.Q<VisualElement>("bleeding_icon").visible = false;
        }
    }
}
public struct Message
{
    public Label label;
    public float timePassed;
    public Vector2 direction;
}
