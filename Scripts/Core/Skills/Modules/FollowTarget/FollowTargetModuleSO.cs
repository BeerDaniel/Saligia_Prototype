using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newFollowTargetModule", menuName = "Saligia/Skills/Modules/FollowTargetModule")]
    public class FollowTargetModuleSO : ModuleSO
    {
        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var followBehaviour = new FollowTargetModuleBehaviour(baseSkill);
            SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(followBehaviour);
        }
    }
}




