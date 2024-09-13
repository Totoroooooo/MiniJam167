using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Player
{
	public class PlayerBody : MonoBehaviour, IHittable
	{
        [SerializeField] private Rigidbody2D _rigidBody;
		[Space]
		[SerializeField] private float _moveSpeed;
		[SerializeField] private int _lifePoint;

		public float Shield => 0;
		public float DamageMultiplier => 1;

		private Vector2 _playerMovementInput;

        private void Start()
        {
            PlayerInput.PlayerMoved += OnPlayerMoved;
        }

        private void OnDestroy()
        {
            PlayerInput.PlayerMoved -= OnPlayerMoved;
        }

        private void OnPlayerMoved(Vector2 position)
        {
            _playerMovementInput = position;
        }

        private void FixedUpdate()
		{
            _rigidBody.velocity = _playerMovementInput * _moveSpeed;
        }

        public void SetMainBody(PlayerBody mainBody)
        {
            _mainBody = mainBody;
        }

        public void OnHit(IHitter hitter)
		{
            float damage = this.GetHitDamage(hitter);
			//_mainBody.TakeDamage();
		}
	}
}
