using System;
using System.Collections.Generic;
using Common;
using Common.StateMachine;
using Common.StateMachine.States;
using Core;
using Core.Logs;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Characters
{
    public class EnemyCharacterView : InitializableMonoBehaviour<EnemyCharacterView.Params>
    {
        public readonly struct Params
        {
            public readonly IDangerDetector DangerDetector;

            public readonly CharacterData CharacterData;

            public readonly Boundaries Boundaries;

            public readonly Vector3 TargetPosition;

            public Params(IDangerDetector dangerDetector, CharacterData characterData, Boundaries boundaries, Vector3 targetPosition)
            {
                DangerDetector = dangerDetector;
                CharacterData = characterData;
                Boundaries = boundaries;
                TargetPosition = targetPosition;
                Boundaries = boundaries;
            }
        }

        [SerializeField, Required]
        private SpriteRenderer _renderer;

        [SerializeField, Required]
        private FrameAnimation _animation;

        private Vector2 _viewSize = Vector2.one;

        private StateMachine _stateMachine;

        private readonly Dictionary<string, State> _availableStates = new();

        private readonly ReactiveCommand _targetReached = new();
        public IObservable<Unit> TargetReached => _targetReached;

        private IDangerDetector DangerDetector => InputParams.DangerDetector;

        private CharacterData CharacterData => InputParams.CharacterData;

        private Boundaries Boundaries => InputParams.Boundaries;

        private Vector3 TargetPosition => InputParams.TargetPosition;

        protected override void Init()
        {
            _animation.Play().AddTo(Disposables);
            CalculateViewSize();
            InitStateMachine();
            RotateTowardTarget();
            Observable.EveryUpdate().Subscribe(Tick).AddTo(Disposables);
        }

        public override void Dispose()
        {
            _availableStates.Clear();
            base.Dispose();
        }

        private void CalculateViewSize()
        {
            if (Log.Assert.IsNotNull(_renderer))
                return;

            _viewSize = _renderer.bounds.size * 0.5f;
        }

        private void InitStateMachine()
        {
            var thisTransform = transform;

            _stateMachine = new StateMachine().AddTo(Disposables);

            var initialAccelerationState = new InitialAccelerationState(_stateMachine, _availableStates, thisTransform,
                CharacterData.NormalSpeed, CharacterData.AccelerationCurve);

            var walkToTargetState =
                new WalkToTargetState(_stateMachine, _availableStates, thisTransform, TargetPosition, CharacterData.NormalSpeed);

            var checkForDangerState = new CheckForDangerState(_stateMachine, _availableStates, DangerDetector, thisTransform);

            var runAwayState = new RunAwayState(_stateMachine, _availableStates, thisTransform, Boundaries, _viewSize,
                CharacterData.NormalSpeed, CharacterData.RunAwayAccelerationPerSecondDelta);

            _availableStates.Add(initialAccelerationState.Name, initialAccelerationState);
            _availableStates.Add(walkToTargetState.Name, walkToTargetState);
            _availableStates.Add(checkForDangerState.Name, checkForDangerState);
            _availableStates.Add(runAwayState.Name, runAwayState);

            walkToTargetState.StateFinished.Subscribe(OnTargetReached).AddTo(Disposables);

            _stateMachine.Initialize(initialAccelerationState);
        }

        private void OnTargetReached(Unit _)
        {
            _animation.Dispose();
            _targetReached.Execute();
        }

        private void RotateTowardTarget()
        {
            var thisTransform = transform;
            thisTransform.up = TargetPosition - thisTransform.position;
        }

        private void Tick(long _)
        {
            _stateMachine.UpdateState();
        }
    }
}