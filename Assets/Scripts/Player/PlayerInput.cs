using UnityEngine;

namespace MiniJam167.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public delegate void PlayerInputEvent(Vector2 position, Quaternion rotation);
        public delegate void PlayerAxisEvent(Vector2 position);
        public delegate void PlayerTimeInputEvent(Vector2 position, Quaternion rotation, float deltaTime);

        public static event PlayerInputEvent PlayerKeyDown;
        public static event PlayerInputEvent PlayerKeyUp;
        public static event PlayerAxisEvent PlayerMoved;
        public static event PlayerTimeInputEvent PlayerKeyPressed;

        private void Update()
        {
            Vector3 playerMovementInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPosition = transform.position;
            Vector3 direction = playerMovementInput - playerPosition;
            direction.z = 0;
            PlayerMoved?.Invoke(direction);

            if (Input.GetKeyDown(KeyCode.Mouse0))
                PlayerKeyDown?.Invoke(transform.position, transform.rotation);
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                PlayerKeyUp?.Invoke(transform.position, transform.rotation);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                PlayerKeyPressed?.Invoke(transform.position, transform.rotation, Time.deltaTime);
            }
        }
    }
}