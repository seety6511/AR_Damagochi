using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatManager : Damagochi
{
    public static CatManager instance;
    KHJ_SceneMngr mngr;
    public NavMeshAgent agent;
    NavMeshPath path;
    public GameObject spawnPoint;
    public float wanderingRadius;
    public Vector3 Target;



    public enum ActionState
    {
        Idle,        
        isMoving,
        isEatting,
        isTouching,
        isPlaying,
        isSleeping,
        isHungry,
    }
    public ActionState actionState;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        mngr = KHJ_SceneMngr.instance;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
    }
    private void Update()
    {
        SetState();
        ActionStateMachine();
    }

    void ActionStateMachine()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                break;

            case ActionState.isMoving:
                Wandering();
                break;

            case ActionState.isEatting:
                break;

            case ActionState.isTouching:
                agent.ResetPath();
                GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
                break;

            case ActionState.isPlaying:
                break;

            case ActionState.isSleeping:
                break; 
            case ActionState.isHungry:
                break;
        }
    }

    public void MoveOrNot()
    {
        int a = UnityEngine.Random.Range(0, 3);
        if (a == 0)
        {
            print("Move로 변환");
            var pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint.transform.position, wanderingRadius);
            Target = pos;
            actionState = ActionState.isMoving;
        }        
    }
    public void Look(GameObject target)
    {
        transform.LookAt(target.transform);
    }
    void Wandering()
    {
        //if (playerble)
        //    return;
        if (HasDestinationReached())
        {
            int a = UnityEngine.Random.Range(0, 2);
            if (a == 0)
            {
                print("Idle로 변환");
                GetComponent<SceneAnimatorController>().SetAnimatorString("Idle");
                actionState = ActionState.Idle;
                return;
            }
            var pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint.transform.position, wanderingRadius);
            Target = pos;
        }

        while (!CanReachedPos(Target))
            Target = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint.transform.position, wanderingRadius);

        MoveTo(Target);
    }
    public void MoveTo(Vector3 pos)
    {
        print(pos);
        if (!CanReachedPos(pos))
        {
            Debug.Log("Cant Move Pos");
            return;
        }

        agent.stoppingDistance = 0.1f;
        agent.SetDestination(pos);
        GetComponent<SceneAnimatorController>().SetAnimatorString("isWalking");
    }
    bool CanReachedPos(Vector3 pos)
    {
        return agent.CalculatePath(pos, path);
    }
    bool HasDestinationReached()
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
