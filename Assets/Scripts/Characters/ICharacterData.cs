using UnityEngine;

namespace Characters
{
    public interface ICharacterData
    {
        float NormalSpeed { get; }
        float RunAwayAccelerationDelta { get; }

        AnimationCurve AccelerationCurve { get; }
    }
}