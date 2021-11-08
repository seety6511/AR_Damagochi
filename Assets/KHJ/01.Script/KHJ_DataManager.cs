using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PetInfo
{
    public bool isGet;  //ȹ���� ������
    public int level;
    public float hp;
    public float atk;
    public float speed;
    public float currH;
    public float currImacy;
}
[System.Serializable]
public struct PetArrayData
{
    public PetInfo[] pets;
}
[System.Serializable]
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
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Mngr = KHJ_SceneMngr.instance;

    }

    public void SaveBattleSceneData()
    {
        string jsonData = JsonUtility.ToJson(sceneInfo, true);
        PlayerPrefs.SetString("scene_data", jsonData);

        PetArrayData data = new PetArrayData();
        data.pets = info;
        string jsonData2 = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("pet_data", jsonData2);
    }

    //public void ResetData()
    //{
    //    sceneInfo = new SceneInfo();
    //    sceneInfo.gold = 200;
    //    sceneInfo.dia = 200;
    //    sceneInfo.ticket = 10;
    //    sceneInfo.nowPet = Pet.cat;
    //    //JSON
    //    string jsonData = JsonUtility.ToJson(sceneInfo, true);
    //    PlayerPrefs.SetString("scene_data", jsonData);
    //    //SaveSceneData();

    //    info = new PetInfo[3];
    //    info[0].isGet = true;
    //    info[1].isGet = false;
    //    info[2].isGet = false;
    //    for (int i = 0; i < info.Length; i++)
    //    {
    //        info[i].level = 0;
    //        info[i].currH = 100;
    //        info[i].currImacy = 100;
    //    }
        
    //    info[0].atk = 5;
    //    info[0].hp = 5;
    //    info[0].speed = 5;

    //    info[1].atk = 3;
    //    info[1]/ = 7;
    //    info[1].stat[2] = 5;
    //    info[1].stat[3] = 4;

    //    info[2].stat[0] = 7;
    //    info[2].stat[1] = 3;
    //    info[2].stat[2] = 5;
    //    info[2].stat[3] = 6;

    //    //JSON
    //    PetArrayData data = new PetArrayData();
    //    data.pets = info;
    //    jsonData = JsonUtility.ToJson(data, true);
    //    PlayerPrefs.SetString("pet_data", jsonData);



    //    SavePetData();
    //}

    public void SaveSceneData()
    {
        //�� ������ �ҷ�����
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
        string jsonData = JsonUtility.ToJson(sceneInfo, true);
        PlayerPrefs.SetString("scene_data", jsonData);
    }

    public void LoadSceneData()
    {
        if (!PlayerPrefs.HasKey("scene_data"))
        {
            SaveBattleSceneData();
        }

        //JSON
        string scene_data = PlayerPrefs.GetString("scene_data");
        SceneInfo data1 = JsonUtility.FromJson<SceneInfo>(scene_data);
        sceneInfo = data1;

        //��������
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
    public void SavePetData()
    {
        info = new PetInfo[3];

        //�� ������ �ҷ�����
        CatManager[] pet = new CatManager[3];
        pet[0] = Mngr.pet_cat.GetComponent<CatManager>();
        pet[1] = Mngr.pet_bear.GetComponent<CatManager>();
        pet[2] = Mngr.pet_dove.GetComponent<CatManager>();

        for (int i = 0; i < info.Length; i++)
        {
            info[i].isGet = Mngr.petButtons[i].activeSelf;
            info[i].level = pet[i].Level;
            info[i].hp = pet[i].hp;
            info[i].atk = pet[i].atk;
            info[i].speed = pet[i].speed;
            info[i].currH = pet[i].currH;
            info[i].currImacy = pet[i].currImacy;
        }

        //JSON
        PetArrayData data = new PetArrayData();
        data.pets = info;
        string jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("pet_data", jsonData);
    }

    public void LoadPetData()
    {
        if (!PlayerPrefs.HasKey("pet_data"))
        {
            SaveBattleSceneData();
        }

        Mngr = FindObjectOfType<KHJ_SceneMngr>();

        //JSON
        string pet_data = PlayerPrefs.GetString("pet_data");
        PetArrayData data = JsonUtility.FromJson<PetArrayData>(pet_data);
        info = data.pets;

        //�굥����
        CatManager[] pet = new CatManager[3];
        pet[0] = Mngr.pet_cat.GetComponent<CatManager>();
        pet[1] = Mngr.pet_bear.GetComponent<CatManager>();
        pet[2] = Mngr.pet_dove.GetComponent<CatManager>();

        for (int i = 0; i < info.Length; i++)
        {
            Mngr.petButtons[i].SetActive(info[i].isGet);
            pet[i].Level = info[i].level;
            pet[i].hp = info[i].hp;
            pet[i].atk = info[i].atk;
            pet[i].speed = info[i].speed;
            pet[i].currH = info[i].currH;
            pet[i].currImacy = info[i].currImacy;
        }
    }
}