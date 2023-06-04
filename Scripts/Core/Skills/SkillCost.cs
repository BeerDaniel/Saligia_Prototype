using SuspiciousGames.Saligia.Core.Entities.Components;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [System.Serializable]
    public class SkillCost
    {
        [SerializeField]
        private float _baseMpCost;
        public float CurrentMpCost { get; private set; }

        [SerializeField]
        private float _baseHealthCost;
        public float CurrentHealthCost { get; private set; }

        [field: SerializeField] public Multiplier MpCostMultiplier { get; private set; }
        [field: SerializeField] public Multiplier HealthCostMultiplier { get; private set; }

        [SerializeField] private bool _isPrerequisitOnly = false;
        public bool IsPrerequisitOnly => _isPrerequisitOnly;

        public void Init()
        {
            CurrentHealthCost = _baseHealthCost;
            CurrentMpCost = _baseMpCost;
            MpCostMultiplier.ResetMultiplier();
            HealthCostMultiplier.ResetMultiplier();
        }

        public void ChangeHealthCost(float value)
        {
            CurrentHealthCost += value;
        }

        public void ChangeMpCost(float value)
        {
            CurrentMpCost += value;
        }

        public void ResetCost()
        {
            CurrentMpCost = _baseMpCost;
            CurrentHealthCost = _baseHealthCost;
        }
    }
}
