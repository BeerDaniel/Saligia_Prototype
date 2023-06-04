using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newFlyOrbSkill", menuName = "Saligia/Skills/Secondary/Flying Orb")]
    public class FlyOrbSkill : BaseSkill
    {
        [SerializeField] private float _spawnGap = 0.5f;
        [SerializeField] private float _gluttonyProjectileGap = 1f;
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
            skillObject = CreateSkillobject();
            Vector3 targetVector = TargetData.GetTargetDirection(CasterEntity.transform.position);

            if (!runes.Contains(Rune.Gluttony))
            {
                skillObject.transform.rotation = Quaternion.LookRotation(targetVector);
                skillObject.transform.position = CasterEntity.transform.position + (targetVector * _spawnGap);
            }
            else
            {
                //Projectile A;Left
                skillObject.transform.rotation = Quaternion.LookRotation(targetVector);
                skillObject.transform.position = CasterEntity.transform.position + (targetVector * _spawnGap) - (CasterEntity.transform.right * _gluttonyProjectileGap * 0.5f);

                //Projectile B;Right
                GameObject objectB = CreateSkillobject();
                objectB.transform.rotation = Quaternion.LookRotation(targetVector);
                objectB.transform.position = CasterEntity.transform.position + (targetVector * _spawnGap) + (CasterEntity.transform.right * _gluttonyProjectileGap * 0.5f);
                foreach (var behaviour in objectB.GetComponent<SkillBehaviour>().ModuleBehaviours)
                {
                    if (behaviour is ProjectileModuleBehaviour)
                    {
                        ((ProjectileModuleBehaviour)behaviour)._projectileData.isMirrored = true;
                        break;
                    }
                }


            }


        }
    }
}
