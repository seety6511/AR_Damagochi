using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SH_ActionDamagochi : SH_PoolDamagochi
{
    //이하 배틀용 스텟
    public float atk;
    public float hp;
    public float maxHp;
    public float critical;
    public float atkSpeed;
    public float currentTurnGage;
    public float maxTurnGage;
    public bool canAttack;

    public List<SH_Skill> skillList;

    public enum ActionState
    {
        Idle,
        isWalking,
        isRunning,
        isAttacking,
        isBattle,
        isDead,
        isStanding,

    }
    public ActionState actionState;

    NavMeshAgent agent;
    NavMeshPath path;
    Animator animator;
    AnimatorControllerParameter[] animParam;

    public float wanderingRadius;//스폰위치에서 벗어나지 않는선의 범위
    public float attackRange;  //공격이 가능한 범위
    public float scanRadius;    //적을 인식하고 쫒아가는 범위
    public bool battleOn;  //현재 전투중인가?
    public SH_DamgochiBattleUI battleUI;
    public SH_ActionDamagochi attackTarget;
    public enum BattleState
    {
        Ambushed,   //공격당함.
        Surprise,   //기습함.
        TurnWaiting,
        Attaking,
    }
    public BattleState battleState;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
        animator = GetComponent<Animator>();
        animParam = animator.parameters;
        //battleUI = GetComponentInChildren<SH_DamgochiBattleUI>();
        battleUI.gameObject.SetActive(false);
        //Debug.Log("Awake Damagochi : " + name);
    }

    protected virtual void Update()
    {
        ActionStateMachine();
    }

    void Wandering()
    {
        if (playerble)
            return;

        var pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint, wanderingRadius);

        while (!CanReachedPos(pos))
            pos = SH_GameManager.GetRandomInnerCirclePoint(spawnPoint, wanderingRadius);

        MoveTo(pos);
    }

    void EnemyScan()
    {
        if (playerble)
            return;
    }

    void ActionStateMachine()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                Wandering();
                EnemyScan();
                break;

            case ActionState.isWalking:
                if (HasDestinationReached())
                    ActionStateChange(ActionState.Idle);
                break;

            case ActionState.isRunning:
                RunningStateAction();
                break;

            case ActionState.isBattle:
                if (!battleOn)
                {
                    agent.isStopped = true;
                    AnimationChange("isRunning", false);
                    FindObjectOfType<SH_BattleManager>().BattleStart(this, attackTarget);
                }
                break;

            case ActionState.isAttacking:
                //AttackStateAction();
                break;

            case ActionState.isStanding:
                break;

            case ActionState.isDead:
                break;
        }
    }

    public void Look(GameObject target)
    {
        transform.LookAt(target.transform);
    }

    public void BattleStateAction()
    {
        switch (battleState)
        {
            case BattleState.Ambushed:
                battleState = BattleState.TurnWaiting;
                ActionStateChange(ActionState.isBattle);
                break;

            case BattleState.Surprise:
                battleState = BattleState.TurnWaiting;
                ActionStateChange(ActionState.isBattle);
                currentTurnGage += 50;
                break;

            case BattleState.TurnWaiting:
                TurnGageUpdate();
                break;

            case BattleState.Attaking:
                Attack();
                break;
        }
    }

    protected virtual void Attack()
    {
        if (!canAttack)
            return;
    }

    void TurnGageUpdate()
    {
        if (currentTurnGage >= maxTurnGage)
            canAttack = true;
        else
        {
            currentTurnGage += atkSpeed * Time.deltaTime;
            canAttack = false;
        }
    }

    void RunningStateAction()
    {
        if (attackTarget != null)
        {
            if (AttackRangeCheck())
                ActionStateChange(ActionState.isBattle);
            else
                AttackTo(attackTarget);
        }
        else
        {
            if (HasDestinationReached())
                ActionStateChange(ActionState.Idle);
        }
    }

    bool AttackRangeCheck()
    {
        if (Vector3.Distance(gameObject.transform.position, attackTarget.gameObject.transform.position) > attackRange)
            return false;
        return true;
    }
    void AnimatorParamClear()
    {
        foreach (var p in animParam)
        {
            animator.SetBool(p.name, false);
        }
    }

    public void AnimationChange(string key, bool value= true)
    {
        AnimatorParamClear();
        animator.SetBool(key, value);
    }

    public void ActionStateChange(ActionState state)
    {
        AnimatorParamClear();
        actionState = state;

        foreach (var p in animParam)
        {
            if (p.name == state.ToString())
            {
                animator.SetBool(state.ToString(), true);
                break;
            }
        }

    }

    public void AttackTo(SH_ActionDamagochi target)
    {
        if (!CanReachedPos(target.gameObject.transform.position))
        {
            Debug.Log("Cant Attacked Target");
            return;
        }
        attackTarget = target;
        agent.stoppingDistance = attackRange;
        agent.SetDestination(target.gameObject.transform.position);
        ActionStateChange(ActionState.isRunning);
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
        ActionStateChange(ActionState.isWalking);
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
}
