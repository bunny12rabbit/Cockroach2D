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

        private GameDesignSettings GameDesignSettings => InputParams.GameDesignSettings;

        protected override void Init()
        {
            base.Init();

            if (Log.Assert.IsNotNull(_enemyCharacterSpeedSlider))
                return;

            SetupEnemySpeedSlider();
        }

        private void SetupEnemySpeedSlider()
        {
            _enemyCharacterSpeedSlider.Init(
                    new SliderWithLabel.Params("Enemy Speed", new Vector2(1, 10), GameDesignSettings.CharacterData.NormalSpeed))
                .AddTo(Disposables);

            _enemyCharacterSpeedSlider.ValueChanged.Subscribe(OnEnemySpeedSliderValueChanged).AddTo(Disposables);
        }

        private void OnEnemySpeedSliderValueChanged(float value)
        {
            var characterData = GameDesignSettings.CharacterData;
            characterData.SetNormalSpeed(value);
            GameDesignSettings.SetCharacterData(characterData);
        }
    }
}