using System;
using Extensions;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Utils;

namespace Core
{
    public class FrameAnimation : MonoBehaviour, IDisposable
    {
        [SerializeField, Min(0f)]
        private float _frameDuration = 1f;

        [SerializeField]
        private bool _playOnEnable;

        [SerializeField, ValidateInput(nameof(ValidateIsNotEmpty), "Frames should contains at least 1 element")]
        private GameObject[] _frames;

        private IDisposable _disposable;

        private int _currentFrame;

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private bool ValidateIsNotEmpty(GameObject[] gameObjects) => !gameObjects.IsNullOrEmpty();

        private void OnEnable()
        {
            if (_playOnEnable)
                Play();
        }

        private void OnDisable()
        {
            Dispose();
        }

        /// <summary>
        /// Plays frame animation
        /// </summary>
        /// <returns>IDisposable that can be disposed in order to Stop animation</returns>
        public IDisposable Play()
        {
            DisposeUtils.DisposeAndSetNull(ref _disposable);

            if (_frames.IsNullOrEmpty())
                return Disposable.Empty;

            _currentFrame = 0;

            foreach (var frame in _frames)
                frame.SetActive(false);

            _disposable = Observable.Interval(TimeSpan.FromSeconds(_frameDuration)).Subscribe(_ => SwitchFrames());
            SwitchFrames();
            return _disposable;
        }

        private void SwitchFrames()
        {
            _frames[_currentFrame].SetActive(true);

            if (_currentFrame > 0)
                _frames[_currentFrame - 1].SetActive(false);
            else
                _frames[^1].SetActive(false);

            _currentFrame = (_currentFrame + 1) % _frames.Length;
        }

        public void Dispose()
        {
            DisposeUtils.DisposeAndSetNull(ref _disposable);
        }
    }
}