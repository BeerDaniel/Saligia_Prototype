using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newObjecktLockedAimingSO", menuName = "Saligia/Skills/Aiming/ObjectLockedAiming")]
    public class ObjectLockedSO : AimingSO
    {
        [SerializeField] private GameObject _markPrefab;
        [SerializeField] private Vector3 _markOffset;

        private float _switch = 0f;
        private Entity _currentTarget;
        private GameObject _mark;
        protected override bool AimingStart(PlayerEntity playerAimer)
        {
            playerAimer.MovementComponent.BlockRotation(true);
            if (playerAimer.TargetingComponent.FindTarget(playerAimer.MaxActionRange))
            {
                _mark = Instantiate(_markPrefab);
                _currentTarget = playerAimer.TargetingComponent.Target;
                return true;
            }
            onAbort?.Invoke();
            return false;
        }

        protected override void AimingUpdate(PlayerEntity playerAimer)
        {
            if (_switch != 0f)
            {
                if (playerAimer.TargetingComponent.FindNextTarget(playerAimer.MaxActionRange, _switch))
                {
                    _switch = 0f;
                    _currentTarget = playerAimer.TargetingComponent.Target;
                }
            }
            if (_currentTarget)
            {
                var additionalOffset = Vector3.zero;
                var collider = _currentTarget.GetComponent<Collider>();
                if (collider)
                    additionalOffset = new Vector3(0, collider.bounds.size.y, 0);
                _mark.transform.position = _currentTarget.transform.position + additionalOffset + _markOffset;
                playerAimer.transform.LookAt(_currentTarget.transform.position);
            }
            else
            {
                onAbort?.Invoke();
            }

        }

        protected override TargetData AimingEnd(PlayerEntity playerAimer)
        {
            Destroy(_mark);
            playerAimer.MovementComponent.BlockRotation(false);

            return new TargetData(_currentTarget ? _currentTarget.gameObject : null);
        }

        public override void OnMove(CallbackContext context)
        {
            if (!context.performed)
                return;
            _switch = context.ReadValue<Vector2>().x;
        }


    }
}




