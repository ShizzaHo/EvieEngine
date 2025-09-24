using System;
using UnityEngine;

namespace EvieEngine.AI
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState?.Enter();
        }

        public void ChangeState(IState newState)
        {
            if (newState == null) return;
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Tick()
        {
            CurrentState?.Tick();
        }
    }
}