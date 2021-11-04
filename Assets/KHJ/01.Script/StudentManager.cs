using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct StudentInfo
{
    //이름, 나이, 키, 몸무게
    public string name;
    public int age;
    public  float height;
    public float weight;
}
[System.Serializable]
public struct ArrayData
{
    public StudentInfo[] users;
}

public class StudentManager : MonoBehaviour
{
    //구조체는 value타입, 클래스는 reference타입
    public StudentInfo[] info = new StudentInfo[2];

    void Savedata()
    {
        //info의 데이터들을 임시로 data에 저장해둠
        ArrayData data = new ArrayData();
        data.users = info;
        //data값을 Json에 저장
        string jsonData = JsonUtility.ToJson(data, true);
        //Json데이터를 PlayerPrefs에 저장
        PlayerPrefs.SetString("user_data", jsonData);
    }

    void LoadData()
    {
        //PlayerPrefs으로부터 값을 임시로 받아옴
        string user_data = PlayerPrefs.GetString("user_data");
        //Json으로부터 배열 전체를 받아오기 위해 ArrayData형식으로 불러옴
        ArrayData data = JsonUtility.FromJson<ArrayData>(user_data);
        //info에 저장
        info = data.users;
    }
    public void Save()
    {
        Savedata();
    }

    public void Load()
    {
        LoadData();
    }
}
