using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public struct CharacterData : ICharacterData
    {
        public static CharacterData Default = new(5, 0.05f);

        [SerializeField]
        private float _normalSpeed;
        public float NormalSpeed => _normalSpeed;

        [SerializeField]
        private float _runAwayAccelerationDelta;
        public float RunAwayAccelerationDelta => _runAwayAccelerationDelta;

        [SerializeField]
        private AnimationCurve _accelerationCurve;
        public AnimationCurve AccelerationCurve => _accelerationCurve;


        public CharacterData(float normalSpeed, float runAwayAccelerationDelta) : this()
        {
            _normalSpeed = normalSpeed;
            _runAwayAccelerationDelta = runAwayAccelerationDelta;
            _accelerationCurve = AnimationCurve.EaseInOut(0f, 0f, 2f, 1f);
        }
    }
}