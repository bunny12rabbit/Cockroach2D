using Characters;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Settings
{
    [CreateAssetMenu(fileName = nameof(GameDesignSettings), menuName = "Settings/Game Design Settings")]
    public class GameDesignSettings : ScriptableObject
    {
        public const string AssetPath = "Assets/GameDesignSettings/GameDesignSettings.asset";

        private const string EnemySettingsLabel = "Enemy Settings";

        [SerializeField, BoxGroup(EnemySettingsLabel)]
        private CharacterData _characterData = CharacterData.Default;
        public CharacterData CharacterData => _characterData;

        [SerializeField, BoxGroup(EnemySettingsLabel)]
        private int _enemiesAmount = 1;

        public int EnemiesAmount
        {
            get => _enemiesAmount;
            set => _enemiesAmount = value;
        }

        public void SetCharacterData(CharacterData data) => _characterData = data;
    }
}