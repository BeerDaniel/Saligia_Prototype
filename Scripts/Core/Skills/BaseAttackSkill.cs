using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public abstract class BaseAttackSkill : BaseSkill
    {
        [field: SerializeField] public AttackValue AttackValue { get; private set; }

        public override void CleanUp()
        {
            CasterEntity.WeaponComponent.CleanUp();
        }

        public virtual void OnEnemyHit(Entity entity) { }
    }
}




