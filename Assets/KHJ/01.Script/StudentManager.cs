using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct StudentInfo
{
    //�̸�, ����, Ű, ������
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
    //����ü�� valueŸ��, Ŭ������ referenceŸ��
    public StudentInfo[] info = new StudentInfo[2];

    void Savedata()
    {
        //info�� �����͵��� �ӽ÷� data�� �����ص�
        ArrayData data = new ArrayData();
        data.users = info;
        //data���� Json�� ����
        string jsonData = JsonUtility.ToJson(data, true);
        //Json�����͸� PlayerPrefs�� ����
        PlayerPrefs.SetString("user_data", jsonData);
    }

    void LoadData()
    {
        //PlayerPrefs���κ��� ���� �ӽ÷� �޾ƿ�
        string user_data = PlayerPrefs.GetString("user_data");
        //Json���κ��� �迭 ��ü�� �޾ƿ��� ���� ArrayData�������� �ҷ���
        ArrayData data = JsonUtility.FromJson<ArrayData>(user_data);
        //info�� ����
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
