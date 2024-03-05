using UnityEngine;
using UnityEngine.AI;

public class EnemyController : ActorController
{
    public GameObject Target { get; set; }
    public string TargetTag { get; set; }

    public EnemyState CurrentState { get; set; }

    private StateMachine m_CurrentState;
    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private SkillSystem m_SkillSystem;
    private NavMeshAgent m_NavMeshAgent;

    /**
    * Sight
    */
    private float m_MaxSightDistance = 10f;
    private float m_FOV = 45f;
    private LayerMask m_LayerMask;
    private Collider[] m_Colliders = new Collider[10];

    /**
    * Combat parameters
    */
    private float m_AttackRange;
    private float m_AttackTimer = 0f;

    private Vector3[] m_PatrolPoints;
    private int m_CurPatrolIdx = 0;
    private float m_WaitTime = 3f;
    private float m_WaitTimer = 0f;

    private float m_UpdateRate = 0.5f;
    private float m_UpdateTimer = 0f;

    public static readonly int SpeedHash = Animator.StringToHash("Speed");
    public static readonly int AttackStateHash = Animator.StringToHash("Attack");

    public override void Init(ActorBehaviour ab)
    {
        base.Init(ab);
        EnemyBehaviour eb = ab as EnemyBehaviour;
        if (eb == null) return;
        TargetTag = eb.TargetTag;
        m_MaxSightDistance = eb.MaxSightDistance;
        m_FOV = eb.FOV;
        m_LayerMask = eb.LayerMask;
        m_AttackRange = eb.AttackRange;
        m_PatrolPoints = eb.PatrolPoints;
        m_WaitTime = eb.WaitTime;
        m_UpdateRate = eb.UpdateRate;
    }

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SkillSystem = GetController<SkillSystem>();
        if (m_CurrentState == null)
        {
            m_CurrentState = new IdleState(this);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (m_NavMeshAgent.hasPath)
        {
            if (CurrentState == EnemyState.Chasing)
            {
                UpdateTarget(); // 有target则无需间歇式更新
            }
            m_Animator.SetFloat(SpeedHash, m_NavMeshAgent.velocity.magnitude);
            m_CharacterController.Move(m_NavMeshAgent.velocity * Time.deltaTime);
        }

        if (m_WaitTimer > 0f)
        {
            m_WaitTimer -= Time.deltaTime;
        }

        if (m_AttackTimer > 0f)
        {
            m_AttackTimer -= Time.deltaTime;
        }

        if (m_UpdateTimer > 0f)
        {
            m_UpdateTimer -= Time.deltaTime;
        }
        else
        {
            m_UpdateTimer = m_UpdateRate;
            UpdateTarget();
            m_CurrentState = m_CurrentState.Update(this);
        }

    }

    public void UpdateTarget()
    {
        if (Target != null)
        {
            // 只检测距离，忽视视线
            if (Vector3.Distance(transform.position, Target.transform.position) > m_MaxSightDistance)
            {
                // 超出视线范围（仇恨消失）
                Target = null;
                return;
            }
        }

        int hits = Physics.OverlapSphereNonAlloc(transform.position, m_MaxSightDistance, m_Colliders, m_LayerMask);
        for (int i = 0; i < hits; i++)
        {
            if (m_Colliders[i].CompareTag(TargetTag))
            {
                if (IsInSight(m_Colliders[i]))
                {
                    Target = m_Colliders[i].gameObject;
                    break; // Found target
                }
            }
        }
    }

    public bool IsInSight(Collider collider)
    {
        // 简单的视线检测
        float height = collider.bounds.size.y;
        Vector3 direction = collider.transform.position - transform.position + Vector3.up * height * 0.5f;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < m_FOV)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, m_MaxSightDistance, m_LayerMask))
            {
                if (hit.collider == collider)
                {
                    return true;
                }
            }
        }
        return false;
    }


    /**
    * AI Behavior
    */
    public bool CanAttack()
    {
        return Target != null &&
         Vector3.Distance(transform.position, Target.transform.position) <= m_AttackRange &&
         m_AttackTimer <= 0f;
    }

    public bool CanPatrol()
    {
        return m_PatrolPoints.Length > 0 && m_WaitTimer <= 0f;
    }

    public void Attack()
    {
        // PlayAnim(AttackStateHash);
        if (m_SkillSystem != null)
        {
            var cd = m_SkillSystem.ReleaseSkill(0);
            m_AttackTimer = cd;
        }
    }

    public void StartChase()
    {
        m_NavMeshAgent.SetDestination(Target.transform.position);
    }

    // 返回是否当前巡逻点已经到达
    public bool ContPatrol()
    {
        if (Vector3.Distance(transform.position, m_PatrolPoints[m_CurPatrolIdx]) < 0.5f)
        {
            m_CurPatrolIdx = (m_CurPatrolIdx + 1) % m_PatrolPoints.Length;
            m_WaitTimer = m_WaitTime;
            return true;
        }

        m_NavMeshAgent.SetDestination(m_PatrolPoints[m_CurPatrolIdx]);
        return false;
    }

    public void ResetNav()
    {
        m_NavMeshAgent.ResetPath();
        m_Animator.SetFloat(SpeedHash, 0f);
    }



    public void PlayAnim(int stateHash, float transitionDuration = 0.05f)
    {
        m_Animator.CrossFade(stateHash, transitionDuration);
    }

}