using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newDamageExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/DamageExecutionData")]
    public class DamageExecutionData : ModuleExecutionData
    {
        [SerializeField] private DamageData _damageData;
        protected override void Logic(GameObject target)
        {
            //TODO this needs to be scaled according to different multipliers
            if (target.TryGetComponent<Entity>(out var entity))
            {
                var damage = _damageData.damageAmount * (baseSkill.DamageMultiplier.Value *
                    baseSkill.CasterEntity.CastComponent.DamageMultiplier.Value);

                DamageData damageDataClone = new DamageData(_damageData);

                damageDataClone.damageSource = baseSkill.CasterEntity;
                damageDataClone.damageAmount = (int)Mathf.Ceil(damage);
                entity.ApplyDamage(damageDataClone);
            }

        }
    }
}




