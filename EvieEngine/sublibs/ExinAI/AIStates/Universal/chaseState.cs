using TriInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EvieEngine.AI.States
{
    public class chaseState : ExinAIState
    {
        private NavMeshAgent navMeshAgent;
        private NavMeshSurface navMeshSurface;
        private ExinAIController exinAI;

        [InfoBox("This is a universal AI State that comes with ExinAI, create your own conditions for more fine-tuning.", TriMessageType.Info)]
        [Header("Messages")]
        public bool sendMessages = true;
        [ShowIf("sendMessages")]
        public string isPlayerAttackMessage = "playerAttack";
        [ShowIf("sendMessages")]
        public string isPlayerLoseMessage = "playerLose";

        // ��������� �������������, ������� ����� ��������� � ����������
        [Header("Chase Settings")]
        public float maxChaseDistance = 20f; // ������������ ��������� �������������
        public float losePlayerDistance = 25f; // ���������, ��� ������� ����� ��������� ����������
        public float attackDistance = 1.5f; // ��������� ��� �����

        [Header("Animations")]
        public bool useAnimations = false;
        [ShowIf("useAnimations")]
        [InfoBox("Universal AI State only work with triggers", TriMessageType.Info)]
        public Animator animator;
        [ShowIf("useAnimations")]
        public string walkAnimation;

        [Header("More subtle settings")]
        public bool subtleSettingsShow = true;
        [ShowIf("subtleSettingsShow")]
        public string playerTag = "Player";

        //---

        private GameObject player;

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

            player = GameObject.FindWithTag(playerTag);
        }

        public override void Enter()
        {
            // ������������� ����������, ������ ���� �� ������
            if (player != null)
            {
                navMeshAgent.SetDestination(player.transform.position);

                if (useAnimations)
                {
                    animator.SetTrigger(walkAnimation);
                }
            }
        }

        public override void StateUpdate()
        {
            if (player == null)
                return;

            // ��������� ���������� �� ������
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // ���� ����� ������� ������, ���������� ��������� �� �����
            if (distanceToPlayer <= attackDistance)
            {
                if (sendMessages) exinAI.Message(isPlayerAttackMessage);
                return;
            }

            // ���� ����� ������� ������, ���������� �������������
            if (distanceToPlayer > maxChaseDistance)
            {
                if (sendMessages) exinAI.Message(isPlayerLoseMessage);
                return;
            }

            // ���������, �� ������������ �� ���� �����
            RaycastHit hit;
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // ���� ����� AI � ������� ���� �����������
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, losePlayerDistance))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    if (sendMessages) exinAI.Message(isPlayerLoseMessage);
                    return;
                }
            }

            // ���� �� �� �������� ������ � �� � �������� ����������, ���������� �������������
            navMeshAgent.SetDestination(player.transform.position);
        }

        public override void Exit()
        {
            navMeshAgent.ResetPath();  // ��������� �������� ��� ������ �� ���������
        }
    }
}