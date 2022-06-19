using System;
using Core;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Windows
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Window<TInputParams> : InitializableMonoBehaviour<TInputParams>, IWindow
    {
        [SerializeField, Required]
        protected Button _closeButton;
        public IObservable<Unit> CloseButtonClicked => _closeButton != null ? _closeButton.OnClickAsObservable() : Observable.Never<Unit>();

        protected override void Init()
        {
            CloseButtonClicked.Subscribe(_ => Hide()).AddTo(Disposables);
        }

        public void Hide()
        {
            Dispose();
        }
    }
}