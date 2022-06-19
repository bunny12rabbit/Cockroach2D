using System;
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

        public void Dispose()
        {
            _disposables.Clear();
        }

        public EnemyCharacterView Spawn(EnemyCharacterView prefab, CharacterData characterData)
        {
            var randomIndex = Random.Range(0, _params.SpawnPositions.Length);
            var spawnPosition = _params.SpawnPositions[randomIndex];
            var targetPosition = _params.TargetPosition;
            var characterParams = new EnemyCharacterView.Params(_params.DangerDetector, characterData, _params.Boundaries, targetPosition);

            var enemyCharacter = _params.PrefabPool.Get(prefab, characterParams).AddTo(_disposables);
            enemyCharacter.transform.position = spawnPosition;
            return enemyCharacter;
        }
    }
}