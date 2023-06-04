using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newInstantExecutionModule", menuName = "Saligia/Skills/Modules/InstantExecutioModule")]
    public class InstantExecutionModuleSO : ModuleSO
    {
        [SerializeField] private List<ModuleExecutionData> _instantExecutionDatas;
        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            //var instantBehaviour = new InstantExecutionModuleBehaviour(baseSkill,
            //    new List<ModuleExecutionData>(_instantExecutionDatas));
            //SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(instantBehaviour);

            foreach (var data in _instantExecutionDatas)
            {
                var target = baseSkill.TargetData.GetTargetObject();
                if (target)
                {
                    data.Execute(target, baseSkill);
                }
            }
        }
    }


}
