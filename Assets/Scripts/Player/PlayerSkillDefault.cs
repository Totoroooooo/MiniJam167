using MiniJam167.Projectile;
using UnityEngine;

namespace MiniJam167
{
    public class PlayerSkillDefault : Missile
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed = 3;

        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            _rb.velocity = rotation * Vector2.up * _speed;
        }

    }
}
