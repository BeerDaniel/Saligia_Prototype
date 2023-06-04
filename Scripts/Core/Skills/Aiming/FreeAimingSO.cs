using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newFreeAimingSO", menuName = "Saligia/Skills/Aiming/FreeAiming")]
    public class FreeAimingSO : AimingSO
    {
        [SerializeField] private GameObject _aimDecalPrefab;
        [SerializeField] private string _overlayTag;
        [SerializeField] private RectTransform _pointerPrefab;
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private LayerMask _aimingLayers;

        private RectTransform _rectTransform;
        private GameObject _aimDecal;
        private Vector2 _movement;

        protected override bool AimingStart(PlayerEntity playerAimer)
        {
            playerAimer.MovementComponent.BlockRotation(true);
            var screenPos = Camera.main.WorldToScreenPoint(playerAimer.transform.position);
            _rectTransform = Instantiate(_pointerPrefab, screenPos, Quaternion.identity, playerAimer.AbilityTargetingCanvas.transform);
            _aimDecal = Instantiate(_aimDecalPrefab, playerAimer.transform.position, _aimDecalPrefab.transform.rotation);
            return false;
        }

        protected override void AimingUpdate(PlayerEntity playerAimer)
        {
            var move = _movement * _scrollSpeed * Time.deltaTime;
            var newPos = _rectTransform.anchoredPosition + move;
            var newPosScreenClamped = new Vector2(Mathf.Clamp(newPos.x, 0, Screen.width), Mathf.Clamp(newPos.y, 0, Screen.height));

            Ray ray = Camera.main.ScreenPointToRay(newPosScreenClamped);


            if (Physics.Raycast(ray, out RaycastHit hit, 50, _aimingLayers.value))
            {
                var direction = hit.point - playerAimer.transform.position;
                var clampedDirection = direction.normalized * Mathf.Min(direction.magnitude, playerAimer.MaxActionRange);

                var point = playerAimer.transform.position + clampedDirection;

                direction.y = 0;
                playerAimer.transform.forward = direction;
                _rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, point);
                _aimDecal.transform.position = point;
            }
        }

        protected override TargetData AimingEnd(PlayerEntity aimer)
        {
            
            var target = new TargetData(_aimDecal.transform.position);
            Destroy(_aimDecal.gameObject);
            Destroy(_rectTransform.gameObject);
            aimer.MovementComponent.BlockRotation(false);
            return target;
        }

        public override void OnMove(CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }
    }
}




