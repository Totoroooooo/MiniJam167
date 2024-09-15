using UnityEngine;

namespace MiniJam167.Player.Skills
{
    [CreateAssetMenu(fileName = "Mm_Projectile_", menuName = "MiniJam167/Skills/Projectile")]
    public class SkillDefault : SkillProjectile
    {
        [SerializeField] private float _bulletPerSecond;

        private bool _isPressingKey;
        private float _timer;

        protected override void OnPlayerKey(Vector2 position, Quaternion rotation, float deltaTime)
        {
            _timer += deltaTime;
            Debug.Log(_isPressingKey);
            Debug.Log(_timer);
            if (!_isPressingKey || _timer < 1 / _bulletPerSecond)
                return;

            Shoot(position, rotation);
            _timer = 0;
        }

        protected override void OnPlayerkeyDown(Vector2 position, Quaternion rotation)
        {
            _isPressingKey = true;
        }

        protected override void OnPlayerkeyUp(Vector2 position, Quaternion rotation)
        {
            _isPressingKey = false;
        }

        protected override void OnSubcribe(Vector2 position, Quaternion rotation)
        {
        }

        protected override void OnUnsubscribe(Vector2 vector2, Quaternion rotation)
        {
        }


    }
}
