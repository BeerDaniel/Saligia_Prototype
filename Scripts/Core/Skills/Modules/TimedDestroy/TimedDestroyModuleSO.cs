using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newTimedDestroyModule", menuName = "Saligia/Skills/Modules/TimedDestroyModule")]
    public class TimedDestroyModuleSO : ModuleSO
    {
        [SerializeField] private float _destroyAfterSeconds;

        public override void Init(GameObject gameObject, BaseSkill baseSkill)
        {
            TimedDestroyModuleBehaviour behaviour = new(_destroyAfterSeconds);
            if (gameObject.TryGetComponent(out SkillBehaviour skillBehaviour))
                skillBehaviour.AddModuleBehaviour(behaviour);
            else
                SkillBehaviour.Create(gameObject).AddModuleBehaviour(behaviour);

        }
    }
}




