using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dialogue
{
    public string name; //npc이름
    public string[][] sentences = new string[4][]; //대화 내용을 담을 2차원 배열

    public void SetDialogue(TextAsset dialogue_data_text)
    {
        string dialogue_text = dialogue_data_text.text;         //불러온 텍스트 파일의 내용 전체
        string[] lines = dialogue_text.Split('\n');             //각 행 단위로 나눠서 저장
        string[] content = new string[7];                       //대화 종류별로 나눔
        int n = -2; //대화 종류를 나누기 위한 변수
        int m = 0;  //각 대화에 포함된 문장의 순서
        foreach (var line in lines)
        {
            if (line == "")  //행이 빈 줄이면
            {
                continue;   //반복문의 처음으로 점프
            }
            if (line.StartsWith("#"))                           //워드의 시작문자가 #이면
            {
                n += 1;                                         //대화의 종류를 바꿈
                m = 0;                                          //문장의 순서도 초기화
                if (n >= 0)                                     //'#이름' 이 부분을 건너뛰기 위한 조건문
                {
                    sentences[n] = content;                     //각각 대응하는 변수에 넣음
                    content = new string[7];                    
                }
                continue;                                       //루프의 시작으로 점프
            }
            content[m] = line;
            m += 1;
            
        }


    }








}
