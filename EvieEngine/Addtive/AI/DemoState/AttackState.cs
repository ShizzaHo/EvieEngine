using UnityEngine;

namespace EvieEngine.AI.Demo
{
    public class AttackState : IState
    {
        private EnemyAI ai;
        private StateMachine sm;
        private float lastAttackTime = -999f;

        public AttackState(EnemyAI ai, StateMachine sm)
        {
            this.ai = ai;
            this.sm = sm;
        }

        public void Enter()
        {
            ai.agent.isStopped = true;
        }

        public void Exit()
        {
            ai.agent.isStopped = false;
        }

        public void Tick()
        {
            var t = ai.blackboard.Target;
            if (t == null)
            {
                sm.ChangeState(ai.patrolState);
                return;
            }

            float dist = Vector3.Distance(ai.transform.position, t.position);
            if (dist > ai.config.attackRange + 0.5f)
            {
                sm.ChangeState(ai.chaseState);
                return;
            }

            Vector3 dir = (t.position - ai.transform.position);
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                ai.transform.rotation = Quaternion.Slerp(
                    ai.transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 10f
                );
            }

            if (Time.time - lastAttackTime >= ai.config.attackCooldown)
            {
                lastAttackTime = Time.time;
                ai.SendMessage("OnEnemyAttack", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}