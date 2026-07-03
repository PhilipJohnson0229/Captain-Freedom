using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    //this property can only be set within this class
    //but it can be read publically
    public static DialogueSystem instance { get; private set; }

    [SerializeField]
    TMPro.TextMeshProUGUI messageText, yesText, noText;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Button yesButton, noButton;

    private List<string> currentMessages = new List<string>();
    private int messageId = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        panel.SetActive(false);
    }

    public void showMessages(List<string> messages, bool dialogue, List<Actions> endMessage, List<Actions> yesActions = null, List<Actions> noActions = null, string yes = "", string no = "")
    {
        messageId = 0;

        //this will turn off both buttons
        yesButton.transform.parent.gameObject.SetActive(false);

        currentMessages = messages;

        panel.SetActive(true);

        if (dialogue)
        {
            messageId = 0;
            HandleYesNoLiseteners(yesActions, noActions, yes, no);
        }

        StartCoroutine(ShowMultipleMessages(dialogue, endMessage));
    }

    private void HandleYesNoLiseteners(List<Actions> yesActions, List<Actions> noActions, string yes, string no)
    {
        yesText.text = yes;

        yesButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(delegate
        {
            //just like an inline lambda function in javascript/react
            panel.SetActive(false);

            if (yesActions != null)
            {
                AssignActionButtons(yesActions);
            }
        });

        noText.text = no;

        if (noText.text.Trim() != "") 
        {
            noButton.gameObject.SetActive(true);

            noButton.onClick.RemoveAllListeners();

            noButton.onClick.AddListener(delegate
            {
                //just like an inline lambda function in javascript/react
                panel.SetActive(false);

                if (noActions != null)
                {
                    AssignActionButtons(noActions);
                }
            });
        }
        else
        {
            noButton.gameObject.SetActive(false);
        }
        
    }

    IEnumerator ShowMultipleMessages(bool useDialogue, List<Actions> endMessage)
    {
        messageText.text = currentMessages[messageId];

        while (messageId < currentMessages.Count)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                messageId++;

                if (messageId < currentMessages.Count)
                {
                    messageText.text = currentMessages[messageId];
                }

                if (useDialogue && messageId == currentMessages.Count - 1)
                {
                    yesButton.transform.parent.gameObject.SetActive(true);
                }
            }

            yield return null;
        }

        if (!useDialogue)
        {
            HideDialogue();
        }

        messageId = 0;
        Debug.Log("End of message array");
        Extensions.RunActions(endMessage.ToArray());
    }

    void AssignActionButtons(List<Actions> actions)
    {
        List<Actions> localActions = actions;

        for (int i = 0; i < localActions.Count; i++)
        {
            localActions[i].Act();
        }
    }

    public void HideDialogue()
    {
        Debug.Log("Hide dialogue was called");
        panel.SetActive(false);
    }
}