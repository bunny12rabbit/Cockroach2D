using UnityEngine;

namespace Characters
{
    public interface IDangerDetector
    {
        bool IsWithinDangerArea(Vector3 position);
    }
}