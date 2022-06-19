using UnityEngine;

namespace Characters
{
    public interface ICharacterData
    {
        float NormalSpeed { get; }
        float RunAwayAccelerationPerSecondDelta { get; }

        AnimationCurve AccelerationCurve { get; }
    }
}