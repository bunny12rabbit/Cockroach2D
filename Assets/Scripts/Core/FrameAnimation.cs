using System;
using Core.Logs;
using Extensions;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Utils;

namespace Core
{
    public class FrameAnimation : MonoBehaviour
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
            DisposeUtils.DisposeAndSetNull(ref _disposable);
        }

        public void Play()
        {
            DisposeUtils.DisposeAndSetNull(ref _disposable);

            if (_frames.IsNullOrEmpty())
                return;

            _currentFrame = 0;

            foreach (var frame in _frames)
                frame.SetActive(false);

            _disposable = Observable.Interval(TimeSpan.FromSeconds(_frameDuration)).Subscribe(_ => SwitchFrames());
            SwitchFrames();
        }

        private void SwitchFrames()
        {
            Log.Info($"currentIndex: {_currentFrame}");

            _frames[_currentFrame].SetActive(true);

            if (_currentFrame > 0)
                _frames[_currentFrame - 1].SetActive(false);
            else
                _frames[^1].SetActive(false);

            _currentFrame = (_currentFrame + 1) % _frames.Length;
        }
    }
}