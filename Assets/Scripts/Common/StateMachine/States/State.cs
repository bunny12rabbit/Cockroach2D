using System;
using System.Collections.Generic;
using Extensions;

namespace Common.StateMachine.States
{
    public abstract class State : IDisposable
    {
        public static State Empty = new Empty(null);

        public readonly string Name;

        protected readonly StateMachine stateMachine;

        protected readonly IReadOnlyDictionary<string, State> availableStates;

        protected State(string name, StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates)
        {
            Name = name;
            this.stateMachine = stateMachine;
            this.availableStates = availableStates;
        }

        public virtual void Enter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void Exit()
        {
            ResetState();
        }

        protected virtual void ResetState()
        {
        }


        protected State TryGetState(string stateName)
        {
            return availableStates.IsNullOrEmpty() || !availableStates.TryGetValue(stateName, out var state) ? Empty : state;
        }

        public virtual void Dispose()
        {
        }
    }
}