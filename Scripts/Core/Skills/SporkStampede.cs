using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newSporkStampede", menuName = "Saligia/Skills/Monster/Boss/Spork Stampede")]
    public class SporkStampede : BaseSkill
    {
        [SerializeField] private DamageData _damageOfFirstHitData;
        [SerializeField] private DamageData _damageOfSecondHitData;
        [SerializeField] private DamageData _damageOfThirdHitData;
        [SerializeField] private DamageData _damageOfFourthHitData;
        [SerializeField] private DamageData _damageOfFifthHitData;

        [SerializeField] private CollisionArea _collisionArea;
        [SerializeField] private GameObject _groundStabEffect;

        private ParticleSystem _particleSystem;
        private int _hitIndex;
        private List<DamageData> _hitDamageDatas;
        private Transform _forkTransform;

        public override void CleanUp()
        {
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(false);
                CasterEntity.MovementComponent.BlockRotation(false);
            }
            if (_particleSystem)
                Destroy(_particleSystem);
        }

        public override void AnimationTriggeredLogic()
        {
            base.AnimationTriggeredLogic();
            if (_hitIndex == 0 && _particleSystem)
                _particleSystem.gameObject.SetActive(true);

            if (_particleSystem)
                _particleSystem.gameObject.transform.position = _forkTransform.position;

            foreach (var collider in _collisionArea.CheckForObjectsInArea(_forkTransform.position))
            {
                if (collider.TryGetComponent(out Entity entity))
                {
                    entity.ApplyDamage(_hitDamageDatas[_hitIndex]);
                }
            }
            _particleSystem?.Play();
            _hitIndex++;
        }

        protected override void Logic()
        {
            if (CasterEntity.MovementComponent)
            {
                CasterEntity.MovementComponent.BlockMovement(true);
                CasterEntity.MovementComponent.BlockRotation(true);
            }

            if (CasterEntity is BossEntity)
                _forkTransform = ((BossEntity)CasterEntity).ForkTransform;

            _hitIndex = 0;
            _hitDamageDatas = new List<DamageData> { _damageOfFirstHitData,
                _damageOfSecondHitData,
                _damageOfThirdHitData,
                _damageOfFourthHitData,
                _damageOfFifthHitData};

            foreach (var damageData in _hitDamageDatas)
                damageData.damageSource = CasterEntity;

            if (_groundStabEffect != null)
            {
                _particleSystem = Instantiate(_groundStabEffect).GetComponent<ParticleSystem>();
                var ps = _particleSystem.main;
                ps.playOnAwake = false;
                _particleSystem.gameObject.SetActive(false);
            }
        }
    }
}
