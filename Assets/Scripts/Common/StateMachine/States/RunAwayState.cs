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

        private readonly Boundaries _boundaries;

        private readonly Vector2 _viewSize;

        private readonly float _initialSpeed;
        private readonly float _accelerationDelta;

        private float _speed;

        private IDisposable _timerDisposable;

        public RunAwayState(StateMachine stateMachine, IReadOnlyDictionary<string, State> availableStates, Transform transform,
            Boundaries boundaries, Vector2 viewSize, float speed, float accelerationDelta) :
            base(nameof(RunAwayState), stateMachine, availableStates)
        {
            _transform = transform;
            _boundaries = boundaries;
            _viewSize = viewSize;
            _initialSpeed = speed;
            _accelerationDelta = accelerationDelta;
        }

        private Vector3 _direction;

        private bool IsBordersIntersected
        {
            get
            {
                var projectedPosition = _transform.position + _direction * _speed * Time.deltaTime;

                return projectedPosition.x > _boundaries.HorizontalRight - _viewSize.x ||
                       projectedPosition.x < _boundaries.HorizontalLeft + _viewSize.x ||
                       projectedPosition.y > _boundaries.VerticalTop - _viewSize.y ||
                       projectedPosition.y < _boundaries.VerticalBottom + _viewSize.y;
            }
        }

        void IMotor.Move(Vector3 direction, float speed)
        {
            _transform.position += direction * speed * Time.deltaTime;
        }

        public override void Enter()
        {
            _speed = _initialSpeed;

            RotateToRandomDirection();

            DisposeUtils.DisposeAndSetNull(ref _timerDisposable);

            _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ => Dispose());
        }

        private void RotateToRandomDirection()
        {
            var position = _transform.position;
            var randomInCircle = (Vector3) Random.insideUnitCircle;
            _direction = (position + randomInCircle - position).normalized;
            _transform.up = _direction;
        }

        public override void OnUpdate()
        {
            if (IsBordersIntersected)
                FindNewRandomDirection();

            _speed += _accelerationDelta * Time.deltaTime;
            ((IMotor) this).Move(_direction, _speed);
        }

        private void FindNewRandomDirection()
        {
            const float maxIterations = 10f;
            var iteration = 0;

            while (IsBordersIntersected)
            {
                RotateToRandomDirection();
                iteration++;

                if (iteration > maxIterations)
                    break;
            }
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
    }
}