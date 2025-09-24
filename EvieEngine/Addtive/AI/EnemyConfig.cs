using UnityEngine;

namespace EvieEngine.AI
{
    [CreateAssetMenu(menuName = "EvieEngine/AI/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        public float walkSpeed = 3.5f;
        public float runSpeed = 6f;
        public float visionRadius = 12f;
        public float visionAngle = 120f;
        public float hearingRadius = 6f;
        public float attackRange = 2f;
        public float attackCooldown = 1.2f;
        public float chaseLoseTime = 3f;
    }
}