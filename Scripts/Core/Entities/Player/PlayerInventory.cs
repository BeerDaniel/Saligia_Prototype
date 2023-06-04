using SuspiciousGames.Saligia.Core.Potions;
using SuspiciousGames.Saligia.Core.Skills;
using SuspiciousGames.Saligia.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SuspiciousGames.Saligia.Core.Entities.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] PlayerInventoryData _basePlayerInventoryData;
        public PlayerInventoryData PlayerInventoryData { get; set; }

        [Serializable]
        public class Events
        {
            [Serializable]
            public class PotionEvents
            {
                public UnityEvent<PotionSlot> onPotionUsed;
                public UnityEvent onPotionSlotsChanged;
                public UnityEvent<PotionSlot> onPotionUpgraded;
                public UnityEvent onPotionChargesGained;
            }

            public PotionEvents potionEvents;
        }

        public Events events;

        private Coroutine _potionRegenerationCoroutine;

        private void OnApplicationQuit()
        {
            foreach (var secondarySkill in PlayerInventoryData.SecondarySkills)
                secondarySkill.ResetRunes();
        }

        public void Init()
        {
            PlayerInventoryData = Instantiate(_basePlayerInventoryData);
            PlayerInventoryData.Init();
            events.potionEvents.onPotionSlotsChanged.Invoke();
            if (_potionRegenerationCoroutine == null)
                _potionRegenerationCoroutine = StartCoroutine(RestorePotionChargeOverTime());
        }

        public void Init(PlayerInventoryData playerInventoryData)
        {
            PlayerInventoryData = Instantiate(_basePlayerInventoryData);
            PlayerInventoryData.Init(playerInventoryData);
            events.potionEvents.onPotionSlotsChanged.Invoke();
            if (_potionRegenerationCoroutine == null)
                _potionRegenerationCoroutine = StartCoroutine(RestorePotionChargeOverTime());
        }

        private IEnumerator RestorePotionChargeOverTime()
        {
            yield return new WaitUntil(() => PlayerEntity.Instance != null);
            while (!PlayerEntity.Instance.HealthComponent.IsDead)
            {
                yield return new WaitForSeconds(1f);
                PlayerInventoryData.RestorePotionCharges(5);
            }

            _potionRegenerationCoroutine = null;
        }

        #region Potion Methods

        public bool SwapPotionSlots(int from, int to)
        {
            if (from < 0 || to < 0 || from > PlayerInventoryData.Potions.Count - 1 || to > PlayerInventoryData.Potions.Count - 1)
                return false;
            PlayerInventoryData.SwapPotions(from, to);
            events.potionEvents.onPotionSlotsChanged.Invoke();
            return true;
        }

        public Potion GetPotion(PotionSettings potionSetting)
        {
            Potion potion = null;
            potion = PlayerInventoryData.Potions.Find(potion => potion.HasPotionSettings(potionSetting));

            return potion;
        }

        public BaseSkill GetSpellForEquippedRune(Rune rune)
        {
            if (PlayerInventoryData.skillRunes.TryGetValue(rune, out var skill))
                return skill;
            return null;
        }

        public Potion GetPotion(PotionSlot potionSlot)
        {
            return GetPotion((int)potionSlot);
        }

        public Potion GetPotion(int potionIndex)
        {
            if (potionIndex < 0 || potionIndex > PlayerInventoryData.Potions.Count - 1)
                return null;

            return PlayerInventoryData.Potions[potionIndex];
        }

        public bool UsePotion(PotionSlot potionSlot)
        {
            var potionToUse = GetPotion((int)potionSlot);
            if (potionToUse != null && potionToUse.IsUsable(PlayerEntity.Instance))
            {
                potionToUse.Use(PlayerEntity.Instance);
                events.potionEvents.onPotionUsed.Invoke(potionSlot);
                return true;
            }

            return false;
        }

        public void RestorePotionCharges(EnemyEntity enemyEntity)
        {
            PlayerInventoryData.RestorePotionCharges(enemyEntity.PotionChargesOnDeath);
            events.potionEvents.onPotionChargesGained.Invoke();
        }

        public void UnlockPotion(PotionSettings potionSettings)
        {
            PlayerInventoryData.UnlockPotion(potionSettings);
        }

        public void UpgradePotion(PotionSettings potionSettings)
        {
            if (PlayerInventoryData.UpgradePotions(potionSettings, out PotionSlot potionSlot))
                events.potionEvents.onPotionUpgraded.Invoke(potionSlot);
        }

        #endregion

        #region Rune Methods

        public void AddTier1Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.AddTier1Rune(rune);
        }

        public void RemoveTier1Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.RemoveTier1Rune(rune);
        }

        public void AddTier2Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.AddTier2Rune(rune);
        }

        public void RemoveTier2Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.RemoveTier2Rune(rune);
        }

        public void AddTier3Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.AddTier3Rune(rune);
        }

        public void RemoveTier3Rune(string runeName)
        {
            Rune rune = (Rune)Enum.Parse(typeof(Rune), runeName);
            PlayerInventoryData.RemoveTier3Rune(rune);
        }

        #endregion
    }
}