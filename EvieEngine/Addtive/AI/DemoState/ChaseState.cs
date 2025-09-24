// ChaseState.cs
using UnityEngine;

namespace EvieEngine.AI.Demo
{
    public class ChaseState : IState
    {
        private EnemyAI ai;
        private StateMachine sm;
        private float loseTimer = 0f;

        public ChaseState(EnemyAI ai, StateMachine sm)
        {
            this.ai = ai;
            this.sm = sm;
        }

        public void Enter()
        {
            ai.agent.isStopped = false;
            ai.agent.speed = ai.config.runSpeed;
            loseTimer = 0f;
        }

        public void Exit() { }

        public void Tick()
        {
            var t = ai.blackboard.Target;
            if (t == null)
            {
                loseTimer += Time.deltaTime;
                if (loseTimer >= ai.config.chaseLoseTime)
                {
                    ai.blackboard.Target = null;
                    sm.ChangeState(ai.patrolState);
                }
                return;
            }

            if (ai.perception.CanSee(t))
            {
                ai.blackboard.LastKnownPosition = t.position;
                ai.agent.SetDestination(t.position);
                loseTimer = 0f;

                float dist = Vector3.Distance(ai.transform.position, t.position);
                if (dist <= ai.config.attackRange)
                {
                    sm.ChangeState(ai.attackState);
                }
            }
            else
            {
                ai.agent.SetDestination(ai.blackboard.LastKnownPosition);
                if (!ai.agent.pathPending && ai.agent.remainingDistance <= 0.5f)
                {
                    loseTimer += Time.deltaTime;
                    if (loseTimer >= ai.config.chaseLoseTime)
                    {
                        ai.blackboard.Target = null;
                        sm.ChangeState(ai.patrolState);
                    }
                }
            }
        }
    }
}