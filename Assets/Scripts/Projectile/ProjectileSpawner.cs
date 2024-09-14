using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private PoolHolder<ProjectileController> _projectilePool;

        public void SpawnProjectile(Vector2 position, Quaternion rotation)
        {
            
        }
    }
}