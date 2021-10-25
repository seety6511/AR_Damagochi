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
        isWalking,
        isRunning,
        isAttacking,
        isBattle,
        isDead,
        isStanding,

    }
    public enum BattleState
    {
        None,
        Ambushed,   //공격당함.
        Surprise,   //기습함.
        TurnWaiting,
        AttackInputWait,
        Attaking,
        End,
    }
    #endregion

    public SH_Player owner;

    //이하 배틀용 스텟
    public float atk;
    public float hp;
    public float maxHp;
    public float critical;
    public float criticalDamage;
    public float atkSpeed;
    public float currentTurnGage;
    public float maxTurnGage;

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

    public List<SH_Skill> skillList;


    public ActionState actionState;

    public NavMeshAgent agent;
    NavMeshPath path;
    public SH_DamgochiBattleUI battleUI;

    public float wanderingRadius;//스폰위치에서 벗어나지 않는선의 범위
    public float attackRange;  //공격이 가능한 범위
    public float scanRadius;    //적을 인식하고 쫒아가는 범위
    public bool battleOn;  //현재 전투중인가?
    public SH_ActionDamagochi attackTarget;

    public BattleState battleState;
    public SH_HitPoint[] hitPoints;

    #region Unity Event Methods
    protected override void Awake()
    {
        base.Awake();

        hitPoints = GetComponentsInChildren<SH_HitPoint>();
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
    #endregion

    public override void Do(string key)
    {
        base.Do(key);
        if(actionState == ActionState.isBattle)
        {
            foreach(var s in skillList)
            {
                if(s.name == key)
                {
                   SH_TextLogControl.Instance.LogText("Damaged : " + s.damage *atk+ "From : " + this + ", To : " + attackTarget.name, Color.black);
                    attackTarget.Damaged(s.damage * s.owner.atk);
                    break;
                }
            }
        }
    }

    public void Damaged(float value)
    {
        hp -= value;

        hp = Mathf.Max(0, hp);
        battleUI.UpdateHpBar();
        if(hp==0)
        {
            AnimationChange("isDead");
            SH_TextLogControl.Instance.LogText("Dead : " + name, Color.red);
            ActionStateChange(ActionState.isDead);
        }
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

    float deadTimer;
    public float deadSinkTime = 3f;
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
                deadTimer += Time.deltaTime;
                battleUI.gameObject.SetActive(false);
                if (deadTimer >= deadSinkTime)
                {
                    transform.DOMoveY(-10f, 10f).OnComplete(delegate { gameObject.SetActive(false); });
                    Init();
                }
                break;
        }
    }

    void Init()
    {
        deadTimer = 0f;
        hp = maxHp;
        currentTurnGage = 0f;
        ActionStateChange(ActionState.Idle);
        battleState = BattleState.None;
        battleOn = false;
        canAnim = true;
        attackTarget = null;
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
        if (owner != null)
            owner.pinPointEffect.Off();

        hp = maxHp;
        agent.ResetPath();
        battleUI.gameObject.SetActive(true);
        battleState = BattleState.TurnWaiting;
        transform.DOLookAt(attackTarget.transform.position, 1f);

        foreach(var s in skillList)
        {
            s.owner = this;
            s.timer = s.coolTime;
            s.canActive = true;
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
                break;

            case BattleState.End:
                battleOn = false;
                battleUI.gameObject.SetActive(false);
                attackTarget = null;
                if (owner != null)
                    owner.battlePanel.gameObject.SetActive(false);

                SH_TextLogControl.Instance.Clear();
                ActionStateChange(ActionState.Idle);
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
