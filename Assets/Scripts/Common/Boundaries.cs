using UnityEngine;

namespace Common
{
    public readonly struct Boundaries
    {
        public readonly float HorizontalRight;
        public readonly float HorizontalLeft;
        public readonly float VerticalTop;
        public readonly float VerticalBottom;

        public Boundaries(float horizontalWorldRight, float horizontalWorldLeft, float verticalWorldTop, float verticalWorldBottom)
        {
            HorizontalRight = horizontalWorldRight;
            HorizontalLeft = horizontalWorldLeft;
            VerticalTop = verticalWorldTop;
            VerticalBottom = verticalWorldBottom;
        }

        public Boundaries(Camera camera, float screenWidth, float screenHeight)
        {
            var worldBoundaries = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, camera.transform.position.z));
            HorizontalRight = worldBoundaries.x;
            HorizontalLeft = -worldBoundaries.x;
            VerticalTop = worldBoundaries.y;
            VerticalBottom = -worldBoundaries.y;
        }
    }
}