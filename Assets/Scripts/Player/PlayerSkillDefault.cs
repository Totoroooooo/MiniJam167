using MiniJam167.Player;
using MiniJam167.Player.Skills;
using MiniJam167.Projectile;
using MiniJam167.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MiniJam167.Enemy.EnemyBody;

namespace MiniJam167
{
    public class PlayerSkillDefault : Missile
    {

        public delegate void ProjectileSpawner(Vector2 position, Quaternion rotation);
        public event ProjectileSpawner SpawnProjectile;
        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            SpawnProjectile?.Invoke(position, rotation);
        }
    }
}
