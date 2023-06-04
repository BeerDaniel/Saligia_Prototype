using SuspiciousGames.Saligia.Core.Components.Weapons;
using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newMushroomAttackSkill", menuName = "Saligia/Skills/Monster/Mushroom/Mushroom Attack")]
    public class MushroomAttackSkill : BaseAttackSkill
    {
        public override void CleanUp()
        {
            base.CleanUp();
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
                //CasterEntity.MovementComponent.BlockRotation(true);
            }
        }

        protected override void Logic()
        {
            CasterEntity.MovementComponent.BlockRotation(true);
            var go = new GameObject("MushroomTarget");
            go.transform.position = TargetData.GetTargetPosition();
            TargetData = new Entities.Components.TargetData(go);
            skillObject = CreateSkillobject();

            skillObject.transform.Rotate(skillObject.transform.right, 70);

            if (CasterEntity.WeaponComponent.GetActiveWeapon(out var weapon))
                if (skillObject)
                    skillObject.transform.position = ((RangeWeapon)weapon).MuzzleTransform.position;
        }
    }
}




