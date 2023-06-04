using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newBarrageSkill", menuName = "Saligia/Skills/Secondary/Barrage")]
    public class BarrageSkill : BaseSkill
    {
        public override void CleanUp()
        {
            CasterEntity.MovementComponent.BlockMovement(false);
            CasterEntity.MovementComponent.BlockRotation(false);
        }

        public override void CastActivate(Entity caster)
        {
            base.CastActivate(caster);
            CasterEntity.MovementComponent.BlockMovement(true);
            CasterEntity.MovementComponent.BlockRotation(true);
        }

        protected override void Logic()
        {
            CreateSkillobject().transform.position = TargetData.GetTargetPosition();
        }
    }
}
