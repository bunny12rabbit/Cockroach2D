using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Core;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class CharacterSpawner : IDisposable
    {
        public readonly struct Params
        {
            public readonly PrefabPool PrefabPool;

            public readonly IDangerDetector DangerDetector;

            public readonly Boundaries Boundaries;

            public readonly Vector3 TargetPosition;
            public readonly Vector3[] SpawnPositions;


            public Params(PrefabPool prefabPool, IDangerDetector dangerDetector, Boundaries boundaries, Vector3 targetPosition,
                params Vector3[] spawnPositions)
            {
                PrefabPool = prefabPool;
                DangerDetector = dangerDetector;
                Boundaries = boundaries;
                TargetPosition = targetPosition;
                SpawnPositions = spawnPositions;
            }
        }

        public CharacterSpawner(Params inputParams)
        {
            _params = inputParams;
        }

        private readonly Params _params;

        private readonly CompositeDisposable _disposables = new();

        private int _previousSpawnPositionIndex = -1;

        public void Dispose()
        {
            _disposables.Clear();
        }

        public ICollection<EnemyCharacterView> SpawnEnemy(EnemyCharacterView prefab, CharacterData characterData, int amount = 1)
        {
            var spawnedEnemies = new List<EnemyCharacterView>();

            for (var i = 0; i < amount; i++)
            {
                var spawnPosition = GetRandomSpawnPoint();
                var targetPosition = _params.TargetPosition;
                var characterParams = new EnemyCharacterView.Params(_params.DangerDetector, characterData, _params.Boundaries, targetPosition);

                var enemyCharacter = _params.PrefabPool.Get(prefab, characterParams).AddTo(_disposables);
                enemyCharacter.transform.position = spawnPosition;
                spawnedEnemies.Add(enemyCharacter);
            }

            return spawnedEnemies;
        }

        private Vector3 GetRandomSpawnPoint()
        {
            if (_params.SpawnPositions.Length == 1)
                return _params.SpawnPositions.First();

            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, _params.SpawnPositions.Length);
            }
            while (randomIndex == _previousSpawnPositionIndex);

            _previousSpawnPositionIndex = randomIndex;

            return _params.SpawnPositions[randomIndex];
        }
    }
}