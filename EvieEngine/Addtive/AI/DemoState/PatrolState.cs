using UnityEngine;

namespace EvieEngine.AI.Demo
{
    public class PatrolState : IState
    {
        private EnemyAI ai;
        private StateMachine sm;
        private int currentIndex = 0;

        public PatrolState(EnemyAI ai, StateMachine sm)
        {
            this.ai = ai;
            this.sm = sm;
        }

        public void Enter()
        {
            ai.agent.isStopped = false;
            ai.agent.speed = ai.config.walkSpeed;

            if (ai.patrolPoints != null && ai.patrolPoints.Length > 0)
            {
                ai.agent.SetDestination(ai.patrolPoints[currentIndex].position);
            }
        }

        public void Exit() { }

        public void Tick()
        {
            var p = ai.GetNearestPlayer();
            if (p != null && ai.perception.CanSee(p))
            {
                ai.blackboard.Target = p;
                sm.ChangeState(ai.chaseState);
                return;
            }

            if (!ai.agent.pathPending && ai.agent.remainingDistance <= ai.waypointTolerance)
            {
                currentIndex = (currentIndex + 1) % ai.patrolPoints.Length;
                ai.agent.SetDestination(ai.patrolPoints[currentIndex].position);
            }
        }
    }
}