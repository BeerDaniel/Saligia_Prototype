using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Buffs;
using SuspiciousGames.Saligia.Core.Entities.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newScourgeSkill", menuName = "Saligia/Skills/Secondary/Scourge")]
    public class ScourgeSkill : BaseSkill
    {
        [SerializeField] private float _duration = 3f;
        [SerializeField, Min(2)] private int _missileCount = 20;
        [SerializeField] private float _missileSpawnAngle = 45f;
        [SerializeField] private float _angleVarationSteepness = 20f;
        [SerializeField] private float _angleVarationWidth = 20f;
        [SerializeField] private List<BuffData> _onCastSuccessfullEnvyBuffs;
        [SerializeField] private string _castEndTrigger;

        private bool _isAborted = false;
        private int _missilesShot = 0;
        public override void CleanUp()
        {
            _isAborted = true;
            CasterEntity.MovementComponent.BlockMovement(false);
            CasterEntity.MovementComponent.BlockRotation(false);
        }

        public override void CastActivate(Entity caster)
        {
            base.CastActivate(caster);
            CasterEntity.MovementComponent.BlockMovement(true);
            CasterEntity.MovementComponent.BlockRotation(true);
        }

        protected override void Logic()
        {
            skillObject = CreateSkillobject();
            CasterEntity.CastComponent.StartCoroutine(Cast());
        }

        private IEnumerator Cast()
        {
            skillObject.transform.position = CasterEntity.transform.position +
                CasterEntity.transform.up;
            skillObject.transform.rotation = GetRandomShootDirection();
            _missilesShot++;
            yield return new WaitForSeconds(_duration / (_missileCount - 1));
            while (!_isAborted)
            {
                var targetObj = TargetData.GetTargetObject();

                if (!targetObj || !targetObj.GetComponent<HealthComponent>())
                {
                    CasterEntity.Animator.SetTrigger(_castEndTrigger);
                    _isAborted = true;
                    break;
                }

                if (_missilesShot < _missileCount)
                {
                    var missile = CreateSkillobject();
                    missile.transform.position = CasterEntity.transform.position +
                        CasterEntity.transform.up;
                    missile.transform.rotation = GetRandomShootDirection();
                    _missilesShot++;
                }
                else
                {
                    CasterEntity.Animator.SetTrigger(_castEndTrigger);
                    if (runes.Contains(Rune.Envy))
                        BuffCasterEntity();
                    _isAborted = true;
                    break;
                }
                yield return new WaitForSeconds(_duration / (_missileCount - 1));
            }
        }

        private Quaternion GetRandomShootDirection()
        {
            var center = Quaternion.AngleAxis(_missileSpawnAngle, CasterEntity.transform.right * -1) * CasterEntity.transform.rotation;
            var xVariated = Quaternion.AngleAxis(Random.Range(-_angleVarationWidth, _angleVarationWidth), CasterEntity.transform.right * -1) * center;
            var xYVariated = Quaternion.AngleAxis(Random.Range(-_angleVarationSteepness, _angleVarationSteepness), CasterEntity.transform.up) * xVariated;
            return xYVariated;
        }

        private void BuffCasterEntity()
        {
            foreach (var buff in _onCastSuccessfullEnvyBuffs)
            {
                CasterEntity.BuffComponent.AddBuff(buff, CasterEntity);
            }
        }

    }
}
