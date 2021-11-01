using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatManager : SH_AnimeDamagochi
{
    public static CatManager instance;
    KHJ_SceneMngr mngr;
    NavMeshPath path;
    Animator anim;
    public NavMeshAgent agent;
    public GameObject spwanPoint;
    public GameObject Panel;
    public GameObject eatPoint;
    public float wanderingRadius;
    public Vector3 Target;


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

    protected override void Awake()
    {
        base.Awake();
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        spawnPoint = spwanPoint.transform.position;
        mngr = KHJ_SceneMngr.instance;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
        anim = GetComponent<Animator>();
    }
    protected override void Update()
    {
        SetState();
        ActionStateMachine();
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
                GetComponent<SceneAnimatorController>().SetAnimatorString("isRunning");
                Playing();
                break;

            case ActionState.isSleeping:
                agent.ResetPath();
                GetComponent<SceneAnimatorController>().SetAnimatorString("isSleeping");
                break; 
            case ActionState.isHungry:
                break;
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
            GetComponent<SceneAnimatorController>().SetAnimatorString("isEatting");
            return;
        }
        GetComponent<SceneAnimatorController>().SetAnimatorString("isWalking");
    }

    public void FinishEat()
    {
        KHJ_SceneMngr.instance.isEat = false;
        Panel.SetActive(true);
        KHJ_SceneMngr.instance.isFoodSet = false;
        KHJ_SceneMngr.instance.currH += 12;
        hungryState += 2;
        actionState = ActionState.Idle;
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
            GetComponent<SceneAnimatorController>().SetAnimatorString("isWaiting");
            return;
        }
        GetComponent<SceneAnimatorController>().SetAnimatorString("isWalking");
    }
    private void Playing()
    {
        //공이 던져지면 쫓아가기
        MoveTo(KHJ_SceneMngr.instance.Ball.transform.position);
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
        GetComponent<SceneAnimatorController>().SetAnimatorString("isWalking");
        //if (playerble)
        //    return;
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
}
