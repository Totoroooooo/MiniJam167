using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public abstract class ProjectileController : MonoBehaviour
    {
        [Header("Scale")]
        [SerializeField] private bool _setScaleOnSpawn = true;
        [SerializeField] private float _lossyScale;
        
        private Pool<ProjectileController> _pool;
        public ActionRadio SoundCaller;

        public void Spawn(Vector2 position, Quaternion rotation, Pool<ProjectileController> pool)
        {
            _pool = pool;
            if (_setScaleOnSpawn)
                transform.localScale = GetLossyScale();
            OnSpawn(position, rotation);
            SoundCaller?.Callback?.Invoke();
        }
        
        protected abstract void OnSpawn(Vector2 position, Quaternion rotation);

        protected void Release()
        {
            _pool.Release(this);
            OnRelease();
        }
        
        protected virtual void OnRelease() { }

        protected Vector3 GetLossyScale()
        {
            transform.localScale = Vector3.one;
            Vector3 lossyScale = transform.lossyScale;
            return new Vector3(
               _lossyScale / lossyScale.x,
               _lossyScale / lossyScale.y,
               _lossyScale / lossyScale.z);
        }
    }
}