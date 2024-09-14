namespace MiniJam167.HitSystem
{
	using UnityEngine;

	public static class HitExtensions
	{
		public static float GetHitDamage(this IHittable hittable, IHitter hitter)
		{
			float shield = Mathf.Max(0, hittable.Shield - hitter.Lethality);
			float damage = Mathf.Max(0, hitter.Damage - shield) * hittable.DamageMultiplier;
			return damage;
		}
	}
}