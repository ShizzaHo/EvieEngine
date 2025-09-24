// IdleState.cs
using UnityEngine;

namespace EvieEngine.AI.Demo
{
    public class IdleState : IState
    {
        private EnemyAI ai;
        private StateMachine sm;
        private float idleDuration;
        private float timer = 0f;

        public IdleState(EnemyAI ai, StateMachine sm, float idleDuration = 2.5f)
        {
            this.ai = ai;
            this.sm = sm;
            this.idleDuration = idleDuration;
        }

        public void Enter()
        {
            timer = 0f;
            ai.agent.isStopped = true;
            ai.agent.ResetPath();
        }

        public void Exit()
        {
            ai.agent.isStopped = false;
        }

        public void Tick()
        {
            var p = ai.GetNearestPlayer();
            if (p != null && ai.perception.CanSee(p))
            {
                ai.blackboard.Target = p;
                sm.ChangeState(ai.chaseState);
                return;
            }

            timer += Time.deltaTime;
            if (timer >= idleDuration)
            {
                sm.ChangeState(ai.patrolState);
            }
        }
    }
}