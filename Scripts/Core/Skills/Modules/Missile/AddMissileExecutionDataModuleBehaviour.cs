using System.Collections.Generic;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class AddMissileExecutionDataModuleBehaviour : ModuleBehaviour
    {
        private List<ModuleExecutionData> _onTargetReachedModuleExecutionData;

        public AddMissileExecutionDataModuleBehaviour(List<ModuleExecutionData> onTargetReachedModuleExecutionData)
        {
            _onTargetReachedModuleExecutionData = onTargetReachedModuleExecutionData;
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
                if (behaviour is MissileModuleBehaviour)
                {
                    foreach (var data in _onTargetReachedModuleExecutionData)
                    {
                        ((MissileModuleBehaviour)behaviour).AddExecutionData(data);
                    }
                    break;
                }
            }
        }
    }
}
