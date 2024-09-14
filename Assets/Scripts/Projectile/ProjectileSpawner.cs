using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private PoolHolder<ProjectileController> _projectilePool;

        public void SpawnProjectile(Vector2 position, Quaternion rotation)
        {
            var projectil = _projectilePool.Pool.Get(transform, position, rotation);
            projectil.Spawn(position, rotation, _projectilePool.Pool);
        }
    }
}