using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SH_ActionDamagochi : SH_AnimeDamagochi
{
    #region Local Enum
    public enum ActionState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Battle,
        Dead,
        Stand,
        Sit,
        Eat,
    }
    public enum BattleState
    {
        Start,
        Ambushed,   //���ݴ���.
        Surprise,   //������.
        TurnWaiting,
        AttackInputWait,
        Attaking,
        End,
    }
    #endregion

    public SH_Player owner;

    //���� ��Ʋ�� ����
    public float atk;
    public float hp;
    public float maxHp;
    public float critical;
    public float criticalDamage;
    public float atkSpeed;
    public float currentTurnGage;
    public float maxTurnGage;
    public bool overPower;

    public int level;
    float e;
    public float exp
    {
        get => e;
        set
        {
            e += value;
            if (e >= maxExp)
            {
                e -= maxExp;
                level++;
            }
        }
    }
    public float maxExp => level * expGap;
    public float expGap => level * 10;
    public float deadExp => level * 2;

    public SH_Skill[] skillList;
    public SH_Skill currentActiveSkill;

    public ActionState actionState;

    public NavMeshAgent agent;
    NavMeshPath path;
    public SH_DamgochiBattleUI battleUI;

    public float wanderingRadius;//������ġ���� ����� �ʴ¼��� ����
    public float attackRange;  //������ ������ ����
    public float scanRadius;    //���� �ν��ϰ� �i�ư��� ����
    public bool battleOn;  //���� �������ΰ�?
    public SH_ActionDamagochi attackTarget;

    public BattleState battleState;

    #region Unity Event Methods
    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        path = new NavMeshPath();
        battleUI.gameObject.SetActive(false);
        skillList = GetComponentsInChildren<SH_Skill>();

        foreach (var s in skillList)
        {
            s.owner = this;
            s.timer = s.coolTime;
            s.canActive = true;
        }
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
    #endregion

    public SH_HitPoint GetRandomHitPoint()
    {
        return hitPoints[Random.Range(0, hitPoints.Length)];
    }

    public override void Do(string key)
    {
        base.Do(key);
    }

    public void Heal(float value)
    {
        hp += value;
        hp = Mathf.Min(maxHp, hp);
        battleUI.UpdateHpBar();
    }

    public void Damaged(float value)
    {
        if (overPower)
            return;

        Debug.Log("Damaged : " + value);
        SH_BattleLogger.Instance.LogText("Damaged To : " + attackTarget + "_" + value,Color.black);
        hp -= value;

        hp = Mathf.Max(0, hp);
        battleUI.UpdateHpBar();
        if(hp==0)
        {
            AnimationChange(ActionState.Dead.ToString());
            SH_BattleLogger.Instance.LogText("Dead : " + name, Color.red);
            ActionStateChange(ActionState.Dead);
        }
    }

    public void TurnGageChange(float value)
    {
        currentTurnGage += value;
        if (currentTurnGage > maxTurnGage)
            currentTurnGage = maxTurnGage;
        if (currentTurnGage < 0f)
            currentTurnGage = 0f;
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

    float deadTimer;
    public float deadSinkTime = 3f;
    void ActionStateMachine()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                Wandering();
                break;

            case ActionState.Walk:
                if (HasDestinationReached())
                {
                    ActionStateChange(ActionState.Idle);
                }
                break;

            case ActionState.Run:
                RunningAction();
                break;

            case ActionState.Battle:
                BattleStateAction();
                break;

            case ActionState.Attack:
                //AttackStateAction();
                break;

            case ActionState.Stand:
                break;

            case ActionState.Dead:
                deadTimer += Time.deltaTime;
                battleUI.gameObject.SetActive(false);
                if (deadTimer >= deadSinkTime)
                {
                    transform.DOMoveY(-10f, 3f).OnComplete(delegate { gameObject.SetActive(false); });
                    deadTimer = 0f;
                    ActionStateChange(ActionState.Idle);
                    attackTarget = null;
                }
                break;
        }
    }

    void Init()
    {
        agent.enabled = false;
        deadTimer = 0f;
        hp = maxHp;
        currentTurnGage = 0f;
        ActionStateChange(ActionState.Idle);
        battleState = BattleState.None;
        battleOn = false;
        canAnim = true;
        attackTarget = null;
        agent.enabled = true;
    }

    void RunningAction()
    {
        if (attackTarget != null)
        {
            if (AttackRangeCheck())
            {
                Debug.Log("Attack Target Reached");
                FindObjectOfType<SH_BattleManager>().StartBattle(this, attackTarget);
                ActionStateChange(ActionState.Battle);
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
        if (owner != null)
            owner.pinPointEffect.Off();

        hp = maxHp;
        agent.ResetPath();
        battleUI.gameObject.SetActive(true);
        battleState = BattleState.TurnWaiting;
        transform.DOLookAt(attackTarget.transform.position, 1f);

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
                currentTurnGage += maxTurnGage*0.5f;
                break;

            case BattleState.TurnWaiting:
                TurnGageUpdate();
                break;

            case BattleState.AttackInputWait:
                AttackInputWait();
                break;

            case BattleState.Attaking:
                break;

            case BattleState.End:
                if(hp<=0)
                {
                    ActionStateChange(ActionState.isDead);
                    battleOn = false;
                }
                battleUI.gameObject.SetActive(false);
                attackTarget = null;
                if (owner != null)
                    owner.battlePanel.gameObject.SetActive(false);

                Invoke("IdleState",2f);
                SH_BattleLogger.Instance.Clear();
                break;
        }
    }

    void IdleState()
    {
        ActionStateChange(ActionState.Idle);
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
            int r = Random.Range(0, skillList.Length);
            if (skillList[r].canActive)
            {
                skillList[r].Active();
                break;
            }
            yield return null;
        }
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
        switch (state)
        {
            case ActionState.Idle:
                AnimationChange(DamagochiAnim.Idle);
                break;

            case ActionState.Battle:
                AnimationChange(DamagochiAnim.Idle);
                break;

            case ActionState.Dead:
                AnimationChange(DamagochiAnim.Dead);
                break;

            case ActionState.Eat:
                AnimationChange(DamagochiAnim.Eat);
                break;

            case ActionState.Run:
                AnimationChange(DamagochiAnim.Run);
                break;

            case ActionState.Sit:
                AnimationChange(DamagochiAnim.Sit);
                break;

            case ActionState.Stand:
                AnimationChange(DamagochiAnim.Stand);
                break;

            case ActionState.Walk:
                break;
        }
        AnimationChange(state.ToString());
        actionState = state;
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

        if (actionState == ActionState.Battle)
        {
            Debug.Log(name + " is Already Battle");
            return false;
        }

        attackTarget = target;
        agent.stoppingDistance = attackRange;
        agent.SetDestination(target.gameObject.transform.position);
        ActionStateChange(ActionState.Run);
        return true;
    }

    Vector3 movePos;
    public void MoveTo(Vector3 pos)
    {
        if (!CanReachedPos(pos))
        {
            Debug.Log("Cant Move Pos");
            return;
        }

        if(actionState == ActionState.Battle)
        {
            Debug.Log(name + " is Battle");
            return;
        }
        movePos = pos;
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(movePos);
        ActionStateChange(ActionState.Walk);
    }

    bool CanReachedPos(Vector3 pos)
    {
        return agent.CalculatePath(pos, path);
    }

    bool HasDestinationReached()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance >= agent.stoppingDistance)
            return false;

        if (agent.velocity.sqrMagnitude != 0f)
            return false;

        return true;
    }
}
