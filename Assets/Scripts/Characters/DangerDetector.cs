using Core;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Characters
{
    public class DangerDetector : InitializableMonoBehaviour<DangerDetector.Params>, IDangerDetector
    {
        public readonly struct Params
        {
            public readonly Camera Camera;

            public Params(Camera camera)
            {
                Camera = camera;
            }
        }

        [SerializeField, Required]
        private SpriteRenderer _dangerousAreaImage;


        [SerializeField]
        private float _dangerRadius = 1.2f;

        private Camera Camera => InputParams.Camera;

        private Vector3 MouseWorldPosition
        {
            get
            {
                var position = Camera.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0;

                return position;
            }
        }

        private void OnValidate()
        {
            TrySetVisualSize();
        }

        private void TrySetVisualSize()
        {
            if (_dangerousAreaImage == null)
                return;

            transform.localScale = Vector2.one * _dangerRadius;
        }

        protected override void Init()
        {
            TrySetVisualSize();
            Observable.EveryUpdate().Subscribe(Tick).AddTo(Disposables);
        }

        private void Tick(long _)
        {
            transform.position = MouseWorldPosition;
        }

        public bool IsWithinDangerArea(Vector3 position) => Vector3.Distance(MouseWorldPosition, position) <= _dangerRadius;
    }
}