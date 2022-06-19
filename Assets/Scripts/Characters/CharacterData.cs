using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public struct CharacterData : ICharacterData
    {
        public static CharacterData Default = new(5, 10f);

        [SerializeField]
        private float _normalSpeed;
        public float NormalSpeed => _normalSpeed;

        [SerializeField]
        private float _runAwayAccelerationPerSecondDelta;
        public float RunAwayAccelerationPerSecondDelta => _runAwayAccelerationPerSecondDelta;

        [SerializeField]
        private AnimationCurve _accelerationCurve;
        public AnimationCurve AccelerationCurve => _accelerationCurve;


        public CharacterData(float normalSpeed, float runAwayAccelerationPerSecondDelta) : this()
        {
            _normalSpeed = normalSpeed;
            _runAwayAccelerationPerSecondDelta = runAwayAccelerationPerSecondDelta;
            _accelerationCurve = AnimationCurve.EaseInOut(0f, 0f, 2f, 1f);
        }

        public void SetNormalSpeed(float value) => _normalSpeed = value;
    }
}