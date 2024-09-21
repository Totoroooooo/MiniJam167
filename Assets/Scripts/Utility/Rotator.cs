using UnityEngine;

namespace MiniJam167.Utility
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 180;

        private void Update()
        {
            transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }
}