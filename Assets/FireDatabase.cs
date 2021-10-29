using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System;
using UnityEngine.UI;

[System.Serializable]
public struct ItemInfo
{
    public int idx;
    public Vector3 postion;
    public Vector3 angle;
    public Vector3 scale;
}

[System.Serializable]
public class UserInfo
{
    public string name;
    public int age;
    public int height;
    public string email;
    public string fbId;
    public List<ItemInfo> items = new List<ItemInfo>();
}

public class FireDatabase : MonoBehaviour
{
    public static FireDatabase instance;

    FirebaseDatabase database;

    public UserInfo myInfo = new UserInfo();

    public Action onComplete;
    public Action<string> onFail;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        database = FirebaseDatabase.DefaultInstance;

        //내정보 임시 입력
        //myInfo.name = "김현진";
        //myInfo.age = 25;
        //myInfo.height = 180;

    }

    public void SetItemInfo(List<GameObject> items)
    {
        myInfo.items.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            ItemInfo info = new ItemInfo();
            //info.idx = items[i].GetComponent<Item>().idx;
            info.postion = items[i].transform.position;
            info.angle = items[i].transform.eulerAngles;
            info.scale = items[i].transform.localScale;
            myInfo.items.Add(info);
        }
    }

    public void SaveUserInfo()
    {
        StartCoroutine(ISaveUserInfo());
    }

    IEnumerator ISaveUserInfo()
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        var task = database.GetReference(path).SetRawJsonValueAsync(JsonUtility.ToJson(myInfo));

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            print("유저 정보 저장 성공");
            if (onComplete != null) onComplete();
        }
        else
        {
            print("유저 정보 저장 실패 : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }
    }


    public void LoadUserInfo()
    {
        StartCoroutine(ILoadUserInfo());
    }


    IEnumerator ILoadUserInfo()
    {
        string path = "USER_INFO/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var task = database.GetReference(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception == null)
        {
            myInfo = JsonUtility.FromJson<UserInfo>(task.Result.GetRawJsonValue());
            print(myInfo.name);
            print(myInfo.age);
            print(myInfo.height);

            print("유저 정보 가져오기 성공");
            if (onComplete != null) onComplete();
        }
        else
        {
            print("유저 정보 가져오기 실패 : " + task.Exception);
            if (onFail != null) onFail(task.Exception.Message);
        }
    }
}
