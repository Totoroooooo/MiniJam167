using FMODUnity;
using MiniJam167.Projectile;
using MiniJam167.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam167
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundCaller _soundCaller;
        [SerializeField] private StudioEventEmitter _emitter;

        public PlayerProjectileSpawner PlayerProjectileSpawner;

        private void Start()
        {
            _soundCaller.Init(_emitter);
        }

        private void Clear()
        {
            _soundCaller.Clear();
        }
    }
}
