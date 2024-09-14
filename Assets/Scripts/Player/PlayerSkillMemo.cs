using UnityEngine;

namespace MiniJam167.Player
{
    public abstract class PlayerSkillMemo : ScriptableObject
    {
        public void Subscribe(Vector2 position, Quaternion rotation)
        {
            PlayerInput.PlayerKeyDown += OnPlayerkeyDown;
            PlayerInput.PlayerKeyDown += OnPlayerkeyUp;
            OnSubcribe(position, rotation);
        }

        public void Unsubscribe()
        {
            PlayerInput.PlayerKeyDown -= OnPlayerkeyDown;
            PlayerInput.PlayerKeyDown -= OnPlayerkeyUp;
        }

        protected abstract void OnSubcribe(Vector2 position, Quaternion rotation);
        protected abstract void OnPlayerkeyUp(Vector2 position, Quaternion rotation);
        protected abstract void OnPlayerkeyDown(Vector2 position, Quaternion rotation);
    }
}