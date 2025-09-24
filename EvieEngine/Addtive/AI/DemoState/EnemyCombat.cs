// EnemyCombat.cs
using UnityEngine;

namespace EvieEngine.AI.Demo
{
    public class EnemyCombat : MonoBehaviour
    {
        public int damage = 10;

        private void OnEnemyAttack()
        {
            // простая реализация: проверим игрока в радиусе
            Collider[] hits = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Player"));
            foreach (var c in hits)
            {
                var hp = c.GetComponent<HPSystem>();
                if (hp != null)
                {
                    
                }
            }
        }
    }
}