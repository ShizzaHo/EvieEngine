using TriInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EvieEngine.AI.States
{
    public class WanderState : ExinAIState
    {
        private NavMeshAgent navMeshAgent;
        private NavMeshSurface navMeshSurface;
        private ExinAIController exinAI;

        [InfoBox("This is a universal AI State that comes with ExinAI, create your own conditions for more fine-tuning.", TriMessageType.Info)]
        [Header("Messages")]
        public bool sendMessages = true;
        [ShowIf("sendMessages")]
        public string isPlayerDetectedMessage = "playerDetected";

        private Vector3 wanderTarget;

        [Header("AI Navigation Settings")]
        public float wanderRadius = 10f;
        public float detectionRange = 15f;
        public float detectionAngle = 120f;

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
        }

        public override void Enter()
        {
            SetNewWanderTarget();
        }

        public override void StateUpdate()
        {
            Wander();
            DetectPlayer();
        }

        public override void Exit()
        {
            navMeshAgent.ResetPath();
        }

        private void Wander()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                SetNewWanderTarget();

                if (useAnimations)
                {
                    animator.SetTrigger(walkAnimation);
                }
            }
        }

        private void SetNewWanderTarget()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;

            if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
            {
                wanderTarget = navHit.position;
                navMeshAgent.SetDestination(wanderTarget);
            }
        }

        private void DetectPlayer()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag(playerTag))
                {
                    Vector3 directionToPlayer = hitCollider.transform.position - transform.position;
                    float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                    if (angleToPlayer <= detectionAngle / 2)
                    {
                        Ray ray = new Ray(transform.position, directionToPlayer);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, detectionRange))
                        {
                            if (hit.collider.CompareTag(playerTag))
                            {
                                if (sendMessages) exinAI.Message(isPlayerDetectedMessage);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}