using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum Pet
{
    cat,
    bear,
    dove,
}
public class KHJ_SceneMngr : MonoBehaviour
{
    public static KHJ_SceneMngr instance;
    public CatManager pet;

    //펫 종류
    public Pet nowPet;
    public GameObject catEnvironmet;
    public GameObject bearEnvironment;
    public GameObject doveEnvironment;
    public GameObject pet_cat;
    public GameObject pet_bear;
    public GameObject pet_dove;

    public GameObject Panel;

    //재화
    public TMP_Text goldUI;
    public TMP_Text diaUI;
    public int gold = 100;
    public int dia = 100;
    public Text ticketNum;
    public int Ticket;

    //유대감, 배고픔 게이지
    public float currH = 0;
    float maxH = 100;
    public float currImacy;
    public Image IntimacyImg;
    public Image IntimacyBar;
    public Image HungryBar;
    public Sprite[] ImmoSprites;

    //공 던지기
    public bool isBall;
    public GameObject[] Ball;
    public GameObject shootPosition;
    public GameObject PlayUI;

    //밥먹기
    public bool isEat;
    public bool[] isFoodSet;
    public GameObject[] Food;
    public GameObject FoodUI;

    //AR
    public bool useAR;
    public GameObject MainCam;
    public GameObject ARCam;
    public GameObject AR_Text;

