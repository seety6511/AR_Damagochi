using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SH_ActionDamagochi : SH_AnimeDamagochi
{
    //���� ��Ʋ�� ����
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

    public float wanderingRadius;//������ġ���� ����� �ʴ¼��� ����
    public float attackRange;  //������ ������ ����
    public float scanRadius;    //���� �ν��ϰ� �i�ư��� ����
    public bool battleOn;  //���� �������ΰ�?
    public SH_DamgochiBattleUI battleUI;
    public SH_ActionDamagochi attackTarget;
    public enum BattleState
    {
        Start,
        Ambushed,   //���ݴ���.
        Surprise,   //�����.
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

    protected override void OnEnable()
    {
        base.OnEnable();
        hp = maxHp;
    }

    protected override void Update()
    {
        base.Update();
        ActionStateMachine();
    }

    public override void Do(string key)
    {
        base.Do(key);
        if(actionState == ActionState.isBattle)
        {
            foreach(var s in skillList)
            {
                if(s.name == key)
                {
                    FindObjectOfType<SH_TextLogControl>().LogText("Damaged : " + s.damage *atk+ "From : " + this + ", To : " + attackTarget.name, Color.black);
                    attackTarget.Damaged(s.damage);
                    break;
                }
            }
        }
    }

    public void Damaged(float value)
    {
        hp -= value*atk;

        hp = Mathf.Max(0, hp);
        battleUI.UpdateHpBar();
    }

    public override void End(string key)
    {
        base.End(key);
        AnimationChange(key, false);
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

    public bool AttackTo(SH_ActionDamagochi target)
    {
        if (target == this)
            return false;

        if (!CanReachedPos(target.gameObject.transform.position))
        {
            Debug.Log("Cant Attacked Target");
            return false;
        }

        if (actionState == ActionState.isBattle)
        {
            Debug.Log(name + " is Already Battle");
            return false;
        }

        attackTarget = target;
        agent.stoppingDistance = attackRange;
        agent.SetDestination(target.gameObject.transform.position);
        ActionStateChange(ActionState.isRunning);
        return true;
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
