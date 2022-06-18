using System;
using Common.StateMachine.States;

namespace Common.StateMachine
{
    public class StateMachine : IDisposable
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void UpdateState()
        {
            CurrentState.OnUpdate();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            newState.Enter();
        }

        public void Dispose()
        {
            CurrentState.Dispose();
        }
    }
}