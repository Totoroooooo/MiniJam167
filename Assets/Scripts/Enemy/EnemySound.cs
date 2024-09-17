using UnityEngine;

namespace MiniJam167.Enemy
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] private FMODUnity.StudioEventEmitter _eventEmitter;
        [SerializeField] private EnemyBody _enemyBody;

        private void Start()
        {
            _enemyBody.Hitted += PlaySound;
            _enemyBody.Died += PlaySound;
        }

        private void OnDestroy()
        {
            _enemyBody.Hitted -= PlaySound;
            _enemyBody.Died -= PlaySound;
        }

        public void PlaySound()
        {
            _eventEmitter.Play();
        }
         
    }

}