    //대화
    public DialogueTrigger[] triggers;
    public TextAsset[] dialogue_data;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        pet = pet_cat.GetComponent<CatManager>();
        currH = pet.currH;
        currImacy = pet.currImacy;
        SetDialogue();
    }
    void Update()
    {
        currH = pet.currH;
        currImacy = pet.currImacy;
        goldUI.text = gold.ToString();
        diaUI.text = dia.ToString();
        ticketNum.text = Ticket.ToString();
        HungryBar.fillAmount = currH / maxH;

        Hungry();
        ConditionSet();
        FoodSet();

        if (Input.GetMouseButtonDown(0))
        {
            //UI Blocking
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            //터치 상호작용
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if(hit.transform.gameObject.name == "FoodPlate" && !isFoodSet[(int)nowPet])
                {
                    //밥주기
                    FoodUI.SetActive(!FoodUI.activeSelf);
                }
                if (hit.transform.gameObject.name == "cat-ball" && !isBall)
                {
                    //공놀이
                    PlayUI.SetActive(!PlayUI.activeSelf);
                }
                if (hit.transform.gameObject.name == "Cat" || hit.transform.gameObject.name == "Bear" || hit.transform.gameObject.name == "Dove")
                {
                    if (isBall)
                        return;
                    pet.actionState = CatManager.ActionState.isTouching;
                    //대사 출력
                    PetTalk();
                }
                if(hit.transform.gameObject.tag == "floor")
                {
                    //터치이동
                    pet.Target = hit.point;
                    pet.actionState = CatManager.ActionState.isMoving;
                }
            }
        }
    }

    void PetTalk()
    {
        if(pet.hungryState == Damagochi.HungryState.Little)
        {
            triggers[(int)nowPet].TriggerDialogue(6);
            return;
        }
        else if(pet.hungryState== Damagochi.HungryState.Very)
        {
            triggers[(int)nowPet].TriggerDialogue(7);
            return;
        }
        switch (pet.condition)
        {
            case Damagochi.Condition.Happy:
                triggers[(int)nowPet].TriggerDialogue(1);
                break;
            case Damagochi.Condition.Good:
                triggers[(int)nowPet].TriggerDialogue(2);
                break;
            case Damagochi.Condition.Normal:
                triggers[(int)nowPet].TriggerDialogue(3);
                break;
            case Damagochi.Condition.Bad:
                triggers[(int)nowPet].TriggerDialogue(4);
                break;
            case Damagochi.Condition.Angry:
                triggers[(int)nowPet].TriggerDialogue(5);
                break;
        }
    }

    void FoodSet()
    {
        //밥그릇 터치시 밥 채워주기
        if (isFoodSet[(int)nowPet])
        {
            Food[(int)nowPet].SetActive(true);
            //펫이 배고프면 밥먹으러 가기
            if (pet.hungryState == Damagochi.HungryState.Little || pet.hungryState == Damagochi.HungryState.Very)
            {
                if (isBall)
                    return;
                pet.actionState = CatManager.ActionState.isEatting;
            }
        }
        else
        {
            Food[(int)nowPet].SetActive(false);
        }
    }
    public void SetPet(int a)
    {
        //펫 목록에서 선택시 셋팅
        nowPet = (Pet)a;
        switch (nowPet)
        {
            case Pet.cat:
                pet.actionState = CatManager.ActionState.isSleeping;
                bearEnvironment.SetActive(false);
                catEnvironmet.SetActive(true);
                doveEnvironment.SetActive(false);
                pet = pet_cat.GetComponent<CatManager>();
                pet.actionState = CatManager.ActionState.Idle;
                break;
            case Pet.bear:
                pet.actionState = CatManager.ActionState.isSleeping;
                bearEnvironment.SetActive(true);
                catEnvironmet.SetActive(false);
                doveEnvironment.SetActive(false);
                pet = pet_bear.GetComponent<CatManager>();
                pet.actionState = CatManager.ActionState.Idle;
                break;
            case Pet.dove:
                pet.actionState = CatManager.ActionState.isSleeping;
                bearEnvironment.SetActive(false);
                catEnvironmet.SetActive(false);
                doveEnvironment.SetActive(true);
                pet = pet_dove.GetComponent<CatManager>();
                pet.actionState = CatManager.ActionState.Idle;
                break;
            default:
                break;
        }
    }


    void ConditionSet()
    {
        if (currImacy >= 80)
        {
            pet.condition = Damagochi.Condition.Happy;
            IntimacyBar.color = Color.green;
        }
        else if (currImacy < 80 && currImacy >= 60)
        {
            pet.condition = Damagochi.Condition.Good;
            IntimacyBar.color = Color.green;
        }
        else if (currImacy < 60 && currImacy >= 40)
        {
            pet.condition = Damagochi.Condition.Normal;
            IntimacyBar.color = Color.green;
        }
        else if (currImacy < 40 && currImacy >= 20)
        {
            pet.condition = Damagochi.Condition.Bad;
            IntimacyBar.color = Color.yellow;
        }
        else
        {
            pet.condition = Damagochi.Condition.Angry;
            IntimacyBar.color = Color.red;
        }
    }



    void Hungry()
    {
        if (currH >= 80)
        {
            pet.hungryState = Damagochi.HungryState.Full;
            HungryBar.color = Color.green;
        }
        else if(currH<80 && currH >= 60)
        {
            pet.hungryState = Damagochi.HungryState.Enough;
            HungryBar.color = Color.green;
        }
        else if(currH<60 && currH>=40)
        {
            pet.hungryState = Damagochi.HungryState.Normal;
            HungryBar.color = Color.green;
        }
        else if(currH<40 && currH >= 20)
        {
            pet.hungryState = Damagochi.HungryState.Little;
            HungryBar.color = Color.yellow;
        }
        else
        {
            pet.hungryState = Damagochi.HungryState.Very;
            HungryBar.color = Color.red;
        }
    }

    public void isBallChange()
    {
        isBall = !isBall;
        if (isBall)
        {
            pet.actionState = CatManager.ActionState.isWaiting;
        }
        else
        {
            pet.ResetDestination();
            pet.actionState = CatManager.ActionState.Idle;
        }
    }

    public void ARChange()
    {
        useAR = !useAR;
        if (useAR)
        {
            MainCam.SetActive(false);
            ARCam.SetActive(true);
            AR_Text.SetActive(true);
        }
        else
        {
            MainCam.SetActive(true);
            ARCam.SetActive(false);
            AR_Text.SetActive(false);
        }
    }

    public void ShootBall()
    {
        //클릭한 곳으로 물체 쏘기
        GameObject tObj = Instantiate(Ball[0]);
        tObj.transform.position = shootPosition.transform.position;
        Vector3 force = (shootPosition.transform.position - Camera.main.transform.position).normalized;

        Rigidbody tR = tObj.GetComponent<Rigidbody>();

        tR.AddForce(force * 100f);
    }
        
    public void Eat(int i)
    {
        switch (i)
        {
            case 0:
                break;
            case 1:
                DecreaseGold(10);
                break;
            case 2:
                DecreaseDia(10);
                break;
        }
        print("음식 채우기");
        FoodUI.SetActive(false);
        isFoodSet[(int)nowPet] = true;
    }

    public bool DecreaseGold(int i)
    {
        if(gold-i < 0)
        {
            return false;
        }
        else
        {
            gold -= i;
            return true;
        }
    }
    public bool DecreaseDia(int i)
    {
        if (dia - i < 0)
        {
            return false;
        }
        else
        {
            dia -= i;
            return true;
        }
    }
    public void GoToScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public GameObject PetGotchaPannel;
    public void PetGotcha()
    {
        //티켓 감소
        if (Ticket > 0)
        {
            Ticket--;
            PetGotchaPannel.SetActive(true);
        }
        else
            return;
    }

    void SetDialogue()
    {
        for(int i = 0; i < dialogue_data.Length; i++)
        {
            DialogueTrigger trigger = triggers[i];
            trigger.dialogue = new Dialogue();
            trigger.dialogue.SetDialogue(dialogue_data[i]);

            trigger.dialogue.name = trigger.dialogue.sentences[0][0];
        }
    }
}
