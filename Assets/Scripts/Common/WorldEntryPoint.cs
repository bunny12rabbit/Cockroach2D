using Characters;
using Common.Settings;
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

            _dangerDetector.Init(new DangerDetector.Params(_camera));

            var screenBorders = new Boundaries(_camera, Screen.width, Screen.height);

            var characterSpawnerParams =
                new CharacterSpawner.Params(_prefabPool, _dangerDetector, screenBorders, _finishPoint.position, _startPoint.position);

            _characterSpawner = new CharacterSpawner(characterSpawnerParams).AddTo(_disposables);
            var enemyCharacterView = _characterSpawner.Spawn(_enemyCharacterViewPrefab, _gameDesignSettings.CharacterData);
            enemyCharacterView.TargetReached.Subscribe(OnTargetReached).AddTo(_disposables);
        }

        private void OnTargetReached(Unit _)
        {
            Log.Info($"GameOver!".Colorize(Color.green));
        }

        private void OnDisable()
        {
            _disposables.Clear();
        }
    }
}