using System;
using Core;
using Core.Logs;
using NaughtyAttributes;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class SliderWithLabel : InitializableMonoBehaviour<SliderWithLabel.Params>
    {
        public readonly struct Params
        {
            public readonly string Label;

            public readonly Vector2 SliderRange;

            public readonly float InitialValue;

            public readonly bool UseWholeNumbers;

            public Params(string label, Vector2 sliderRange, float initialValue, bool useWholeNumbers = false)
            {
                Label = label;
                SliderRange = sliderRange;
                InitialValue = initialValue;
                UseWholeNumbers = useWholeNumbers;
            }
        }

        [SerializeField, Required]
        private TMP_Text _label;

        [SerializeField, Required]
        private Slider _slider;

        private readonly ReactiveCommand<float> _valueChanged = new();
        public IObservable<float> ValueChanged => _valueChanged;

        public void SetLabel(string text)
        {
            if (_label == null)
                return;

            _label.text = text;
        }

        public void SetValue(float value, bool withoutNotify = false)
        {
            if (_slider == null)
                return;

            if (withoutNotify)
                _slider.SetValueWithoutNotify(value);
            else
                _slider.value = value;
        }

        protected override void Init()
        {
            if (Log.Assert.IsNotNull(_label) || Log.Assert.IsNotNull(_slider))
                return;

            _label.text = InputParams.Label;

            _slider.wholeNumbers = InputParams.UseWholeNumbers;
            _slider.minValue = InputParams.SliderRange.x;
            _slider.maxValue = InputParams.SliderRange.y;
            _slider.value = InputParams.InitialValue;

            _slider.onValueChanged.AsObservable().Subscribe(OnSliderValueChanged).AddTo(Disposables);
        }

        private void OnSliderValueChanged(float value)
        {
            _valueChanged.Execute(value);
        }
    }
}