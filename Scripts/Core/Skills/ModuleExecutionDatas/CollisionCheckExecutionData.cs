using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newCollisionCheckExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/CollisionCheckExecutionData")]
    public class CollisionCheckExecutionData : ModuleExecutionData
    {
        [SerializeField] private CollisionArea _collisionArea;
        [SerializeField] private List<ModuleExecutionData> _executionDatas;
        protected override void Logic(GameObject target)
        {
            foreach (var gameObject in _collisionArea.CheckForObjectsInArea(target.transform.position))
            {
                foreach (var executionData in _executionDatas)
                {
                    executionData.Execute(gameObject.gameObject, baseSkill);
                }
            }

        }
    }
}




