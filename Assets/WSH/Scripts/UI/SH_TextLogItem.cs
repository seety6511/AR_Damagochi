using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_TextLogItem : MonoBehaviour
{
    [SerializeField]
    Text text;
    public void SetText(string text, Color color)
    {
        this.text.text = text;
        this.text.color = color;
    }
}
