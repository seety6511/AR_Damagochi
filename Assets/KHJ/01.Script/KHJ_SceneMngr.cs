using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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
    public GameObject pet_cat;
    public GameObject pet_bear;


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
    float maxImacy = 100;
    public Image IntimacyImg;
    public Image IntimacyBar;
    public Image HungryBar;
    public Sprite[] ImmoSprites;

    //공 던지기
    public bool isBall;
    public GameObject Ball;
    public GameObject shootPosition;
    public GameObject PlayUI;

    //밥먹기
    public bool isEat;
    public bool isFoodSet;
    public GameObject Food;
    public GameObject FoodUI;

    //AR
    public bool useAR;
    public GameObject MainCam;
    public GameObject ARCam;
    public GameObject AR_Text;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        currH = 100;
        currImacy = 100;
        pet = CatManager.instance;
    }
    void Update()
    {
        goldUI.text = gold.ToString();
        diaUI.text = dia.ToString();
        ticketNum.text = Ticket.ToString();
        HungryBar.fillAmount = currH / maxH;
        if (!useAR)
        {
            BallPlayingCam();
            FoodCam();
        }
        Hungry();
        ConditionSet();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if(hit.transform.gameObject.name == "FoodPlate" && !isFoodSet)
                {
                    FoodUI.SetActive(!FoodUI.activeSelf);
                }
                if (hit.transform.gameObject.name == "cat-ball" && !isBall)
                {
                    PlayUI.SetActive(!PlayUI.activeSelf);
                }
                if (hit.transform.gameObject.name == "Cat")
                {
                    if (isBall)
                        return;
                    CatManager.instance.actionState = CatManager.ActionState.isTouching;
                }
            }
        }


        if (isFoodSet)
        {
            Food.SetActive(true);
            if(pet.hungryState == Damagochi.HungryState.Little || pet.hungryState == Damagochi.HungryState.Very)
            {
                if (isBall)
                    return;
                pet.actionState = CatManager.ActionState.isEatting;
            }
        }
        else
        {
            Food.SetActive(false);
        }

    }

    public void SetPet(int a)
    {
        nowPet = (Pet)a;
        switch (nowPet)
        {
            case Pet.cat:
                bearEnvironment.SetActive(false);
                catEnvironmet.SetActive(true);
                pet = pet_cat.GetComponent<CatManager>();
                break;
            case Pet.bear:
                bearEnvironment.SetActive(true);
                catEnvironmet.SetActive(false);
                pet = pet_bear.GetComponent<CatManager>();
                break;
            default:
                break;
        }
    }

    float currTime1;
    float ConditionTime = 1;
    void ConditionSet()
    {
        currTime1 += Time.deltaTime;
        if (HungryTime < currTime1)
        {
            currImacy -= 1;
            if (currImacy < 0)
            {
                currImacy = 0;
                return;
            }
            currTime1 = 0;
        }

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


    float currTime;
    float HungryTime = 1; 
    void Hungry()
    {
        currTime += Time.deltaTime;
        if(HungryTime < currTime)
        {
            currH -= 1;
            if (currH < 0)
            {
                currH = 0;
                return;
            }
            currTime = 0;
        }

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
            CatManager.instance.actionState = CatManager.ActionState.isWaiting;
        }
        else
        {
            pet.ResetDestination();
            CatManager.instance.actionState = CatManager.ActionState.Idle;
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
    void BallPlayingCam()
    {
        if (isEat)
            return;

        if (isBall)
        {
            Ball.GetComponent<DragAndThrow>().enabled = true;
            //Ball.GetComponent<DragAndThrow>().Reset();
            Camera.main.fieldOfView = 70;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -0.6f);
        }
        else
        {
            Ball.GetComponent<DragAndThrow>().enabled = false;
            Ball.transform.SetParent(null);
            Ball.transform.position = new Vector3(82.3f, 0, 0.47f);
            Ball.transform.eulerAngles = new Vector3(0, -25, 0);
            Camera.main.fieldOfView = 25;
            Camera.main.transform.position = new Vector3(82.46f, 0.38f, -2.9f);
        }
    }

    void FoodCam()
    {
        if (isEat)
        {
            Camera.main.fieldOfView = 70;
            Camera.main.transform.position = new Vector3(82.31f, 0.38f, -1.32f);
        }
    }


    public void ShootBall()
    {
        //클릭한 곳으로 물체 쏘기
        GameObject tObj = Instantiate(Ball);
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
        isFoodSet = true;
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
}
