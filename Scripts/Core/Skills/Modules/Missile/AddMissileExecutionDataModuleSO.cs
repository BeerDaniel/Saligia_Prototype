using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newAddMissileExecutionDataModule", menuName = "Saligia/Skills/Modules/AddMissileExecutionDataModule")]
    public class AddMissileExecutionDataModuleSO : ModuleSO
    {
        [SerializeField] private List<ModuleExecutionData> _onTargetReachedModuleExecutionData;

        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var addBehaviour = new AddMissileExecutionDataModuleBehaviour(_onTargetReachedModuleExecutionData);
            SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(addBehaviour);
        }
    }


}
