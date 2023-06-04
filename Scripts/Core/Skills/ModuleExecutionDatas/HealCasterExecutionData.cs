using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newHealCasterExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/HealCasterExecutionData")]
    public class HealCasterExecutionData : ModuleExecutionData
    {
        [SerializeField] private int _healamount;
        protected override void Logic(GameObject target)
        {
            Debug.Log("Healing Caster");
            baseSkill.CasterEntity.HealthComponent.ApplyHealing(_healamount);
        }
    }
}




