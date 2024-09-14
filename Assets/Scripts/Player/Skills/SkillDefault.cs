using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam167.Player.Skills
{
    public class SkillDefault : PlayerSkillMemo
    {
        [SerializeField] private float _bulletPerSecond;


        private bool _isPressingKey;
        private float _timer;

        public event PlayerInput.PlayerInputEvent PlayerShot;

        protected override void OnPlayerKey(Vector2 position, Quaternion rotation, float deltaTime)
        {
            _timer += deltaTime;
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

        private void Shoot(Vector2 position, Quaternion rotation) 
        {
            PlayerShot?.Invoke(position, rotation);
        }


    }
}
