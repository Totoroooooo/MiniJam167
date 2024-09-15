using FMODUnity;
using MiniJam167.Enemy;
using UnityEngine;

namespace MiniJam167
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] StudioEventEmitter _eventEmitter;
        [SerializeField] EnemyBody _enemyBody;

        private void Start()
        {
            _enemyBody.Died += PlaySound;
        }

        private void OnDestroy()
        {
            _enemyBody.Died -= PlaySound;
        }

        private void PlaySound()
        {
            _eventEmitter?.Play();
        }
         
    }

}
