using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public abstract class ModuleBehaviour
    {
        protected BaseSkill baseSkill;
        public bool needsMonoBehaviour;

        protected GameObject skillObject;

        public void SetGameobject(GameObject skillObject)
        {
            this.skillObject = skillObject;
        }

        public abstract void OnFixedUpdate();
        public abstract void OnStart();
        public abstract void OnAwake();

        public virtual void OnDrawGizmos() { }
        public virtual void OnDrawGizmosSelected() { }
    }
}




