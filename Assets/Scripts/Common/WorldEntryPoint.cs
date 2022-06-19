using System.Linq;
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
        private const string EnemyLabel = "Enemy";
        private const string SceneObjectsLabel = "Scene Objects";

        [SerializeField, Required, BoxGroup(SceneObjectsLabel)]
        private PrefabPool _prefabPool;

        [SerializeField, Required, BoxGroup(SceneObjectsLabel)]
        private Camera _camera;

        [SerializeField, Required, BoxGroup(SceneObjectsLabel)]
        private UIManager _uiManager;

        [SerializeField, Required, BoxGroup(SceneObjectsLabel)]
        private DangerDetector _dangerDetector;

        [SerializeField, ValidateInput(nameof(ValidateIsNotEmpty), "Should contains at least 1 element"), BoxGroup(EnemyLabel)]
        private Transform[] _startPoints;

        [SerializeField, Required, BoxGroup(EnemyLabel)]
        private Transform _finishPoint;

        [SerializeField, Required, BoxGroup(EnemyLabel)]
        private EnemyCharacterView _enemyCharacterViewPrefab;

        [SerializeField, Required]
        private GameDesignSettings _gameDesignSettings;

        private readonly CompositeDisposable _disposables = new();
        private readonly CompositeDisposable _spawnedEnemiesDisposables = new();

        private CharacterSpawner _characterSpawner;

        private Vector3[] StartPositions => _startPoints?.Select(point => point.position).ToArray() ?? Empty.Array<Vector3>();


        private bool ValidateIsNotEmpty(Transform[] transforms) => !transforms.IsNullOrEmpty();

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
                new CharacterSpawner.Params(_prefabPool, _dangerDetector, screenBorders, _finishPoint.position, StartPositions);

            _characterSpawner = new CharacterSpawner(characterSpawnerParams).AddTo(_disposables);

            var enemyCharacterViews =
                _characterSpawner.SpawnEnemy(_enemyCharacterViewPrefab, _gameDesignSettings.CharacterData, _gameDesignSettings.EnemiesAmount);

            enemyCharacterViews
                .Select(enemy => enemy.TargetReached)
                .Merge()
                .First()
                .Subscribe(OnTargetReached)
                .AddTo(_spawnedEnemiesDisposables);
        }

        private void OnTargetReached(Unit _)
        {
            _uiManager.ShowGameOverWindow().First().Subscribe(_ => RestartGame());
        }

        private void RestartGame()
        {
            _spawnedEnemiesDisposables.Clear();
            _characterSpawner.Dispose();
            StartGame();
        }
    }
}