using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newSporeSkill", menuName = "Saligia/Skills/Monster/Mushroom/Spore Skill")]
    public class SporeSkill : BaseSkill
    {
        public override void CleanUp()
        {
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
            }
        }

        public override void CastActivate(Entity caster)
        {
            base.CastActivate(caster);
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }

        protected override void Logic()
        {
            skillObject = CreateSkillobject();
            skillObject.transform.position = CasterEntity.transform.position;
        }
    }
}




