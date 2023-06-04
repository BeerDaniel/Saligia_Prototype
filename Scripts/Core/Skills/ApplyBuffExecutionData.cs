using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Buffs;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newApplyBuffExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/Apply Buff Execution Data")]
    public class ApplyBuffExecutionData : ModuleExecutionData
    {
        [SerializeField] private BuffData _buffToApply;
        protected override void Logic(GameObject buffTarget)
        {
            if (buffTarget.TryGetComponent<Entity>(out var entity))
                if (entity.BuffComponent)
                    entity.BuffComponent.AddBuff(_buffToApply, baseSkill.CasterEntity);
        }
    }
}




