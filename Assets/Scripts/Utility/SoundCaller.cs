using System;
using FMODUnity;
using UnityEngine;

namespace MiniJam167.Utility
{
    [Serializable]
    public class SoundCaller
    {
        public ActionRadio CallbackRadio;

        private StudioEventEmitter _emitter;

        public void Init(StudioEventEmitter emitter)
        {
            _emitter = emitter;
            CallbackRadio.Callback += CallSound;
        }

        public void Clear()
        {
            CallbackRadio.Callback -= CallSound;
        }

        private void CallSound()
        {
            _emitter.Play();
        }
    }
}
