using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public abstract class ProjectileController : MonoBehaviour
    {
        private Pool<ProjectileController> _pool;
        public ActionRadio SoundCaller;

        public void Spawn(Vector2 position, Quaternion rotation, Pool<ProjectileController> pool)
        {
            SoundCaller?.Callback?.Invoke();
            _pool = pool;
            OnSpawn(position, rotation);
        }
        
        protected abstract void OnSpawn(Vector2 position, Quaternion rotation);

        protected void Release()
        {
            _pool.Release(this);
            OnRelease();
        }
        
        protected virtual void OnRelease() { }
    }
}