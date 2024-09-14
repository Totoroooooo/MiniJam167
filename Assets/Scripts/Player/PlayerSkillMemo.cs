using UnityEngine;

namespace MiniJam167.Player
{
    public abstract class PlayerSkillMemo : ScriptableObject
    {
        public void Subscribe(Vector2 position, Quaternion rotation)
        {
            PlayerInput.PlayerKeyDown += OnPlayerkeyDown;
            PlayerInput.PlayerKeyDown += OnPlayerkeyUp;
            PlayerInput.PlayerKeyPressed += OnPlayerKey;
            OnSubcribe(position, rotation);
        }

        public void Unsubscribe(Vector2 position, Quaternion rotation)
        {
            PlayerInput.PlayerKeyDown -= OnPlayerkeyDown;
            PlayerInput.PlayerKeyDown -= OnPlayerkeyUp;
            PlayerInput.PlayerKeyPressed -= OnPlayerKey;
            OnUnsubscribe(position, rotation);
        }

        protected abstract void OnSubcribe(Vector2 position, Quaternion rotation);
        protected abstract void OnUnsubscribe(Vector2 vector2, Quaternion rotation);
        protected abstract void OnPlayerkeyUp(Vector2 position, Quaternion rotation);
        protected abstract void OnPlayerkeyDown(Vector2 position, Quaternion rotation);
        protected abstract void OnPlayerKey(Vector2 position, Quaternion rotation, float deltaTime);
    }
}