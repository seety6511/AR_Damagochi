using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SH_ActionDamagochi : SH_AnimeDamagochi
{
    //이하 배틀용 스텟
    public float atk;
    public float hp;
    public float maxHp;
    public float critical;
    public float atkSpeed;
    public float currentTurnGage;
    public float maxTurnGage;

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


    public NavMeshAgent agent;
    NavMeshPath path;

    public float wanderingRadius;//스폰위치에서 벗어나지 않는선의 범위
    public float attackRange;  //공격이 가능한 범위
    public float scanRadius;    //적을 인식하고 쫒아가는 범위
    public bool battleOn;  //현재 전투중인가?
    public SH_DamgochiBattleUI battleUI;
    public SH_ActionDamagochi attackTarget;
    public enum BattleState
    {
        Start,
        Ambushed,   //공격당함.
        Surprise,   //기습함.
        TurnWaiting,
        AttackInputWait,
        Attaking,
        End,
    }
    public BattleState battleState;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
        battleUI.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
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
                RunningAction();
                break;

            case ActionState.isBattle:
                BattleStateAction();
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
    
    void RunningAction()
    {
        if (attackTarget != null)
        {
            if (AttackRangeCheck())
            {
                Debug.Log("Attack Target Reached");
                FindObjectOfType<SH_BattleManager>().StartBattle(this, attackTarget);
                ActionStateChange(ActionState.isBattle);
                AnimationChange(ActionState.Idle.ToString());
            }
            else
                agent.SetDestination(attackTarget.transform.position);
        }
        else if (HasDestinationReached())
        {
            ActionStateChange(ActionState.Idle);
        }
    }
    public void Look(GameObject target)
    {
        transform.LookAt(target.transform);
    }

    void BattleInit()
    {
        agent.ResetPath();
        battleUI.gameObject.SetActive(true);
        battleState = BattleState.TurnWaiting;
        transform.DOLookAt(attackTarget.transform.position, 1f);

        foreach(var s in skillList)
        {
            s.owner = this;
        }

    }
    public void BattleStateAction()
    {
        foreach(var s in skillList)
        {
            s.CoolTimeUpdate();
        }

        switch (battleState)
        {
            case BattleState.Ambushed:
                BattleInit();
                break;

            case BattleState.Surprise:
                BattleInit();
                currentTurnGage += 50;
                break;

            case BattleState.TurnWaiting:
                TurnGageUpdate();
                break;

            case BattleState.AttackInputWait:
                AttackInputWait();
                break;

            case BattleState.Attaking:
                Attack();
                break;

            case BattleState.End:
                attackTarget = null;
                break;
        }
    }

    void AttackInputWait()
    {
        if (playerble)
            return;

        StartCoroutine(AIAttackSkillSelect());
    }

    IEnumerator AIAttackSkillSelect()
    {
        while (true)
        {
            int r = Random.Range(0, skillList.Count);
            if (skillList[r].canActive)
            {
                skillList[r].Active();
                break;
            }
            yield return null;
        }
    }

    protected virtual void Attack()
    {
    }

    void TurnGageUpdate()
    {
        if (currentTurnGage >= maxTurnGage)
            battleState = BattleState.AttackInputWait;
        else
        {
            currentTurnGage += atkSpeed * Time.deltaTime;
            battleUI.UpdateTurnGage();
        }
    }

    bool AttackRangeCheck()
    {
        if (Vector3.Distance(gameObject.transform.position, attackTarget.gameObject.transform.position) > attackRange)
            return false;
        return true;
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

        if (actionState == ActionState.isBattle)
        {
            Debug.Log(name + " is Already Battle");
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

        if(actionState == ActionState.isBattle)
        {
            Debug.Log(name + " is Battle");
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
