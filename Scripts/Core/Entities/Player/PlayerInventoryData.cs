using SuspiciousGames.Saligia.Core.Player;
using SuspiciousGames.Saligia.Core.Potions;
using SuspiciousGames.Saligia.Core.Skills;
using SuspiciousGames.Saligia.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Entities.Player
{
    [Serializable, CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "PlayerInventoryData")]
    public class PlayerInventoryData : ScriptableObject
    {
        [Flags]
        public enum RuneFlags
        {
            Envy = 1,
            Gluttony = 2,
            Greed = 4,
            Lust = 8,
            Pride = 16,
            Sloth = 32,
            Wrath = 64
        }

        [Serializable]
        public class ActivePlayerSkills
        {
            public PlayerWeaponType currentWeaponType;
            public BaseSkill secondaryOneSkill;
            public BaseSkill secondaryTwoSkill;
            public BaseSkill secondaryThreeSkill;
        }

        [SerializeField] private ActivePlayerSkills _activePlayerSkills;

        [SerializeField] private BaseSkill _scytheSkill;
        [SerializeField] private BaseSkill _grimoireSkill;
        
        [SerializeField] private List<BaseSkill> _primarySkills;
        [SerializeField] private List<BaseSkill> _secondarySkills;

        [SerializeField] private List<PotionSettings> _availablePotionSettings;
        [SerializeField, HideInInspector] private List<Potion> _potions;
        public List<Potion> Potions => _potions;

        [Tooltip("Unlocked Runes")]
        [field: SerializeField, EnumFlags] public RuneFlags UnlockedTier1Runes { get; private set; }
        [field: SerializeField, EnumFlags] public RuneFlags UnlockedTier2Runes { get; private set; }
        [field: SerializeField, EnumFlags] public RuneFlags UnlockedTier3Runes { get; private set; }

        [Serializable]
        public class RuneSkillMapper
        {
            public Rune rune;
            public BaseSkill skill;
        }

        [SerializeField, HideInInspector] private List<RuneSkillMapper> _mappedSkillRunes;

        public Dictionary<Rune, BaseSkill> skillRunes;
        public BaseSkill ScytheSkill => _scytheSkill;
        public BaseSkill GrimoireSkill => _grimoireSkill;
        public IReadOnlyList<BaseSkill> PrimarySkills => _primarySkills;
        public IReadOnlyList<BaseSkill> SecondarySkills => _secondarySkills;
        
        public void Init()
        {
            InitPotions();
            InitSkillRunes();
            PlayerEntity.Instance.SwitchWeapon(_activePlayerSkills.currentWeaponType);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryOneSkill, SkillSlot.SecondaryOne);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryTwoSkill, SkillSlot.SecondaryTwo);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryThreeSkill, SkillSlot.SecondaryThree);
        }

        private void InitSkillRunes()
        {
            skillRunes = new();
            _mappedSkillRunes = new();
            foreach (var secondarySkill in _secondarySkills)
            {
                foreach (var rune in secondarySkill.Runes)
                {
                    if (rune == Rune.Base)
                        continue;
                    skillRunes.Add(rune, secondarySkill);
                    _mappedSkillRunes.Add(new() { rune = rune, skill = secondarySkill });
                }
            }
        }

        private void InitPotions()
        {
            _potions = new List<Potion>() { null, null, null, null };
            for (int i = 0; i < _availablePotionSettings.Count; i++)
            {
                _potions[i] = new Potion(_availablePotionSettings[i]);
            }
        }

        public void Init(PlayerInventoryData playerInventoryData)
        {
            _availablePotionSettings = playerInventoryData._availablePotionSettings;
            skillRunes = playerInventoryData.skillRunes;
            _activePlayerSkills = playerInventoryData._activePlayerSkills;

            PlayerEntity.Instance.SwitchWeapon(_activePlayerSkills.currentWeaponType);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryOneSkill, SkillSlot.SecondaryOne);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryTwoSkill, SkillSlot.SecondaryTwo);
            PlayerEntity.Instance.SetAbilityBySlot(_activePlayerSkills.secondaryThreeSkill, SkillSlot.SecondaryThree);

            if (playerInventoryData.Potions.Count == 0)
            {
                InitPotions();
            }
            else
            {
                _potions = playerInventoryData._potions;
                foreach (var potion in _potions)
                {
                    potion.UpdatePotion();
                }
            }

            if (playerInventoryData._mappedSkillRunes.Count == 0)
            {
                InitSkillRunes();
            }
            else
            {
                _mappedSkillRunes = playerInventoryData._mappedSkillRunes;
                skillRunes = new();
                foreach (var mappedSkillRune in _mappedSkillRunes)
                {
                    skillRunes.Add(mappedSkillRune.rune, mappedSkillRune.skill);
                    mappedSkillRune.skill.AddRune(mappedSkillRune.rune);
                }
            }

            UnlockedTier1Runes = playerInventoryData.UnlockedTier1Runes;
            UnlockedTier2Runes = playerInventoryData.UnlockedTier2Runes;
            UnlockedTier3Runes = playerInventoryData.UnlockedTier3Runes;
        }

        #region Active Skill Methods
        public void ChangeActiveWeapon(PlayerWeaponType playerWeaponType)
        {
            _activePlayerSkills.currentWeaponType = playerWeaponType;
            PlayerEntity.Instance.SwitchWeapon(playerWeaponType);
        }

        public void ChangeActiveSkillBySkillSlot(BaseSkill skill, SkillSlot skillSlot)
        {
            PlayerEntity.Instance.ChangeSecondaryAbility(skill, skillSlot);
            _activePlayerSkills.secondaryOneSkill = PlayerEntity.Instance.SecondaryAbilityOne;
            _activePlayerSkills.secondaryTwoSkill = PlayerEntity.Instance.SecondaryAbilityTwo;
            _activePlayerSkills.secondaryThreeSkill = PlayerEntity.Instance.SecondaryAbilityThree;
            //PlayerEntity.Instance.SetAbilityBySlot(skill, skillSlot);
        }
        #endregion

        #region Skill Rune Methods
        public void AddRuneToSkill(Rune rune, BaseSkill baseSkill)
        {
            if (skillRunes.TryGetValue(rune, out var skill))
            {
                skill.RemoveRune(rune);
                _mappedSkillRunes.RemoveAt(_mappedSkillRunes.FindIndex(item => item.rune == rune));
                skillRunes.Remove(rune);
            }
            baseSkill.AddRune(rune);
            skillRunes.Add(rune, baseSkill);
            _mappedSkillRunes.Add(new() { rune = rune, skill = baseSkill });
        }

        public void RemoveRuneFromSkill(Rune rune)
        {
            if (skillRunes.TryGetValue(rune, out var skill))
            {
                skill.RemoveRune(rune);
                _mappedSkillRunes.RemoveAt(_mappedSkillRunes.FindIndex(item => item.rune == rune));
                skillRunes.Remove(rune);
            }
        }
        #endregion

        #region Potion Methods
        internal void SwapPotions(int from, int to)
        {
            var tempPotion = _potions[from];
            _potions[from] = _potions[to];
            _potions[to] = tempPotion;

            var tempPotionSetting = _availablePotionSettings[from];
            _availablePotionSettings[from] = _availablePotionSettings[to];
            _availablePotionSettings[to] = tempPotionSetting;
        }

        internal bool UpgradePotions(PotionSettings potionSettings, out PotionSlot potionSlot)
        {
            var potion = _potions.Find(p => p.HasPotionSettings(potionSettings));
            if (potion == null)
            {
                potionSlot = PotionSlot.North;

                return false;
            }

            potion.Upgrade();
            potionSlot = (PotionSlot)_potions.IndexOf(potion);
            return true;
        }

        internal void UnlockPotion(PotionSettings potionSettings)
        {
            if (_availablePotionSettings.Contains(potionSettings))
                return;
            _availablePotionSettings.Add(potionSettings);
            _potions[_availablePotionSettings.IndexOf(potionSettings)] = new Potion(potionSettings);
        }

        internal bool ContainsPotion(PotionSettings potionSettings, out PotionSlot potionSlot)
        {
            if (!_availablePotionSettings.Contains(potionSettings))
            {
                potionSlot = PotionSlot.North;
                return false;
            }
            potionSlot = (PotionSlot)_availablePotionSettings.IndexOf(potionSettings);
            return true;
        }

        internal void RestorePotionCharges(int amount)
        {
            foreach (var potion in _potions)
                potion.RestoreCharges(amount);
        }
        #endregion

        #region Rune Methods
        public void AddTier1Rune(Rune rune)
        {
            RuneFlags flags = (RuneFlags)(int)rune;
            UnlockedTier1Runes |= flags;
        }

        public void RemoveTier1Rune(Rune rune)
        {
            RuneFlags flags = ~(RuneFlags)rune;
            UnlockedTier1Runes &= flags;
        }

        public void AddTier2Rune(Rune rune)
        {
            RuneFlags flags = (RuneFlags)(int)rune;
            UnlockedTier2Runes |= flags;
        }

        public void RemoveTier2Rune(Rune rune)
        {
            RuneFlags flags = ~(RuneFlags)rune;
            UnlockedTier2Runes &= flags;
        }

        public void AddTier3Rune(Rune rune)
        {
            RuneFlags flags = (RuneFlags)(int)rune;
            UnlockedTier3Runes |= flags;
        }

        public void RemoveTier3Rune(Rune rune)
        {
            RuneFlags flags = ~(RuneFlags)rune;
            UnlockedTier3Runes &= flags;
        }
        #endregion
    }
}