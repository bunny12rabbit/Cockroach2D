using Characters;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Settings
{
    [CreateAssetMenu(fileName = nameof(GameDesignSettings), menuName = "Settings/Game Design Settings")]
    public class GameDesignSettings : ScriptableObject
    {
        public const string AssetPath = "Assets/GameDesignSettings/GameDesignSettings.asset";

        private const string EnemySettingsLabel = "Enemy Settings Label";

        [SerializeField, BoxGroup(EnemySettingsLabel)]
        private CharacterData _characterData = CharacterData.Default;
        public CharacterData CharacterData => _characterData;
    }
}