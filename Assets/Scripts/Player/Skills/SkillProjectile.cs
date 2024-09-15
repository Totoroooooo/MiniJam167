using UnityEngine;

namespace MiniJam167.Player.Skills
{
    public abstract class SkillProjectile : PlayerSkillMemo
    {
        public event PlayerInput.PlayerInputEvent PlayerShot;

        protected void Shoot(Vector2 position, Quaternion rotation) 
        {
            PlayerShot?.Invoke(position, rotation);
        }
    }
}