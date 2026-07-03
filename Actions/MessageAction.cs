using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagingAction : Actions
{
    //these attributes help us extend the typical instpector fields in the editor
    //this will turn the string field into a textarea in the editor
    [Multiline(5)]
    [SerializeField]
    List<string> messages;
    [SerializeField]
    string yestText, noText;
    [SerializeField]
    bool enableDialogue;
    [SerializeField]
    List<Actions> yesActions, noActions, endMessage;

    public override void Act()
    {
        Debug.Log("Trying to open dialogue");
        DialogueSystem.instance.showMessages(messages, enableDialogue, endMessage, yesActions, noActions, yestText, noText);
    }
}