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

    public List<Message> messages;

    private void Awake()
    {
        overHeadUITemplate = Resources.Load<VisualTreeAsset>("UIDocuments/OverHeadBattleStats");
    }

    // Update is called once per frame
    void Update()
    {
        if(ui == null)
        {
            ui = overHeadUITemplate.CloneTree();
            ui.visible = false;
            UIManager.instance.root.Add(ui);
        }
        // update the messages
    }
}
public struct Message
{
    public Label label;
    public float timePassed;
    public Vector2 direction;
}
