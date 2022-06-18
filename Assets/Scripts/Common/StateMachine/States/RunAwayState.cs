using System;
using System.Collections.Generic;
using Characters;
using UniRx;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Common.StateMachine.States
{
    public class RunAwayState : State, IMotor
    {
        private const string CheckForDangerStateName = nameof(CheckForDangerState);

        private readonly Transform _transform;

        private readonly float _initialSpeed;
        private readonly float _accelerationDelta;

        private float _speed;

        private IDisposable _timerDisposable;

        public RunAwayState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates, Transform transform, float speed,
            float accelerationDelta) : base(nameof(RunAwayState), stateMachine, availableStates)
        {
            _transform = transform;
            _initialSpeed = speed;
            _accelerationDelta = accelerationDelta;
        }

        private Vector3 _direction;

        public override void Enter()
        {
            _speed = _initialSpeed;

            var position = _transform.position;
            var randomInCircle = (Vector3)Random.insideUnitCircle;
            _direction = (position + randomInCircle - position).normalized;
            _transform.up = _direction;

            DisposeUtils.DisposeAndSetNull(ref _timerDisposable);

            _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ => Dispose());
        }

        public override void OnUpdate()
        {
            _speed += Time.deltaTime;
            ((IMotor) this).Move(_direction, _speed);
        }

        public override void Dispose()
        {
            ResetState();
            stateMachine.ChangeState(TryGetState(CheckForDangerStateName));
        }

        protected override void ResetState()
        {
            _speed = _initialSpeed;
            DisposeUtils.DisposeAndSetNull(ref _timerDisposable);
        }

        void IMotor.Move(Vector3 direction, float speed)
        {
            _transform.position += direction * speed * Time.deltaTime;
        }
    }
}