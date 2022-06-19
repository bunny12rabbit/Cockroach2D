using UnityEngine;

namespace Characters
{
    public interface IDangerDetector : IDangerDetectorData
    {
        bool IsWithinDangerArea(Vector3 position);
    }
}