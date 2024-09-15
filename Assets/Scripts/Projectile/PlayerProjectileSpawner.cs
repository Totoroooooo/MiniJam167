using FMODUnity;
using MiniJam167.Player.Skills;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class PlayerProjectileSpawner : ProjectileSpawner
    {
        [SerializeField] private SkillProjectile _skillProjectile;

        private void Start()
        {
            _skillProjectile.PlayerShot += SpawnProjectile;
        }

        private void OnDestroy()
        {
            _skillProjectile.PlayerShot -= SpawnProjectile;
        }
    }
}