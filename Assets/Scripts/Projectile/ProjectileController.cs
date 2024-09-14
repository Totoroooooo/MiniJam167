using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public abstract class ProjectileController : MonoBehaviour
    {
        protected Pool<ProjectileController> _pool;

        public void Spawn(Vector2 position, Vector2 direction, Pool<ProjectileController> pool)
        {
            _pool = pool;
            OnSpawn(position, direction);
        }
        
        protected abstract void OnSpawn(Vector2 position, Vector2 direction);
    }
}