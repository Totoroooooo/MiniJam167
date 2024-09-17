using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class FixedHomingMissile : DefaultMissile
    {
        [SerializeField] private float _rotationSpeed = 1f;
        [SerializeField] private TransformRadio _target;

        private void Update()
        {
            Vector2 direction = (_target.Value.transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.up, direction, transform.forward);
            angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), _rotationSpeed * Time.deltaTime);
            transform.up = Quaternion.Euler(0f, 0, angle) * transform.up;
            _rb.velocity = transform.up * _speed;
        }
    }
}