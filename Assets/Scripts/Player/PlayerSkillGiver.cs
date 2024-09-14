using System;
using MiniJam167.Projectile;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniJam167.Player
{
    public class PlayerSkillGiver : ProjectileController
    {
        [SerializeField] private PlayerSkillMemo _skill;
        [Space]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        private void OnDestroy()
        {
            _skill.Unsubscribe(Vector2.zero, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerBody body))
                return;
            
            Transform bodyTransform = body.transform;
            _skill.Subscribe(bodyTransform.position, bodyTransform.rotation);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerBody body))
                return;
            Transform bodyTransform = body.transform;
            _skill.Unsubscribe(bodyTransform.position, bodyTransform.rotation);
        }

        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            
        }
    }
}
