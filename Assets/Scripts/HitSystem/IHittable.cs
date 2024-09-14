namespace MiniJam167.HitSystem
{
	public interface IHittable
    {
        public float Shield { get; }
        public float DamageMultiplier { get; }
        
        public void OnHit(IHitter hitter);
    }
}
