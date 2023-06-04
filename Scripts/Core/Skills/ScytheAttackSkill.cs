using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newScytheAttackSkill", menuName = "Saligia/Skills/Attack Skills/Scythe Attack")]
    public class ScytheAttackSkill : BaseAttackSkill
    {
        public GameObject trailRenderer;

        public override void CleanUp()
        {
            base.CleanUp();

            var playerEntity = (PlayerEntity)CasterEntity;
            if (playerEntity != null)
            {
                playerEntity.ActivateScytheTrail(false);
            }

            if (CasterEntity.MovementComponent != null)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
            }
        }

        public override void OnEnemyHit(Entity entity)
        {
            base.OnEnemyHit(entity);
            if (entity.HealthComponent.IsDead)
                return;

            entity.ApplyDamage(AttackValue.damageData);
            if (CasterEntity.CastCostComponent != null)
                CasterEntity.CastCostComponent.AddMindPower(AttackValue.mpPerHit, true);
        }

        protected override void Logic()
        {
            var playerEntity = (PlayerEntity)CasterEntity;
            if (playerEntity != null)
            {
                playerEntity.ActivateScytheTrail(true, playerEntity.WeaponComponent.AttackSkillIndex);
            }
            AttackValue.damageData.damageSource = CasterEntity;
            if (CasterEntity.MovementComponent != null)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }
    }
}
