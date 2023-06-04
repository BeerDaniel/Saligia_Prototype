using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newTeleportSkill", menuName = "Saligia/Skills/Movement/Teleport")]
    public class Teleport : BaseSkill
    {
        [SerializeField] private float _teleportRange;

        public override void AnimationTriggeredLogic()
        {
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
                CasterEntity.MovementComponent.Warp(CasterEntity.transform.position +
                    CasterEntity.transform.forward * _teleportRange);
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }

        public override void CleanUp()
        {
            CasterEntity.MovementComponent.BlockMovement(false);
            CasterEntity.MovementComponent.BlockRotation(false);
        }

        protected override void Logic()
        {
            var playerEntity = (PlayerEntity)CasterEntity;
            if (playerEntity != null)
            {
                playerEntity.ForceForward();
            }
            CasterEntity.MovementComponent.BlockMovement(true);
            CasterEntity.MovementComponent.BlockRotation(true);
        }
    }
}
