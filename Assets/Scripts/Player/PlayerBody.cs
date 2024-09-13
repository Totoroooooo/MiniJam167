using MiniJam67.HitSystem;
using UnityEngine;

namespace MiniJam67.Player
{
	public class PlayerBody : MonoBehaviour, IHittable
	{
        [SerializeField] private Rigidbody2D _rigidBody;
		[Space]
		[SerializeField] private float _moveSpeed;
		[SerializeField] private int _lifePoint;

		private Vector2 _playerMovementInput;
		private PlayerBody _mainBody;

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
			//_mainBody.TakeDamage();
		}

		//private void TakeDamage(int damageAmount)
		//{
		//	_lifePoint = _lifePoint - damageAmount;

  //      }
	}
}
