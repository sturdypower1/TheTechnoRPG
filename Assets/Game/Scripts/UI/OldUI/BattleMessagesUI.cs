using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessagesUI : MonoBehaviour
{
    public GameObject physicalLabel;
    public GameObject bleedingLabel;

    private RectTransform _spawnArea;

    private void Awake()
    {
        _spawnArea = GetComponent<RectTransform>();
    }
    public void AddMessage(string text, MessageType messageType)
    {
        var message = GetMessageFromType(messageType);
        message.Initialize(text, _spawnArea);
    }

    private Message GetMessageFromType(MessageType messageType)
    {
        Message message = null;
        GameObject messageObject = null;
        switch (messageType)
        {
            case MessageType.PhysicalDamage:
                messageObject = Instantiate(physicalLabel, transform);
                break;
            case MessageType.BleedingDamage:
                messageObject = Instantiate(bleedingLabel, transform);
                break;
        }
        message = messageObject.GetComponent<Message>();
        return message;
    }

   
}
