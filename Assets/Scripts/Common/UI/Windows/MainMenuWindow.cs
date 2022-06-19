using System;
using Common.Settings;
using Core.Logs;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Common.UI.Windows
{
    public class MainMenuWindow : Window<MainMenuWindow.Params>
    {
        public readonly struct Params
        {
            public readonly GameDesignSettings GameDesignSettings;

            public Params(GameDesignSettings gameDesignSettings)
            {
                GameDesignSettings = gameDesignSettings;
            }
        }

        public IObservable<Unit> StartButtonClicked => CloseButtonClicked;

        [SerializeField, Required]
        private SliderWithLabel _enemyCharacterSpeedSlider;

        [SerializeField, Required]
        private SliderWithLabel _enemiesAmountSlider;

        private GameDesignSettings GameDesignSettings => InputParams.GameDesignSettings;

        protected override void Init()
        {
            base.Init();

            if (Log.Assert.IsNotNull(_enemyCharacterSpeedSlider))
                return;

            SetupEnemySpeedSlider();
            SetupEnemiesAmountSlider();
        }

        private void SetupEnemySpeedSlider()
        {
            _enemyCharacterSpeedSlider.Init(
                    new SliderWithLabel.Params("Enemy Speed", new Vector2(1, 10), GameDesignSettings.CharacterData.NormalSpeed))
                .AddTo(Disposables);

            _enemyCharacterSpeedSlider.ValueChanged.Subscribe(OnEnemySpeedSliderValueChanged).AddTo(Disposables);
        }

        private void SetupEnemiesAmountSlider()
        {
            _enemiesAmountSlider.Init(
                    new SliderWithLabel.Params("Enemies Amount", new Vector2(1, 3), GameDesignSettings.EnemiesAmount, true))
                .AddTo(Disposables);

            _enemiesAmountSlider.ValueChanged.Subscribe(OnEnemiesAmountSliderValueChanged).AddTo(Disposables);
        }

        private void OnEnemySpeedSliderValueChanged(float value)
        {
            var characterData = GameDesignSettings.CharacterData;
            characterData.SetNormalSpeed(value);
            GameDesignSettings.SetCharacterData(characterData);
        }

        private void OnEnemiesAmountSliderValueChanged(float value)
        {
            GameDesignSettings.EnemiesAmount = Mathf.FloorToInt(value);
        }
    }
}