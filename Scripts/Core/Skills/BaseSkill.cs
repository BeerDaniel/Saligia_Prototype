using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public enum SkillPriority
    {
        Low = 1,
        Medium = 10,
        High = 20
    }

    public abstract class BaseSkill : ScriptableObject
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected LocalizedString localizedSkillName;
        public string SkillName => skillName;
        protected string skillDescription;
        [SerializeField] protected LocalizedString localizedSkillDescription;
        public string SkillDescription => skillDescription;
        protected string skillDescriptionGluttony;
        [SerializeField] protected LocalizedString localizedSkillDescriptionGluttony;
        public string SkillDescriptionGluttony => skillDescriptionGluttony;
        protected string skillDescriptionEnvy;
        [SerializeField] protected LocalizedString localizedSkillDescriptionEnvy;
        public string SkillDescriptionEnvy => skillDescriptionEnvy;
        [SerializeField] protected Sprite sprite;
        public Sprite Sprite => sprite;
        [SerializeField] protected List<Rune> runes = new List<Rune>() { Rune.Base };
        public List<Rune> Runes => runes;
        [field: SerializeField] public Multiplier DamageMultiplier { get; private set; }

        [SerializeField, Min(0.0f)] protected float cooldown = 0.0f;
        public float Cooldown => cooldown;

        [SerializeField] protected SkillCost skillCost;

        [SerializeField] protected SkillPriority skillPriority;
        public SkillPriority Priority => skillPriority;

        [SerializeField] private AimingSO _aiming;
        [Tooltip("Runes that need to use Different Aiming")]
        [SerializeField] private List<Rune> _specialAimingRunes;
        [Tooltip("Second Aiming to use at Specific Runes")]
        [SerializeField] private AimingSO _specialAiming;
        public AimingSO Aiming => GetAiming();
        public bool HasAiming => GetAiming() != null;

        public SkillCost SkillCost => skillCost;

        public List<ModuleSO> moduleSOs;

        [SerializeField] private string _castAnimationTrigger = "";
        [SerializeField] private string _skillAnimationTrigger = "";
        public bool HasCastAnimation { get; private set; }
        public bool HasSkillAnimation { get; private set; }
        public int CastAnimationTriggerHash { get; private set; }
        public int SkillAnimationTriggerHash { get; private set; }

        public Entity CasterEntity { get; private set; }
        public TargetData TargetData { get; protected set; }
        protected GameObject skillObject;

        [field: SerializeField] public bool NeedsTarget { get; private set; }

        protected virtual void OnEnable()
        {
            if (_castAnimationTrigger == "" || _castAnimationTrigger == null)
            {
                HasCastAnimation = false;
            }
            else
            {
                HasCastAnimation = true;
                CastAnimationTriggerHash = Animator.StringToHash(_castAnimationTrigger);
            }

            if (_skillAnimationTrigger == "" || _skillAnimationTrigger == null)
            {
                HasSkillAnimation = false;
            }
            else
            {
                HasSkillAnimation = true;
                SkillAnimationTriggerHash = Animator.StringToHash(_skillAnimationTrigger);
            }

            localizedSkillName.StringChanged += (string s) => skillName = s;
            localizedSkillDescription.StringChanged += (string s) => skillDescription = s;
            localizedSkillDescriptionGluttony.StringChanged += (string s) => skillDescriptionGluttony = s;
            localizedSkillDescriptionEnvy.StringChanged += (string s) => skillDescriptionEnvy = s;

            skillCost?.Init();

            // if (HasAiming)
            //     NeedsTarget = true;
        }

        public virtual void CastActivate(Entity caster)
        {
            if (caster == null)
                throw new NullReferenceException("Caster was not set");
            CasterEntity = caster;
            if (HasCastAnimation)
                CasterEntity.Animator.SetTrigger(CastAnimationTriggerHash);
        }

        public void Activate(Entity caster, TargetData targetData)
        {
            if (caster == null)
                throw new NullReferenceException("Caster was not set");

            CasterEntity = caster;
            if (targetData != null)
                TargetData = new TargetData(targetData);
            if (HasSkillAnimation)
                CasterEntity.Animator.SetTrigger(SkillAnimationTriggerHash);
            //skillObject = CreateSkillobject();
            Logic();
        }

        public void AddRune(Rune rune)
        {
            if (!runes.Contains(rune))
            {
                runes.Add(rune);
                CheckRunes();
            }
        }
        public void RemoveRune(Rune rune)
        {
            if (runes.Contains(rune))
            {
                runes.Remove(rune);
                CheckRunes();
            }
        }

        public void ResetRunes()
        {
            runes = new() { Rune.Base };
        }

        protected GameObject CreateSkillobject()
        {
            GameObject dummySkillObject = null;
            if (moduleSOs.Count != 0)
                dummySkillObject = new GameObject(skillName);
            foreach (var runeModule in moduleSOs)
                if (runes.Contains(runeModule.Rune))
                    runeModule.Init(dummySkillObject, this);

            return dummySkillObject;
        }

        private AimingSO GetAiming()
        {
            if (runes.Intersect(_specialAimingRunes).Any())
            {
                return _specialAiming;
            }
            return _aiming;
        }

        protected abstract void Logic();

        public abstract void CleanUp();

        public virtual void AnimationTriggeredLogic() { }

        protected virtual void CheckRunes() { }

        // private void OnValidate()
        // {
        //     if (HasAiming)
        //         NeedsTarget = true;
        // }
    }
}




