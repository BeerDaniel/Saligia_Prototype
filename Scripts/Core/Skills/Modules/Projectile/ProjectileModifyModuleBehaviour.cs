using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class ProjectileModifyModuleBehaviour : ModuleBehaviour
    {
        private ProjectileModifyData _projectileModifyData;

        public ProjectileModifyModuleBehaviour(ProjectileModifyData projectileModifyData)
        {
            _projectileModifyData = projectileModifyData;
        }

        public override void OnAwake()
        {
            //throw new NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            //throw new NotImplementedException();
        }

        public override void OnStart()
        {
            foreach (var behaviour in skillObject.GetComponent<SkillBehaviour>().ModuleBehaviours)
            {
                if (behaviour is ProjectileModuleBehaviour)
                {
                    _projectileModifyData.ModifyProjectileData(ref ((ProjectileModuleBehaviour)behaviour)._projectileData);
                    break;
                }
            }
        }
    }
}
