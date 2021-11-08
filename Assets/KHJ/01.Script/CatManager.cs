using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class CatManager : SH_AnimeDamagochi
{
    KHJ_SceneMngr mngr;
    NavMeshPath path;
    public NavMeshAgent agent;
    public GameObject spwanPoint;
    public GameObject Panel;
    public GameObject eatPoint;
    public float wanderingRadius;
    public Vector3 Target;

    public float currH;
    public float currImacy;

    public int Level;
    public float[] stat; //공격력, Hp, 치명타, 공격속도
    public Text[] stats;
    public Text leveltxt;

    public GameObject healEft;
    public GameObject heartEft;
    public enum ActionState
    {
        Idle,        
        isMoving,
        isEatting,
        isTouching,
        isWaiting,
        isPlaying,
        isSleeping,
        isHungry,
    }
    public ActionState actionState;

    private void Start()
    {
        spawnPoint = spwanPoint.transform.position;
        mngr = KHJ_SceneMngr.instance;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();

    }
    protected override void Update()
    {
        SetStat();
        SetState();
        ActionStateMachine();
        ConditionSet();
        Hungry();
    }

    void ActionStateMachine()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
                break;

            case ActionState.isMoving:
                Wandering();
                break;

            case ActionState.isEatting:
                GoToEat();
                break;

            case ActionState.isTouching:
                agent.ResetPath();
                LookCam();
                GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
                break;

            case ActionState.isWaiting:
                Waiting();
                break;

            case ActionState.isPlaying:
                GetComponent<SceneAnimatorController>().SetAnimatorString("Run");
                Playing();
                break;

            case ActionState.isSleeping:
                agent.ResetPath();
                GetComponent<SceneAnimatorController>().SetAnimatorString("Sleep");
                break; 
            case ActionState.isHungry:
                break;
        }
    }

    float currTime1;
    public float ConditionTime = 1;
    void ConditionSet()
    {
        currTime1 += Time.deltaTime;
        if (ConditionTime < currTime1)
        {
            currImacy -= 1;
            if (currImacy < 0)
            {
                currImacy = 0;
                return;
            }
            if (currImacy >= 100)
            {
                currImacy = 100;
            }
            currTime1 = 0;
        }
    }

    float currTime;
    public float HungryTime = 5;
    void Hungry()
    {
        currTime += Time.deltaTime;
        if (HungryTime < currTime)
        {
            currH -= 1;
            if (currH < 0)
            {
                currH = 0;
                return;
            }
            currTime = 0;
        }
    }


    private void GoToEat()
    {
        KHJ_SceneMngr.instance.isEat = true;

        Panel.SetActive(false);

        MoveTo(eatPoint.transform.position);
        if (HasDestinationReached())
        {
            LookCam();
            ResetDestination();
            //스폰지점에 도착하면 앉아서 밥먹기
            GetComponent<SceneAnimatorController>().SetAnimatorString("Eat");
            return;
        }
        GetComponent<SceneAnimatorController>().SetAnimatorString("Walk");
    }

    public void FinishEat()
    {
        healEft.GetComponent<ParticleSystem>().Play();
        KHJ_SceneMngr.instance.isEat = false;
        Panel.SetActive(true);
        KHJ_SceneMngr.instance.isFoodSet[(int)KHJ_SceneMngr.instance.nowPet] = false;
        switch (KHJ_SceneMngr.instance.FoodSelect[(int)KHJ_SceneMngr.instance.nowPet])
        {
            case 0:
                currH += 10;
                break;
            case 1:
                currH += 30;
                break;
            case 2:
                currH += 50;
                break;
        }
        actionState = ActionState.Idle;
        KHJ_DataManager.instance.SavePetData();
        KHJ_DataManager.instance.SaveSceneData();
    }
    public void ResetDestination()
    {
        agent.ResetPath();
    }
    private void Waiting()
    {
        MoveTo(spawnPoint);
        if (HasDestinationReached())
        {
            //스폰지점에 도착하면 앉아서 대기상태
            LookCam();
            GetComponent<SceneAnimatorController>().SetAnimatorString("Wait");
            return;
        }
        GetComponent<SceneAnimatorController>().SetAnimatorString("Walk");
    }
    private void Playing()
    {
        //공이 던져지면 쫓아가기
        MoveTo(KHJ_SceneMngr.instance.Ball[(int)KHJ_SceneMngr.instance.nowPet].transform.position);
        //if (HasDestinationReached())
        //{
        //    print("Catched Ball!");
        //    agent.ResetPath();
        //    GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
        //    actionState = ActionState.isWaiting;
        //}
    }

    public override void Do(string key)
    {
        base.Do(key);
        switch (key)
        {
            case "MoveOrNot":
                MoveOrNot();
                break;
        }
    }
    void MoveOrNot()
    {
        int a = UnityEngine.Random.Range(0, 3);
        if (a == 0)
        {
            print("Move로 변환");
            var pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint, wanderingRadius);
            Target = pos;
            actionState = ActionState.isMoving;
        }        
    }
    public void Look(GameObject target)
    {        
        Vector3 dir = target.transform.position - transform.position;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, dir, 0.3f * Time.deltaTime);
    }
    public void LookCam( )
    {
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0,180,0), 2f * Time.deltaTime);
    }

    void Wandering()
    {
        GetComponent<SceneAnimatorController>().SetAnimatorString("Walk");

        if (HasDestinationReached())
        {
            //목적지에 도착하면 랜덤 행동
            int a = UnityEngine.Random.Range(0, 3);
            print("a : " + a);
            if (a == 0)
            {
                print("Idle로 변환");
                GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
                actionState = ActionState.Idle;
                return;
            }
            if (a == 1)
            {
                print("isSleeping으로 변환");
                actionState = ActionState.isSleeping;
                return;
            }
            var pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint, wanderingRadius);
            Target = pos;
        }

        while (!CanReachedPos(Target))
            Target = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint, wanderingRadius);

        MoveTo(Target);
    }
    public void MoveTo(Vector3 pos)
    {
        if (!CanReachedPos(pos))
        {
            Debug.Log("Cant Move Pos");
            return;
        }
        
        agent.stoppingDistance = 0.1f;        
        agent.SetDestination(pos);
    }
    bool CanReachedPos(Vector3 pos)
    {
        return agent.CalculatePath(pos, path);
    }
    public bool HasDestinationReached()
    {
        if (agent.pathPending)
            return false;

        if (agent.hasPath)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.velocity.sqrMagnitude != 0f)
            return false;

        return true;
    }

    void SetState()
    {
        if(hungryState == HungryState.Little)
        {
            mngr.IntimacyImg.sprite = mngr.ImmoSprites[5];
            return;
        }
        if(hungryState == HungryState.Very)
        {
            mngr.IntimacyImg.sprite = mngr.ImmoSprites[6];
            return;
        }

        switch (condition)
        {
            case Condition.Happy:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[0];
                return;
            case Condition.Good:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[1];
                return;
            case Condition.Normal:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[2];
                return;
            case Condition.Bad:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[3];
                return;
            case Condition.Angry:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[4];
                return;
        }
    }

    public void LevelUp()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            //stat[i] += (Level+1);
        }
    }
    void SetStat()
    {
        leveltxt.text = "Lv." + (Level+1).ToString();

        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].text = (stat[i] + Level*2 ).ToString();
        }
    }
}
