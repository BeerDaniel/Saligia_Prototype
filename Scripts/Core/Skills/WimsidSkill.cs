using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newWimsidSkill", menuName = "Saligia/Skills/Secondary/Wimsid")]
    public class WimsidSkill : BaseSkill
    {
        [SerializeField] private int _healOnEnvyKill;
        [SerializeField] private int _mpOnEnvyKill;

        [SerializeField] private SkillCost _skillCostNormal;
        [SerializeField] private AnimationCurve _additionalDamageMultiplierNormal;
        [SerializeField] private SkillCost _skillCostGluttony;
        [SerializeField] private AnimationCurve _additionalDamageMultiplierGluttony;

        private float _payedSkillCostInPercent = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            CheckRunes();
            _skillCostNormal.Init();
            _skillCostGluttony.Init();
        }

        private void OnValidate()
        {
            CheckRunes();
        }

        public override void CleanUp()
        {
            CasterEntity.MovementComponent.BlockMovement(false);
            CasterEntity.MovementComponent.BlockRotation(false);
            DamageMultiplier.UndoMultiplier(_additionalDamageMultiplierGluttony.Evaluate(_payedSkillCostInPercent));
        }

        public override void CastActivate(Entity caster)
        {
            base.CastActivate(caster);
            CasterEntity.MovementComponent.BlockMovement(true);
            CasterEntity.MovementComponent.BlockRotation(true);
        }

        protected override void Logic()
        {
            PayWimsid();
            skillObject = CreateSkillobject();
            if (TargetData.GetTargetObject().GetComponent<HealthComponent>().IsDead)
                KilledBySkill();
            Destroy(skillObject);
        }

        private void PayWimsid()
        {
            var costComponent = CasterEntity.GetComponent<CastCostComponent>();
            if (runes.Contains(Rune.Gluttony))
                PayWithLife();
            else
                PayWithMindPower();
        }

        private void PayWithLife()
        {
            int paying = (int)Mathf.Ceil(CasterEntity.HealthComponent.CurrentHitPoints -
                (skillCost.CurrentHealthCost / 100) * CasterEntity.HealthComponent.MaxHitPoints);
            _payedSkillCostInPercent = Mathf.InverseLerp(0, CasterEntity.HealthComponent.MaxHitPoints -
                (skillCost.CurrentHealthCost / 100) * CasterEntity.HealthComponent.MaxHitPoints, paying);
            DamageMultiplier.ApplyMultiplier(_additionalDamageMultiplierGluttony.Evaluate(_payedSkillCostInPercent));
            CasterEntity.HealthComponent.ApplyDamage(new DamageData(paying, isTrueDamage: true));
        }

        private void PayWithMindPower()
        {
            var costComponent = CasterEntity.GetComponent<CastCostComponent>();
            var paying = costComponent.CurrentMindPower;
            _payedSkillCostInPercent = Mathf.InverseLerp(0, costComponent.MaxMindPower, paying);
            DamageMultiplier.ApplyMultiplier(_additionalDamageMultiplierNormal.Evaluate(_payedSkillCostInPercent));
            costComponent.Pay(paying);
        }

        private void KilledBySkill()
        {
            if (runes.Contains(Rune.Envy))
            {
                CasterEntity.HealthComponent?.ApplyHealing((int)Mathf.Ceil(_healOnEnvyKill / 100f * CasterEntity.HealthComponent.MaxHitPoints));
                CasterEntity.CastCostComponent?.AddMindPower(_mpOnEnvyKill / 100f * CasterEntity.GetComponent<CastCostComponent>().MaxMindPower, ignoreRegenMultipliers: true);
            }
        }

        protected override void CheckRunes()
        {
            if (runes.Contains(Rune.Gluttony))
                skillCost = _skillCostGluttony;

            else
                skillCost = _skillCostNormal;
        }
    }
}
