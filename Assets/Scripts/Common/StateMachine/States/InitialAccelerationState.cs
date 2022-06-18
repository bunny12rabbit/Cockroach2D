using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Common.StateMachine.States
{
    public class InitialAccelerationState : State, IMotor
    {
        private const string WalkToTargetStateName = nameof(WalkToTargetState);

        private readonly Transform _transform;

        private readonly float _targetSpeed;

        private readonly AnimationCurve _accelerationCurve;

        public InitialAccelerationState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates, Transform transform,
            float targetSpeed, AnimationCurve accelerationCurve) : base(nameof(InitialAccelerationState), stateMachine, availableStates)
        {
            _transform = transform;
            _targetSpeed = targetSpeed;
            _accelerationCurve = accelerationCurve;
        }

        private float _accelerationTime;
        private float _currentSpeed;

        public override void OnUpdate()
        {
            if (_currentSpeed < _targetSpeed)
            {
                _accelerationTime += Time.deltaTime;
                _currentSpeed = _targetSpeed * _accelerationCurve.Evaluate(_accelerationTime);

                ((IMotor) this).Move(_transform.up, _currentSpeed);
            }
            else
            {
                stateMachine.ChangeState(TryGetState(WalkToTargetStateName));
            }
        }

        void IMotor.Move(Vector3 direction, float speed)
        {
            _transform.position += direction * speed * Time.deltaTime;
        }
    }
}