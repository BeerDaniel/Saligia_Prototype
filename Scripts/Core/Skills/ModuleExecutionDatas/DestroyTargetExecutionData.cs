using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newDestroyTargetExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/DestroyTargetExecutionData")]
    public class DestroyTargetExecutionData : ModuleExecutionData
    {
        protected override void Logic(GameObject target)
        {
            Destroy(target);
        }
    }
}




