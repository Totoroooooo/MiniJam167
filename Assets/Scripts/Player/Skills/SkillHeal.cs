using MiniJam167.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniJam167.Player.Skills
{
    [CreateAssetMenu(fileName = "Mm_SkillHeal", menuName = "MiniJam167/Skills/Heal")]
    public class SkillHeal : PlayerSkillMemo
    {
        [SerializeField] private PlayerRadio _playerRadio;
        [FormerlySerializedAs("_bulletPerSecond")] [SerializeField] private float _fireRate = 2f;
        [SerializeField] private float _healValue = 5f;

        private float _timer;
        private float _delay;
        
        protected override void OnSubcribe(Vector2 position, Quaternion rotation)
        {
            _delay = 1f / _fireRate;
            _timer = 0;
        }

        protected override void OnUnsubscribe(Vector2 vector2, Quaternion rotation)
        {
            
        }

        protected override void OnPlayerkeyUp(Vector2 position, Quaternion rotation)
        {
            
        }

        protected override void OnPlayerkeyDown(Vector2 position, Quaternion rotation)
        {
            
        }

        protected override void OnPlayerKey(Vector2 position, Quaternion rotation, float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < _delay)
                return;

            Heal();
            _timer -= _delay;
        }

        private void Heal()
        {
            _playerRadio.Value.Heal(_healValue);
        }
    }
}