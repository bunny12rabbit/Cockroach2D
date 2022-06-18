using System.Collections.Generic;
using Characters;
using Unity.Mathematics;
using UnityEngine;

namespace Common.StateMachine.States
{
    public class WalkToTargetState : State, IMotor
    {
        private const float LocationTolerance = 0.5f;

        private const string CheckForDangerStateName = nameof(CheckForDangerState);

        private readonly Transform _transform;

        private readonly Vector3 _targetPosition;

        private readonly float _speed;
        private Vector3 _direction;


        public WalkToTargetState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates, Transform transform,
            Vector3 targetPosition, float speed) : base(nameof(WalkToTargetState), stateMachine, availableStates)
        {
            _transform = transform;
            _targetPosition = targetPosition;
            _speed = speed;
        }

        private bool IsFinishPointReached => math.abs(_targetPosition.x - _transform.position.x) < LocationTolerance &&
                                             math.abs(_targetPosition.y - _transform.position.y) < LocationTolerance;

        public override void Enter()
        {
            var position = _transform.position;

            _transform.up = _targetPosition - position;
            _direction = _transform.up.normalized;
        }

        public override void OnUpdate()
        {
            if (IsFinishPointReached)
                return;

            ((IMotor) this).Move(_direction, _speed);

            stateMachine.ChangeState(TryGetState(CheckForDangerStateName));
        }

        void IMotor.Move(Vector3 direction, float speed)
        {
            _transform.position += direction * speed * Time.deltaTime;
        }
    }
}