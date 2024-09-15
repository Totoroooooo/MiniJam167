using UnityEngine;

namespace MiniJam167.Player.Skills
{
    [CreateAssetMenu(fileName = "Mm_Projectile_", menuName = "MiniJam167/Skills/Projectile")]
    public class SkillDefaultProjectile : SkillProjectile
    {
        [SerializeField] private float _bulletPerSecond;

        private float _timer;
        private float _delay;

        protected override void OnPlayerKey(Vector2 position, Quaternion rotation, float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < _delay)
                return;

            Shoot(position, rotation);
            _timer -= _delay;
        }

        protected override void OnPlayerkeyDown(Vector2 position, Quaternion rotation)
        {
        }

        protected override void OnPlayerkeyUp(Vector2 position, Quaternion rotation)
        {
        }

        protected override void OnSubcribe(Vector2 position, Quaternion rotation)
        {
            _delay = 1f / _bulletPerSecond;
            _timer = 0;
        }

        protected override void OnUnsubscribe(Vector2 vector2, Quaternion rotation)
        {
        }


    }
}
