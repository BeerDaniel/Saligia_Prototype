using SuspiciousGames.Saligia.Audio;
using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Targeting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newGrimoireComboAttackSkill", menuName = "Saligia/Skills/Attack Skills/Grimoire Combo Attack")]
    public class GrimoireComboAttackSkill : BaseAttackSkill
    {
        [SerializeField] private CollisionArea _collisionArea;
        [SerializeField] private GameObject _visualEffectToSpawn;
        [SerializeField] private AudioClip _attackAudioClip;

        public override void AnimationTriggeredLogic()
        {
            base.AnimationTriggeredLogic();

            if (!TargetData.GetTargetObject().TryGetComponent(out Entity targetEntity))
                return;

            ParticleSystem particleSystem = null;

            Vector3 spawnPos = targetEntity.transform.position;

            if (NavMesh.SamplePosition(targetEntity.transform.position, out var hit, 100, NavMesh.AllAreas))
                spawnPos = hit.position;

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

                var objects = _collisionArea.CheckForObjectsInArea(spawnPos);
                var enemies = new List<Entity>();
                foreach (var obj in objects)
                    if (obj.TryGetComponent(out Entity entity))
                        enemies.Add(entity);
                //TODO add mind power = mp for each enemy
                foreach (var enemy in enemies)
                {
                    if (enemy == null)
                        continue;
                    if (!enemy.HealthComponent)
                        continue;
                    if (enemy.HealthComponent.IsDead)
                        continue;
                    enemy.ApplyDamage(AttackValue.damageData);
                    if (CasterEntity.CastCostComponent)
                        CasterEntity.CastCostComponent.AddMindPower(AttackValue.mpPerHit, true);
                    //TODO apply any buffs or debuffs
                    if (enemy.HealthComponent.CurrentHitPoints <= 0)
                    {
                        //TODO apply on death effects
                    }
                }
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




