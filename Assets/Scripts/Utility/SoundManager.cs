using UnityEngine;

namespace MiniJam167.Utility
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundCaller[] _soundCallers;

        private void Start()
        {
            foreach (SoundCaller caller in _soundCallers)
                caller.Init();
        }

        private void OnDestroy()
        {
            foreach (SoundCaller soundInfo in _soundCallers)
                soundInfo.Clear();
        }
    }
}
