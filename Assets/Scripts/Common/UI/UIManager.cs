using System;
using Characters;
using Common.Settings;
using Common.UI.Windows;
using Core;
using Core.Logs;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class UIManager : InitializableMonoBehaviour<UIManager.Params>
    {
        public readonly struct Params
        {
            public readonly PrefabPool PrefabPool;

            public readonly GameDesignSettings GameDesignSettings;

            public readonly IDangerDetectorData DangerDetectorData;

            public Params(PrefabPool prefabPool, GameDesignSettings gameDesignSettings, IDangerDetectorData dangerDetectorData)
            {
                PrefabPool = prefabPool;
                GameDesignSettings = gameDesignSettings;
                DangerDetectorData = dangerDetectorData;
            }
        }

        [SerializeField, Required]
        private MainMenuWindow _mainMenuWindowPrefab;

        [SerializeField, Required]
        private GameOverWindow _gameOverWindowPrefab;

        [SerializeField, Required]
        private SliderWithLabel _dangerAreaRadiusSlider;

        private readonly ReactiveCommand _startButtonClicked = new();
        public IObservable<Unit> StartButtonClicked => _startButtonClicked;

        protected override void Init()
        {
            if (_dangerAreaRadiusSlider != null)
                _dangerAreaRadiusSlider.gameObject.SetActive(false);

            var mainMenuWindowParams = new MainMenuWindow.Params(InputParams.GameDesignSettings);
            var mainMenuWindow = InputParams.PrefabPool.Get(_mainMenuWindowPrefab, mainMenuWindowParams).AddTo(Disposables);

            mainMenuWindow.StartButtonClicked
                .Subscribe(_ =>
                {
                    _startButtonClicked.Execute();
                    SetupDangerAreaRadiusSlider();
                })
                .AddTo(Disposables);
        }

        private void SetupDangerAreaRadiusSlider()
        {
            if (Log.Assert.IsNotNull(_dangerAreaRadiusSlider))
                return;

            _dangerAreaRadiusSlider.gameObject.SetActive(true);

            var sliderParams =
                new SliderWithLabel.Params("Dangerous Area Radius", new Vector2(0.1f, 5f), InputParams.DangerDetectorData.Radius.Value);

            _dangerAreaRadiusSlider.Init(sliderParams).AddTo(Disposables);
            _dangerAreaRadiusSlider.ValueChanged.Subscribe(OnDangerAreaSliderValueChanged).AddTo(Disposables);
        }

        private void OnDangerAreaSliderValueChanged(float value)
        {
            InputParams.DangerDetectorData.Radius.Value = value;
        }

        public IObservable<Unit> ShowGameOverWindow()
        {
            var gameOverWindowParams = new MainMenuWindow.Params(InputParams.GameDesignSettings);
            var gameOverWindow = InputParams.PrefabPool.Get(_gameOverWindowPrefab, gameOverWindowParams).AddTo(Disposables);
            return gameOverWindow.RestartButtonClicked;
        }
    }
}