using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PetInfo
{
    public bool isGet;  //È¹µæÇÑ ÆêÀÎÁö
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
            instance = this;
        Mngr = KHJ_SceneMngr.instance;

        DontDestroyOnLoad(gameObject);
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

    public void SaveSceneData()
    {
        //¾À µ¥ÀÌÅÍ ºÒ·¯¿À±â
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

        //¾Àµ¥ÀÌÅÍ
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

        //Æê µ¥ÀÌÅÍ ºÒ·¯¿À±â
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

        //Æêµ¥ÀÌÅÍ
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