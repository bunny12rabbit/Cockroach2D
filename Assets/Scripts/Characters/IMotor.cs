using UnityEngine;

namespace Characters
{
    public interface IMotor
    {
        void Move(Vector3 direction, float speed);
    }
}