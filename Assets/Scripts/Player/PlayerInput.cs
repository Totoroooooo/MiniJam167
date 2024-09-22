using System.Collections.Generic;
using MiniJam167.HitSystem;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private GameObject[] _targets;
        [SerializeField] private GameObject _target;
        [SerializeField] private TransformRadio _targetRadio;
        
        private ITargetable GetTarget => _targetList[_currentTargetIndex];
        private GameObject GetTargetObject => _targets[_currentTargetIndex];
        
        private ITargetable[] _targetList;
        private int _currentTargetIndex;
        private Camera _camera;
        
        public delegate void PlayerInputEvent(Vector2 position, Quaternion rotation);
        public delegate void PlayerAxisEvent(Vector2 position);
        public delegate void PlayerTimeInputEvent(Vector2 position, Quaternion rotation, float deltaTime);

        public static event PlayerInputEvent PlayerKeyDown;
        public static event PlayerInputEvent PlayerKeyUp;
        public static event PlayerAxisEvent PlayerMoved;
        public static event PlayerTimeInputEvent PlayerKeyPressed;

        private void Start()
        {
            _camera = Camera.main;
            List<ITargetable> targets = new List<ITargetable>();
            foreach (GameObject target in _targets)
                if (target.TryGetComponent(out ITargetable targetComponent))
                    targets.Add(targetComponent);
            _targetList = targets.ToArray();
            UpdateTarget(true);
        }

        private void OnDestroy()
        {
            if (GetTarget != null)
                GetTarget.TargetableChanged -= UpdateTarget;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                UpdateTarget(false);
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
                UpdateTarget(true);
            
            Vector3 playerMovementInput = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPosition = transform.position;
            Vector3 direction = playerMovementInput - playerPosition;
            direction.z = 0;
            PlayerMoved?.Invoke(direction);
            
            Vector3 targetPosition = _target.transform.position = GetTarget.Position;
            Vector3 targetDirection = targetPosition - playerPosition;
            targetDirection.z = 0;
            Vector3 transformDirection = transform.up;
            transform.up = targetDirection;
            
            PlayerKeyPressed?.Invoke(transform.position, transform.rotation, Time.deltaTime);
            
            transform.up = transformDirection;
        }

        private void UpdateTarget(bool right)
        {
            GetTarget.TargetableChanged -= UpdateTarget;
            
            int modifier = right ? 1 : -1;
            do
            {
                int length = _targetList.Length;
                _currentTargetIndex = (_currentTargetIndex + length + modifier) % length;
            } while (!GetTarget.Targetable);
            
            GetTarget.TargetableChanged += UpdateTarget;
            _targetRadio.Value = GetTarget.Transform;
        }
    }
}