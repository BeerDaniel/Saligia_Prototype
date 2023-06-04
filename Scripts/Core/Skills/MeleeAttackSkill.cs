using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newMeleeAttackSkill", menuName = "Saligia/Skills/Attack Skills/Melee Attack")]

    public class MeleeAttackSkill : BaseAttackSkill
    {
        public override void CleanUp()
        {
            base.CleanUp();
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
            if (entity.HealthComponent.IsDead)
            {
                //TODO apply onDeath effects of scythe skill if any
            }
            //TODO apply any buffs or debuffs
            if (CasterEntity.CastCostComponent)
                CasterEntity.CastCostComponent.AddMindPower(AttackValue.mpPerHit, true);
        }

        protected override void Logic()
        {
            if (CasterEntity.MovementComponent != null)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }
    }
}
