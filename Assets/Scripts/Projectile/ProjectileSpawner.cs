using MiniJam167.Utility;
using System;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private PoolHolder<ProjectileController> _projectilePool;

        public void SpawnProjectile(Vector2 position, Quaternion rotation)
        {
            ProjectileController projectile = _projectilePool.Pool.Get(transform, position, rotation);
            projectile.Spawn(position, rotation, _projectilePool.Pool);
        }
    }
}