using EvieEngine.AI.Demo;
using UnityEngine;
using UnityEngine.AI;

namespace EvieEngine.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour
    {
        public EnemyConfig config;
        public Blackboard blackboard = new Blackboard();
        public PerceptionSensor perception;
        public Transform[] patrolPoints;
        public NavMeshAgent agent;
        public Animator animator; // optional
        public float waypointTolerance = 0.5f;

        private StateMachine stateMachine;

        // Теперь публичные readonly-поля состояний
        [HideInInspector] public IdleState idleState;
        [HideInInspector] public PatrolState patrolState;
        [HideInInspector] public ChaseState chaseState;
        [HideInInspector] public AttackState attackState;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (config != null)
                agent.speed = config.walkSpeed;

            stateMachine = new StateMachine();

            // создаём состояния один раз
            idleState = new IdleState(this, stateMachine);
            patrolState = new PatrolState(this, stateMachine);
            chaseState = new ChaseState(this, stateMachine);
            attackState = new AttackState(this, stateMachine);
        }

        private void Start()
        {
            // можно начать с idleState или patrolState
            stateMachine.Initialize(patrolState);
        }

        private void Update()
        {
            if (blackboard.Target != null)
            {
                blackboard.TargetVisible = perception.CanSee(blackboard.Target);
                if (blackboard.TargetVisible)
                    blackboard.LastKnownPosition = blackboard.Target.position;
            }

            stateMachine.Tick();

            if (animator)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }

        public Transform GetNearestPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, config.visionRadius, perception.targetMask);
            float best = float.MaxValue;
            Transform bestT = null;
            foreach (var c in hits)
            {
                float d = Vector3.Distance(transform.position, c.transform.position);
                if (d < best)
                {
                    best = d;
                    bestT = c.transform;
                }
            }

            return bestT;
        }
    }
}
