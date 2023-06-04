using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newProjectileModule", menuName = "Saligia/Skills/Modules/ProjectileModule")]
    public class ProjectileModuleSO : ModuleSO
    {
        [field:SerializeField]
        public ProjectileData ProjectileData { get; private set; }

        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var projBehaviour = new ProjectileModuleBehaviour(new ProjectileData(ProjectileData));
            SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(projBehaviour);
        }
    }

    [System.Serializable]
    public class ProjectileData
    {
        [field:SerializeField]
        public string AgentTypeName { get; private set; }
        public float agentBaseOffset = 1;
        public float distance = 10;
        public AnimationCurve speedCurve;
        public AnimationCurve translationCurve;
        public bool isReturning = false;
        [HideInInspector]
        public bool isMirrored = false;

        public ProjectileData(ProjectileData projectileData)
        {
            AgentTypeName = projectileData.AgentTypeName;
            agentBaseOffset = projectileData.agentBaseOffset;
            distance = projectileData.distance; 
            speedCurve = projectileData.speedCurve; 
            translationCurve = projectileData.translationCurve; 
            isReturning = projectileData.isReturning;
            isMirrored = projectileData.isMirrored;
        }
    }

   
}
