using MiniJam67.HitSystem;
using UnityEngine;

namespace MiniJam67.Player
{
	public class PlayerBody : MonoBehaviour, IHittable
	{
		[SerializeField] private float _moveSpeed;

		public void OnHit(IHitter hitter)
		{
			
		}

		private void Move(Vector2 input)
		{
			
		}
	}
}
