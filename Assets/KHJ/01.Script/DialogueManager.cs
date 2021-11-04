using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    
    public Queue<string> sentences;
    public GameObject dialogueBox;
    
    public TMP_Text dialogueText;
    
    void Start()
    {
        sentences = new Queue<string>();
        
    }
    public void StartDialogue(Dialogue dialogue, int n)
    {
        dialogueBox.SetActive(true);                //대화창 UI 나타내기
        sentences.Clear();
        foreach (string sentence in dialogue.sentences[n])
        {
            if (sentence != null)                   //배열이 비어있지않다면
            {
                sentences.Enqueue(sentence);        //큐에 한문장씩 넣는다
            }
        }
        DisplayNextSentence();                      //첫문장 출력
    }
    
    public string[] sentence;
    public void DisplayNextSentence()
    {
        //펫 컨디션에 맞는 대사 출력
        int a = Random.Range(0, 3);
        print(a);        
        for(int i = 0; i < sentence.Length; i++)
        {
            sentence[i] = sentences.Dequeue();      //큐에서 한 문장씩 빼기
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence[a]));     //UI에 나타내기
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;            //한글자씩 화면에 반영
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        dialogueBox.SetActive(false);
    }    
}
