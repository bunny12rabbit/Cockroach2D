using System.Collections.Generic;
using Characters;
using Core.Logs;
using Extensions;
using UnityEngine;

namespace Common.StateMachine.States
{
    public class CheckForDangerState : State
    {
        private const string RunAwayStateName = nameof(RunAwayState);
        private const string WalkToTargetStateName = nameof(WalkToTargetState);

        private readonly IDangerDetector _dangerDetector;

        private readonly Transform _transform;

        public CheckForDangerState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates, IDangerDetector dangerDetector,
            Transform transform) :
            base(nameof(CheckForDangerState), stateMachine, availableStates)
        {
            _dangerDetector = dangerDetector;
            _transform = transform;
        }

        public override void Enter()
        {
            if (_dangerDetector.IsWithinDangerArea(_transform.position))
            {
                Log.Info("Danger!".Colorize(Color.red));
                stateMachine.ChangeState(TryGetState(RunAwayStateName));
            }
            else
            {
                stateMachine.ChangeState(TryGetState(WalkToTargetStateName));
            }
        }
    }
}