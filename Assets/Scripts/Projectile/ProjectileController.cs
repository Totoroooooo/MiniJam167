using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public abstract class ProjectileController : MonoBehaviour
    {
        [Header("Bounds")]
        [SerializeField] private VectorRadio _topRightRadio;
        [SerializeField] private VectorRadio _bottomLeftRadio;

        [Header("Scale")]
        [SerializeField] private bool _setScaleOnSpawn = true;
        [SerializeField] private float _lossyScale;
        
        private Pool<ProjectileController> _pool;
        public ActionRadio SoundCaller;

        private Vector2 _distance;

        private void Awake()
        {
            _distance = _topRightRadio.Value - _bottomLeftRadio.Value;
            _distance.x = Mathf.Abs(_distance.x);
            _distance.y = Mathf.Abs(_distance.y);
        }

        protected virtual void Update()
        {
            Vector3 position = transform.position;
            
            if (position.x > _topRightRadio.Value.x
                || position.x < _bottomLeftRadio.Value.x
                || position.y > _topRightRadio.Value.y
                || position.y < _bottomLeftRadio.Value.y)
                Release();
        }
        
        public void Spawn(Vector2 position, Quaternion rotation, Pool<ProjectileController> pool)
        {
            _pool = pool;
            if (_setScaleOnSpawn)
                transform.localScale = GetLossyScale();
            OnSpawn(position, rotation);
            SoundCaller?.Value?.Invoke();
        }
        
        protected abstract void OnSpawn(Vector2 position, Quaternion rotation);

        protected void Release()
        {
            OnRelease();
            _pool.Release(this);
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