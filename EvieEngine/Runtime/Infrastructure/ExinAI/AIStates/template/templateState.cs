using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace EvieEngine.AI.States
{
    public class TemplateState : ExinAIState
    {
        private NavMeshAgent navMeshAgent;
        private NavMeshSurface navMeshSurface;
        private ExinAIController exinAI;

        private void Start()
        {
            exinAI = GetComponent<ExinAIController>();

            navMeshAgent = exinAI.NavMeshAgent;
            navMeshSurface = exinAI.NavMeshSurface;
        }

        public override void Enter()
        {

        }

        public override void StateUpdate()
        {

        }

        public override void Exit()
        {

        }
    }
}