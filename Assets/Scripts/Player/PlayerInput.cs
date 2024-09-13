using MiniJam67.HitSystem;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void PlayerInputEvent(Vector2 position, List<IHittable> hittables);
    public delegate void PlayerAxisEvent(Vector2 position);
    public static event PlayerInputEvent PlayerClicked;
    public static event PlayerAxisEvent PlayerMoved;

    private void Update()
    {
        Vector2 playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        PlayerMoved?.Invoke(playerMovementInput.normalized);
    }
}
