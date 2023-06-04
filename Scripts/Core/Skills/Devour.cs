using SuspiciousGames.Saligia.Audio;
using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Buffs;
using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newDevour", menuName = "Saligia/Skills/Monster/Boss/Devour")]
    public class Devour : BaseSkill
    {
        [Space(2.0f), Header("Skill Data")]
        [SerializeField] private DamageData _firstBiteDamageData;
        [SerializeField] private DamageData _secondBiteDamageData;
        [SerializeField] private DamageData _thirdBiteDamageData;
        [SerializeField] private GameObject _chompParticleEffectPrefab;
        [SerializeField] private StunBuffData _stunBuffData;
        [SerializeField] private CollisionArea _devourArea;

        private int _numberOfAdditionalCalls;
        private bool _isPlayerInBelly;
        private PlayerEntity _player;
        private ParticleSystem _biteParticleSystem;
        private BossEntity _bossCasterEntity;

        public override void CleanUp()
        {
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
            }
        }

        public override void AnimationTriggeredLogic()
        {
            base.AnimationTriggeredLogic();

            if (_numberOfAdditionalCalls == 0)
            {
                var gameObjects = _devourArea.CheckForObjectsInArea(CasterEntity.transform.position, CasterEntity.transform.forward);
                if (gameObjects.Count > 0)
                {
                    _player = gameObjects[0].GetComponent<PlayerEntity>();
                    _player.BuffComponent.AddBuff(_stunBuffData, CasterEntity);
                    _isPlayerInBelly = true;
                    //TODO what to do with the player? --> invisible, scale down, attack transform to boss while chomping
                    _player.ModelObject.SetActive(false);
                    if (_player.WeaponComponent.GetActiveWeapon(out var weapon))
                        weapon.gameObject.SetActive(false);
                }
            }
            else if (_numberOfAdditionalCalls == 1)
            {
                Chomp(_firstBiteDamageData);
            }
            else if (_numberOfAdditionalCalls == 2)
            {
                Chomp(_secondBiteDamageData);
            }
            else if (_numberOfAdditionalCalls == 3)
            {
                Chomp(_thirdBiteDamageData);
            }
            else
            {
                if (_isPlayerInBelly)
                {
                    _player.ModelObject.SetActive(true);
                    if (_player.WeaponComponent.GetActiveWeapon(out var weapon))
                        weapon.gameObject.SetActive(true);
                }
                //TODO play burp vfx and sfx  
            }
            _numberOfAdditionalCalls++;
        }

        private void Chomp(DamageData damageData)
        {
            if (!_player)
            {
                //TODO this is optional, cancels the animation when the boss can't eat anything
                CasterEntity.CastComponent.SendMessage("OnSkillAnimationEnd");
                return;
            }
            damageData.damageSource = CasterEntity;
            _player.ApplyDamage(damageData);
            if (_biteParticleSystem)
                _biteParticleSystem.Play();
        }

        protected override void Logic()
        {
            _bossCasterEntity = CasterEntity as BossEntity;
            if (_chompParticleEffectPrefab)
                _biteParticleSystem = Instantiate(_chompParticleEffectPrefab, _bossCasterEntity.BellyTransform).GetComponent<ParticleSystem>();
            _numberOfAdditionalCalls = 0;
            _isPlayerInBelly = false;
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }
        }
    }
}
