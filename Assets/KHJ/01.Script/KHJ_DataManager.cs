using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PetInfo
{
    public bool isGet;  //획득한 펫인지
    public int level;
    public float[] stat; //공격력, Hp, 치명타, 공격속도
    public float currH;
    public float currImacy;
}
[System.Serializable]
public struct PetArrayData
{
    public PetInfo[] pets;
}

public struct SceneInfo
{
    public int gold;
    public int dia;
    public int ticket;
    public List<bool> lootbox;
    public List<Prize> prize;
    public Pet nowPet;
}

public class KHJ_DataManager : MonoBehaviour
{
    KHJ_SceneMngr Mngr;
    public PetInfo[] info = new PetInfo[3];
    public SceneInfo sceneInfo;

    public static KHJ_DataManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        Mngr = KHJ_SceneMngr.instance;
    }
    void SaveData()
    {
        //펫 데이터 불러오기
        CatManager[] pet = new CatManager[3];
        pet[0] = Mngr.pet_cat.GetComponent<CatManager>();
        pet[1] = Mngr.pet_bear.GetComponent<CatManager>();
        pet[2] = Mngr.pet_dove.GetComponent<CatManager>();

        for (int i = 0; i < info.Length; i++)
        {
            info[i].isGet = Mngr.petButtons[i].activeSelf;
            info[i].level = pet[i].Level;
            info[i].stat = pet[i].stat;
            info[i].currH = pet[i].currH;
            info[i].currImacy = pet[i].currImacy;
        }

        //JSON
        PetArrayData data = new PetArrayData();
        data.pets = info;
        string jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("pet_data", jsonData);


        //씬 데이터 불러오기
        sceneInfo.gold = Mngr.gold;
        sceneInfo.dia = Mngr.dia;
        sceneInfo.ticket = Mngr.Ticket;
        sceneInfo.nowPet = Mngr.nowPet;
        for(int i = 0; i < LootBoxMngr.instance.lootBoxesOrigin.Count; i++)
        {
            sceneInfo.lootbox[i] = LootBoxMngr.instance.lootBoxesOrigin[i].Finish;
            sceneInfo.prize[i] = LootBoxMngr.instance.lootBoxesOrigin[i].prize;
        }
        //JSON
        jsonData = JsonUtility.ToJson(sceneInfo, true);
        PlayerPrefs.SetString("scene_data", jsonData);
    }

    void LoadData()
    {
        //JSON
        string pet_data = PlayerPrefs.GetString("pet_data");
        PetArrayData data = JsonUtility.FromJson<PetArrayData>(pet_data);
        info = data.pets;

        //펫데이터
        CatManager[] pet = new CatManager[3];
        pet[0] = Mngr.pet_cat.GetComponent<CatManager>();
        pet[1] = Mngr.pet_bear.GetComponent<CatManager>();
        pet[2] = Mngr.pet_dove.GetComponent<CatManager>();

        for (int i = 0; i < info.Length; i++)
        {
            Mngr.petButtons[i].SetActive(info[i].isGet);
            pet[i].Level = info[i].level;
            pet[i].stat = info[i].stat;
            pet[i].currH = info[i].currH;
            pet[i].currImacy = info[i].currImacy;
        }

        //JSON
        string scene_data = PlayerPrefs.GetString("scene_data");
        SceneInfo data1 = JsonUtility.FromJson<SceneInfo>(scene_data);
        sceneInfo = data1;

        //씬데이터
        Mngr.gold = sceneInfo.gold;
        Mngr.dia = sceneInfo.dia;
        Mngr.Ticket = sceneInfo.ticket;
        Mngr.nowPet = sceneInfo.nowPet;
        for (int i = 0; i < LootBoxMngr.instance.lootBoxesOrigin.Count; i++)
        {
            LootBoxMngr.instance.lootBoxesOrigin[i].Finish = sceneInfo.lootbox[i];
            LootBoxMngr.instance.lootBoxesOrigin[i].OnDisappear = sceneInfo.lootbox[i];
            LootBoxMngr.instance.lootBoxesOrigin[i].prize = sceneInfo.prize[i];
        }
    }

    public void Save()
    {
        SaveData();
    }
    public void Load()
    {
        LoadData();
    }
}