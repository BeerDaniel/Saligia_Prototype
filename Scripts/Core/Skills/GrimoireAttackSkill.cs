using SuspiciousGames.Saligia.Audio;
using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newGrimoireAttackSkill", menuName = "Saligia/Skills/Attack Skills/Grimoire Attack")]
    public class GrimoireAttackSkill : BaseAttackSkill
    {
        [SerializeField] private GameObject _visualEffectToSpawn;
        [SerializeField] private AudioClip _attackAudioClip;
        public override void AnimationTriggeredLogic()
        {
            base.AnimationTriggeredLogic();
            if (!TargetData.GetTargetObject().TryGetComponent(out Entity targetEntity) || !targetEntity.HealthComponent || targetEntity.HealthComponent.IsDead)
                return;

            ParticleSystem particleSystem = null;

            Vector3 spawnPos = targetEntity.transform.position;

            Collider col = targetEntity.GetComponent<Collider>();
            if (col)
                spawnPos = col.bounds.center;

            particleSystem = Instantiate(_visualEffectToSpawn, spawnPos, Quaternion.identity).GetComponent<ParticleSystem>();
            particleSystem?.Play();

            var audioSource = AudioSourcePooler.Instance.Get(AudioSourcePooler.Instance.SfxGroup);
            audioSource.volume = CasterEntity.SpellAudioPlayer.AudioSource.volume;
            audioSource.transform.position = spawnPos;
            audioSource.PlayOneShot(_attackAudioClip);

            if (targetEntity.HealthComponent)
            {
                if (targetEntity.HealthComponent.IsDead)
                    return;

                targetEntity.ApplyDamage(AttackValue.damageData);

                if (CasterEntity.CastCostComponent)
                    CasterEntity.CastCostComponent.AddMindPower(AttackValue.mpPerHit, true);
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
            }
        }

        protected override void Logic()
        {
            AttackValue.damageData.damageSource = CasterEntity;
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }
    }
}

