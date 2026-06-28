using UnityEngine;

namespace Shooter.Characters
{
    [CreateAssetMenu(menuName = "Shooter/Character Stats")]
    public sealed class CharacterStatsDefinition : ScriptableObject
    {
        [SerializeField] private string _displayName = "Character";
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _armor;
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _attackRange = 1.6f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _attackCooldown = 1f;

        public string DisplayName => _displayName;
        public float MaxHealth => _maxHealth;
        public float Armor => _armor;
        public float MoveSpeed => _moveSpeed;
        public float AttackRange => _attackRange;
        public float AttackDamage => _attackDamage;
        public float AttackCooldown => _attackCooldown;

        public static CharacterStatsDefinition CreateRuntime(
            string displayName,
            float maxHealth,
            float armor,
            float moveSpeed,
            float attackRange,
            float attackDamage,
            float attackCooldown)
        {
            var definition = CreateInstance<CharacterStatsDefinition>();
            definition._displayName = displayName;
            definition._maxHealth = maxHealth;
            definition._armor = armor;
            definition._moveSpeed = moveSpeed;
            definition._attackRange = attackRange;
            definition._attackDamage = attackDamage;
            definition._attackCooldown = attackCooldown;
            return definition;
        }
    }
}
