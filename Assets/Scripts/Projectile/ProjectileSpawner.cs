using MiniJam167.Utility;
using System;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private PoolHolder<ProjectileController> _projectilePool;

        public void SpawnProjectile(Vector2 position, Quaternion rotation, Transform parent)
        {
            ProjectileController projectile = _projectilePool.Pool.Get(parent ?? transform, position, rotation);
            projectile.Spawn(position, rotation, _projectilePool.Pool);
        }
        
        public void SpawnProjectile(Vector2 position, Quaternion rotation)
        {
            SpawnProjectile(position, rotation, transform);
        }
        
    }
}