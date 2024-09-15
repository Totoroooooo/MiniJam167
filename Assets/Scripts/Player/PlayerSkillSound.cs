using UnityEngine;

namespace MiniJam167.Sound
{
    public class PlayerSkillSound : MonoBehaviour
    {
        [SerializeField] FMODUnity.StudioEventEmitter _eventEmitter;
        [SerializeField] PlayerSkillDefault _playerSkill;

        private void Start()
        {
            _playerSkill.Launched += PlaySound;
        }

        private void OnDestroy()
        {
            _playerSkill.Launched -= PlaySound;
        }

        public void PlaySound()
        {
            _eventEmitter.Play();
        }
    }
}
