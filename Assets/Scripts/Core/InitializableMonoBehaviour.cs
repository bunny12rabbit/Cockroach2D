using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;

namespace Core
{
    public abstract class InitializableMonoBehaviour<TInputParams> : MonoBehaviour, IInitializableMonoBehaviour<TInputParams>
    {
        private readonly CompositeDisposable _disposables = new();

        private bool _isInitialized;

        private Action _onDispose;

        Action ICustomDisposable.OnDispose
        {
            get => _onDispose;
            set => _onDispose = value;
        }

        public ICollection<IDisposable> Disposables => _disposables;

        protected TInputParams InputParams { get; private set; }

        private void OnDestroy()
        {
            DisposeUtils.EnsureIsDisposed(this, gameObject);
        }

        protected virtual void Awake()
        {
            if (!_isInitialized)
                enabled = false; // Чтобы не добавлять в каждом наследнике проверки на инициализацию в Update/FixedUpdate.
        }

        public void Dispose()
        {
            _disposables.Clear();
            InputParams = default;
            enabled = false;
            _isInitialized = false;

            DisposeUtils.InvokeAndSetNull(ref _onDispose);
        }

        public IDisposable Init(TInputParams inputParams)
        {
            InputParams = inputParams;
            enabled = true;
            _isInitialized = true;
            Init();

            return this;
        }

        protected virtual void Init()
        {
        }
    }
}