using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Player;
using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace SuspiciousGames.Saligia.Core.Skills
{

    public abstract class AimingSO : ScriptableObject
    {
        [SerializeField] private string _aimingAnimationTrigger;

        [HideInInspector] public UnityEvent onAbort;
        public bool StartAiming(PlayerEntity playerAimer)
        {
            playerAimer.MovementComponent.BlockMovement(true);
            playerAimer.Animator.SetTrigger(_aimingAnimationTrigger);
            return AimingStart(playerAimer);
        }
        protected abstract bool AimingStart(PlayerEntity playerAimer);
        public void UpdateAiming(PlayerEntity playerAimer)
        {
            AimingUpdate(playerAimer);
        }
        protected abstract void AimingUpdate(PlayerEntity playerAimer);
        public void EndAiming(PlayerEntity playerAimer)
        {
            playerAimer.MovementComponent.BlockMovement(false);
            playerAimer.CastComponent.TargetData = AimingEnd(playerAimer);
        }
        protected abstract TargetData AimingEnd(PlayerEntity playerAimer);

        public virtual void OnMove(CallbackContext context) { }
    }
}




