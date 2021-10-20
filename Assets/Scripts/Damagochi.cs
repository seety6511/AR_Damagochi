using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Damagochi : MonoBehaviour
{
    public new string name;
    NavMeshAgent agent;
    NavMeshPath path;
    Animator animator;
    AnimatorControllerParameter[] animParam;

    List<Damagochi> enablePool;
    List<Damagochi> disablePool;

    public bool playerble;

    public void SetPool(List<Damagochi> enablePool, List<Damagochi> disablePool)
    {
        this.enablePool = enablePool;
        this.disablePool = disablePool;
    }

    public void On(Vector3 pos)
    {
        disablePool[0].transform.position = pos;
        disablePool[0].gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        if (playerble)
            return;

        if (enablePool == null || disablePool == null)
            return;

        enablePool.Add(this);
        disablePool.Remove(this);
    }

    protected virtual void OnDisable()
    {
        if (playerble)
            return;

        enablePool.Remove(this);
        disablePool.Add(this);
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
        animator = GetComponent<Animator>();
        animParam = animator.parameters;
    }

    int hungry
    {
        set
        {
            hungry += value;
            Mathf.Clamp(hungry, 0, 9);
        }
        get
        {
            return hungry;
        }
    }  //배고픔
    public enum HungryState
    {
        Very,
        Little,
        Normal,
        Enough,
        Full,
    }
    public HungryState hungryState;

    int intimacy {
        set
        {
            intimacy += value;
            Mathf.Clamp(intimacy, 0, 9);
        }
        get
        {
            return intimacy;
        }
    }   //애정도

    public enum Condition
    {
        Angry = 0,
        Bad = 3,
        Normal = 5,
        Good = 7,
        Happy= 10,
    }
    public Condition condition; //지금 기분

    public enum ActionState
    {
        Idle,
        isWalking,
        isRunning,
        isAttacking,
        isDead,
        isStanding,

    }
    public ActionState actionState;

    public float moveSpeed;

    //이하 배틀용 스텟
    public float atk;
    public float hp;
    public float critical;
    public float atkSpeed;

    protected virtual void Update()
    {
        ActionStateMachine();
    }

    void ActionStateMachine()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                break;

            case ActionState.isWalking:
                if (HasDestinationReached())
                    ActionStateChange(ActionState.Idle);
                break;

            case ActionState.isRunning:
                break;

            case ActionState.isAttacking:
                break;

            case ActionState.isStanding:
                break;

            case ActionState.isDead:
                break;

        }
    }

    void AnimatorParamClear()
    {
        foreach(var p in animParam)
        {
            animator.SetBool(p.name, false);
        }
    }

    public void ActionStateChange(ActionState state)
    {
        AnimatorParamClear();
        actionState = state;
        if (state == ActionState.Idle)
            return;

        animator.SetBool(state.ToString(), true);
    }

    public void HungryChange(int value)
    {
        hungry += value;
        switch (hungry)
        {
            case 0:
            case 1:
                hungryState = HungryState.Very;
                break;
            case 2:
            case 3:
                hungryState = HungryState.Little;
                break;
            case 4:
            case 5:
                hungryState = HungryState.Normal;
                break;
            case 6:
            case 7:
                hungryState = HungryState.Enough;
                break;
            case 8:
            case 9:
                hungryState = HungryState.Full;
                break;
        }
    }

    public void IntimacyChange(int value)
    {
        intimacy += value;
        switch (intimacy)
        {
            case 0:
            case 1:
                condition = Condition.Angry;
                break;
            case 2:
            case 3:
                condition = Condition.Bad;
                break;
            case 4:
            case 5:
                condition = Condition.Normal;
                break;
            case 6:
            case 7:
                condition = Condition.Good;
                break;
            case 8:
            case 9:
                condition = Condition.Happy;
                break;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        bool result = agent.CalculatePath(pos, path);
        if(!result)
        {
            print("Cant Move");
            return;
        }

        agent.SetDestination(pos);
        ActionStateChange(ActionState.isWalking);
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
}
