using System.Collections.Generic;
using UnityEngine;
using static SuspiciousGames.Saligia.Core.Skills.OnCollisionModuleBehaviour;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newOnCollisionModule", menuName = "Saligia/Skills/Modules/OnCollisionModule")]
    public class OnCollisionModuleSO : ModuleSO
    {
        [SerializeField] private CollisionArea _collisionArea;
        [SerializeField] private List<ModuleExecutionData> _onEnterModuleExecutionData;
        [SerializeField] private List<UpdateIntervalContainer> _onStayModuleExecutionData;
        [SerializeField] private List<ModuleExecutionData> _onExitModuleExecutionData;

        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var colBehaviour = new OnCollisionModuleBehaviour(_onEnterModuleExecutionData,
               _onStayModuleExecutionData,
               _onExitModuleExecutionData,
               _collisionArea,
               baseSkill);
            if (skillGameObject.TryGetComponent(out SkillBehaviour skillBehaviour))
                skillBehaviour.AddModuleBehaviour(colBehaviour);
            else
                SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(colBehaviour);
        }
    }
}




