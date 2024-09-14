using UnityEngine;

namespace MiniJam167.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public delegate void PlayerInputEvent(Vector2 position, Quaternion rotation);
        public delegate void PlayerAxisEvent(Vector2 position);

        public static event PlayerInputEvent PlayerKeyDown;
        public static event PlayerInputEvent PlayerKeyUp;
        public static event PlayerAxisEvent PlayerMoved;

        private void Update()
        {
            Vector2 playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            PlayerMoved?.Invoke(playerMovementInput.normalized);

            if (Input.GetKeyDown(KeyCode.Space))
                PlayerKeyDown?.Invoke(transform.position, transform.rotation);
            else if (Input.GetKeyUp(KeyCode.Space))
                PlayerKeyUp?.Invoke(transform.position, transform.rotation);
        }
    }
}