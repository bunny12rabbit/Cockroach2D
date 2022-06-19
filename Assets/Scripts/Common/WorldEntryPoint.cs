using Characters;
using Common.Settings;
using Common.UI;
using Core;
using Core.Logs;
using Extensions;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Common
{
    public class WorldEntryPoint : MonoBehaviour
    {
        private const string EnemyLabel = "Enemy Label";
        private const string SceneObjectsLabel = "Scene Objects";

        [SerializeField, Required]
        private GameDesignSettings _gameDesignSettings;

        [SerializeField, BoxGroup(EnemyLabel)]
        private Transform _startPoint;

        [SerializeField, BoxGroup(EnemyLabel)]
        private Transform _finishPoint;

        [SerializeField, BoxGroup(SceneObjectsLabel)]
        private PrefabPool _prefabPool;

        [SerializeField, Required]
        private Camera _camera;

        [SerializeField, Required]
        private EnemyCharacterView _enemyCharacterViewPrefab;

        [SerializeField, Required]
        private DangerDetector _dangerDetector;

        [SerializeField, Required]
        private UIManager _uiManager;

        private readonly CompositeDisposable _disposables = new();

        private CharacterSpawner _characterSpawner;


        private void OnValidate()
        {
            ValidateDependencies();
        }

        private void ValidateDependencies()
        {
            if (_gameDesignSettings == null)
            {
#if UNITY_EDITOR
                _gameDesignSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<GameDesignSettings>(GameDesignSettings.AssetPath);
#endif
            }
        }

        private void Awake()
        {
            Log.Assert.IsNotNull(_gameDesignSettings);
            Log.Assert.IsNotNull(_camera);

            _dangerDetector.gameObject.SetActive(false);

            _uiManager.Init(new UIManager.Params(_prefabPool, _gameDesignSettings, _dangerDetector)).AddTo(_disposables);
            _uiManager.StartButtonClicked.Subscribe(_ => StartGame()).AddTo(_disposables);
        }

        private void StartGame()
        {
            _dangerDetector.gameObject.SetActive(true);
            _dangerDetector.Init(new DangerDetector.Params(_camera)).AddTo(_disposables);

            var screenBorders = new Boundaries(_camera, Screen.width, Screen.height);

            var characterSpawnerParams =
                new CharacterSpawner.Params(_prefabPool, _dangerDetector, screenBorders, _finishPoint.position, _startPoint.position);

            _characterSpawner = new CharacterSpawner(characterSpawnerParams).AddTo(_disposables);

            var enemyCharacterView = _characterSpawner.Spawn(_enemyCharacterViewPrefab, _gameDesignSettings.CharacterData);
            enemyCharacterView.TargetReached.Subscribe(OnTargetReached).AddTo(enemyCharacterView.Disposables);
        }

        private void OnTargetReached(Unit _)
        {
            _uiManager.ShowGameOverWindow().First().Subscribe(_ => RestartGame());
        }

        private void RestartGame()
        {
            _characterSpawner.Dispose();
            StartGame();
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }
    }
}