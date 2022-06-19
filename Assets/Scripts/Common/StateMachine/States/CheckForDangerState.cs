using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Common.StateMachine.States
{
    public class CheckForDangerState : State
    {
        private const string RunAwayStateName = nameof(RunAwayState);
        private const string WalkToTargetStateName = nameof(WalkToTargetState);

        private readonly IDangerDetector _dangerDetector;

        private readonly Transform _transform;

        public CheckForDangerState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates,
            IDangerDetector dangerDetector,
            Transform transform) :
            base(nameof(CheckForDangerState), stateMachine, availableStates)
        {
            _dangerDetector = dangerDetector;
            _transform = transform;
        }

        public override void Enter()
        {
            stateMachine.ChangeState(_dangerDetector.IsWithinDangerArea(_transform.position)
                ? TryGetState(RunAwayStateName)
                : TryGetState(WalkToTargetStateName));
        }
    }
}