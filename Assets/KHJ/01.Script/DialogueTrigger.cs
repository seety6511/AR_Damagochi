using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isFirst;
    public bool isFriend;
    public bool isLose;


    public void TriggerDialogue(int n)
    {

        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue, n);
    }
    

}
