using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public enum Seekmode { linear, cubic }

    [CreateAssetMenu(fileName = "newMissileModule", menuName = "Saligia/Skills/Modules/MissileModule")]
    public class MissileModuleSO : ModuleSO
    {
        [SerializeField] private List<ModuleExecutionData> _onTargetReachedModuleExecutionData;
        [SerializeField] private Seekmode _seekmode = Seekmode.linear;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _curvature = 1f;
        [SerializeField] private float _afterglow;

        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var missileBehaviour = new MissileModuleBehaviour(baseSkill,
                new List<ModuleExecutionData>(_onTargetReachedModuleExecutionData),
                _seekmode,
                _speed,
                _curvature,
                _afterglow);
            SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(missileBehaviour);
        }
    }


}
