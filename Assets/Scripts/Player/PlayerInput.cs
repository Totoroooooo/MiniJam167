using MiniJam167.HitSystem;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void PlayerInputEvent(Vector2 position, List<IHittable> hittables);
    public static event PlayerInputEvent PlayerClicked;


}
