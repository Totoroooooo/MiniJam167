using System;
using FMODUnity;
using UnityEngine;

namespace MiniJam167.Utility
{
    [Serializable]
    public class SoundCaller
    {
        public ActionRadio CallbackRadio;
        public StudioEventEmitter Emitter;

        public void Init()
        {
            CallbackRadio.Value += CallSound;
        }

        public void Clear()
        {
            CallbackRadio.Value -= CallSound;
        }

        private void CallSound()
        {
            Emitter.Play();
        }
    }
}
