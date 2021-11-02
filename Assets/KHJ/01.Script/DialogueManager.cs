using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    /*
    Player player;
    public Animator animator;
    public Animator animator_select;
    public Queue<string> sentences;
    public GameObject dialogueBox;
    public GameObject selectBox;
    public GameObject continueButton;
    public GameObject alertBox;
    public GameObject FailBox;
    public GameObject rank;

    public GameObject CameraOne;
    public GameObject CameraTwo;
    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;

    public Text nameText;
    public Text dialogueText;
    private int dialogueType = 0;    //대화 종류
    public GameObject[] EnemyRank = new GameObject[6];

    public enum SelectNum           //선택지 종류
    {
        none=-1,talk,fight,quit
    };
    public SelectNum nextSentence = SelectNum.none;


    void Start()
    {
        sentences = new Queue<string>();
        player = GameObject.Find("Player").GetComponent<Player>();
        cameraOneAudioLis = CameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = CameraTwo.GetComponent<AudioListener>();
        cameraPositionChange(0);
    }
    public void StartDialogue(Dialogue dialogue, int n)
    {
        dialogueType = n;
        dialogueBox.SetActive(true);                //대화창 UI 나타내기
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        nameText.text = dialogue.name;
        foreach (string sentence in dialogue.sentences[n])
        {

            if (sentence != null)                   //배열이 비어있지않다면
            {
                sentences.Enqueue(sentence);        //큐에 한문장씩 넣는다
            }
        }
        DisplayNextSentence();                      //첫문장 출력
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)                    //대화가 끝나면
        {
            animator.SetBool("IsOpen", false);      //대화창 UI 사라지게하기
            dialogueBox.SetActive(false);
            player.isTalk = false;                  //대화 상태 해제
            player.target = null;                   //타겟 해제
            return;
        }
        if (dialogueType > 1 && sentences.Count == 6)          //기본대화, 친구대화일 경우 플레이어의 선택지를 기다려야 함
        {
            continueButton.SetActive(false);            
            selectBox.SetActive(true);
            animator_select.SetBool("IsOpen", true);
        }

        string sentence = sentences.Dequeue();      //큐에서 한 문장만 빼서 저장한 후
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));     //UI에 나타내기


        if(nextSentence != SelectNum.none)          //선택지에 해당하는 결과표시
        {
            switch (nextSentence)
            {
                case SelectNum.talk:
                    player.target.GetComponent<Enemy>().GetLikePoint();
                    sentences.Clear();                  //큐에 남은 나머지 문장들 삭제
                    nextSentence = SelectNum.none;      //다음 대화를 위해 다시 none으로 설정
                    if (player.friend == 5)
                        StartCoroutine(ClearGame());    //모든 고양이와 친구가 되면 게임 클리어
                    break;
                case SelectNum.fight:
                    sentences.Clear();
                    nextSentence = SelectNum.none;
                    //싸움 시작
                    if (!player.target.GetComponent<DialogueTrigger>().isLose)
                    {
                        if(CompareCharisma() && RankCheck())
                        {
                            continueButton.SetActive(false);
                            StartCoroutine(WaitForFight());
                            StartCoroutine(PlayFightScene());
                        }
                    }
                    break;
                case SelectNum.quit:
                    sentences.Clear();
                    nextSentence = SelectNum.none;
                    break;
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;            //한글자씩 화면에 반영
            yield return null;
        }
    }

    IEnumerator ClearGame()
    {
        yield return new WaitForSeconds(2);         //2초 후
        SceneManager.LoadScene(4);                  //클리어 씬으로
    }

    public void Select1()
    {
        nextSentence = SelectNum.talk;
        continueButton.SetActive(true);
        animator_select.SetBool("IsOpen", false);
        selectBox.SetActive(false);
    }
    public void Select2()
    {
        sentences.Dequeue();
        if (player.target.GetComponent<DialogueTrigger>().isLose)   //A
        {
            Debug.Log("서열 높아서 싸움불가능");
            //서열이 이미 높으면 싸움X
            sentences.Dequeue();    
            sentences.Dequeue();
        }
        else
        {                      
            //싸워서 이기기 전
            if (CompareCharisma() == true && RankCheck() )      //B
            {
                //바로 밑 랭크이고, 카리스마도 비교했을 때 싸움 가능
                Debug.Log("싸움가능");
                sentences.Dequeue();
            }
            else
            {
                //바로 밑 랭크가 아니거나 카리스마가 모자라서 싸움 불가능
                Debug.Log("랭크가 안맞거나 카리스마가 모자람");
            }            
        }

        nextSentence = SelectNum.fight;
        continueButton.SetActive(true);
        animator_select.SetBool("IsOpen", false);
        selectBox.SetActive(false);
    }
    public void Select3()
    {
        sentences.Dequeue();
        sentences.Dequeue();
        sentences.Dequeue();
        sentences.Dequeue();
        nextSentence = SelectNum.quit;
        continueButton.SetActive(true);
        animator_select.SetBool("IsOpen", false);
        selectBox.SetActive(false);
    }
    public bool RankCheck()
    {
        bool result;     //ture = 싸울 수 있음, 랭크 순서대로 싸움을 걸어야 함
        Debug.Log(player.stat.rank);
        Debug.Log(player.target.GetComponent<Enemy>().stat.rank + 1);
        if (player.stat.rank == player.target.GetComponent<Enemy>().stat.rank + 1)
        {            
            result = true;
        }
        else
            result = false;

        Debug.Log("RankCheck result = " + result);
        return result;
    }
    public void BecomeFriend()
    {
        
        player.friend++;
        alertBox.gameObject.GetComponentInChildren<Text>().text = player.target.GetComponent<DialogueTrigger>().dialogue.name + "님과 친구가 되었어요!";
        alertBox.SetActive(true);
                
        int a = player.target.GetComponent<Enemy>().stat.rank;
        EnemyRank[a].SetActive(true);

        
    }

    public bool CompareCharisma()
    {
        bool result = true;     //ture = 싸울 수 있음
        //플레이어의 카리스마가 타겟의 카리스마보다 2이상 적다면
        if (player.stat.charisma < (player.target.GetComponent<Enemy>().stat.charisma - 2))
            result = false;
        return result;
    }

    public bool CompareStrength()
    {
        bool result = true;     //ture = 플레이어가 승리
        //플레이어의 힘이 타겟의 힘보다 적다면 (같은 경우에도 승리 판정)
        if (player.stat.strength < player.target.GetComponent<Enemy>().stat.strength)
            result = false;
        return result;
    }

    IEnumerator WaitForFight()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("IsOpen", false);      //대화창 UI 사라지게하기
        dialogueBox.SetActive(false);
    }

    IEnumerator PlayFightScene()
    {
        yield return new WaitForSeconds(2f);
        player.isPlay = true;
        //카메라 변경
        cameraChangeCounter();
        yield return new WaitForSeconds(5f);
        player.isPlay = false;
        player.vEnd = player.transform.position;
        if (CompareStrength())  //승리
        {
            alertBox.gameObject.GetComponentInChildren<Text>().text = "랭크가 올랐어요!";
            alertBox.SetActive(true);
            player.target.GetComponent<DialogueTrigger>().isLose = true;
            rank.GetComponent<Rank>().RankUp();

        }
        else                    //패배
        {
            FailBox.SetActive(true);       
        
        }
        cameraChangeCounter();
        continueButton.SetActive(true);
        DisplayNextSentence();
        
    }
    void cameraChangeCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1
        if (camPosition == 0)
        {
            CameraOne.SetActive(true);
            cameraOneAudioLis.enabled = true;

            cameraTwoAudioLis.enabled = false;
            CameraTwo.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == 1)
        {
            CameraTwo.SetActive(true);
            cameraTwoAudioLis.enabled = true;

            cameraOneAudioLis.enabled = false;
            CameraOne.SetActive(false);
        }

    }
    */
}
