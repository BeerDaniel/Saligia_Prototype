using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public abstract class ModuleExecutionData : ScriptableObject
    {
        [SerializeField] private LayerMask _affectedEntitites;

        protected BaseSkill baseSkill;

        public LayerMask AffectedEntitites => _affectedEntitites;

        // owner, target, baseskill
        public void Execute(GameObject target, BaseSkill baseSkill)
        {
            if ((1 << target.layer & _affectedEntitites) != 1 << target.layer)
                return;
            this.baseSkill = baseSkill;
            Logic(target);
        }

        protected abstract void Logic(GameObject gameObject);
    }
}




