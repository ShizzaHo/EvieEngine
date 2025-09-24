using TriInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EvieEngine.AI.States
{
    public class attackState : ExinAIState
    {
        private NavMeshAgent navMeshAgent;
        private NavMeshSurface navMeshSurface;
        private ExinAIController exinAI;

        [InfoBox("This is a universal AI State that comes with ExinAI, create your own conditions for more fine-tuning.", TriMessageType.Info)]
        [Header("Messages")]
        public bool sendMessages = true;
        [ShowIf("sendMessages")]
        public string isPlayerAttackEndMessage = "playerAttackEnd";

        [Header("AI Navigation Settings")]
        public float detectionRadius = 3f;
        public float stopDistance = 1.5f;

        [Header("Animations")]
        public bool useAnimations = false;
        [ShowIf("useAnimations")]
        [InfoBox("Universal AI State only work with triggers", TriMessageType.Info)]
        public Animator animator;
        [ShowIf("useAnimations")]
        public string attackAnimation;

        private Transform player;

        private void Start()
        {
            exinAI = GetComponent<ExinAIController>();

            if (exinAI == null)
            {
                Debug.LogError($"[EVIECORE/EXINAI/ERROR] ExinAIController not found! Make sure it is added to the scene before using it {gameObject.name}.");
                return;
            }

            navMeshAgent = exinAI.NavMeshAgent;
            navMeshSurface = exinAI.NavMeshSurface;

            // ����� ������
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        public override void Enter()
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = false;

                if (useAnimations)
                {
                    animator.SetTrigger(attackAnimation);
                }
            }
        }

        public override void StateUpdate()
        {
            if (player == null || navMeshAgent == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= stopDistance)
            {
                // ����� �����, ����� ����� � �������
                navMeshAgent.isStopped = true;
            }
            else if (distanceToPlayer <= detectionRadius)
            {
                // ����� � �������� �������, ����� ����������
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                // ����� ������� ������, ���������� ���������
                if (exinAI != null)
                {
                    if (sendMessages) exinAI.Message(isPlayerAttackEndMessage);
                }
                navMeshAgent.isStopped = true;
            }
        }

        public override void Exit()
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
        }
    }
}