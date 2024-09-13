using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Player
{
	public class PlayerBody : MonoBehaviour, IHittable
	{
		[SerializeField] private float _moveSpeed;

		public float Shield => 0;
		public float DamageMultiplier => 0;

		public void OnHit(IHitter hitter)
		{
			
		}

		private void Move(Vector2 input)
		{
			
		}
	}
}
