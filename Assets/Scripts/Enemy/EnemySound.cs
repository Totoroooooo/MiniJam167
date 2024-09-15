using MiniJam167.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam167
{
    public class EnemySound : MonoBehaviour
    {
        [SerializeField] FMODUnity.StudioEventEmitter _eventEmitter;
        [SerializeField] EnemyBody _enemyBody;
        [SerializeField] PlayerSkillDefault __playerSkill;


        private void Start()
        {
            _enemyBody.Died += PlaySound;
            __playerSkill.Launched += PlaySound;
        }

        private void OnDestroy()
        {
            _enemyBody.Died -= PlaySound;
        }

        public void PlaySound()
        {
            _eventEmitter.Play();
        }
         
    }

}
