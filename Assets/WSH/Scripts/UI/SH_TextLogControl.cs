using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_TextLogControl : MonoBehaviour
{
    [SerializeField]
    GameObject textTemplate;

    public List<GameObject> textItems = new List<GameObject>();
    public int maxLog = 10;

    public static SH_TextLogControl Instance => ins;
    static SH_TextLogControl ins;

    private void Awake()
    {
        if (ins == null)
            ins = this;
    }
    public void Clear()
    {
        for (int i = 0; i < textItems.Count; ++i)
        {
            var temp = textItems[0];
            Destroy(temp);
            textItems.RemoveAt(0);
        }
    }

    public void LogText(string text, Color color)
    {
        if (textItems.Count == maxLog)
        {
            GameObject temp = textItems[0];
            Destroy(temp);
            textItems.RemoveAt(0);
        }

        GameObject obj = Instantiate(textTemplate);
        obj.SetActive(true);
        obj.GetComponent<SH_TextLogItem>().SetText(text, color);
        obj.transform.SetParent(transform, false);

        textItems.Add(obj);
    }
}
