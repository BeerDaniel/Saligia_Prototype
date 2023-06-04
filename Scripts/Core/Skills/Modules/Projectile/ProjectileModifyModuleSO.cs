using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newProjectileModifyModule", menuName = "Saligia/Skills/Modules/ProjectileModifyModule")]
    public class ProjectileModifyModuleSO : ModuleSO
    {
        [SerializeField]private ProjectileModifyData _projectileModifyData;
        public override void Init(GameObject skillGameObject, BaseSkill baseSkill)
        {
            var projModifyBehaviour = new ProjectileModifyModuleBehaviour(_projectileModifyData);
            SkillBehaviour.Create(skillGameObject).AddModuleBehaviour(projModifyBehaviour);
        }
    }

    [System.Serializable]
    public class ProjectileModifyData
    {
        [SerializeField] private bool _overrideSpeedCurve = false;
        [SerializeField] private AnimationCurve _speedCurve;

        [SerializeField] private bool _overrideTranslationCurve;
        [SerializeField] private AnimationCurve _translationCurve;

        [SerializeField] private bool _overrideIsReturning;
        [SerializeField] private bool _isReturning = false;

        public void ModifyProjectileData(ref ProjectileData projectileData)
        {
            if (_overrideSpeedCurve)
                projectileData.speedCurve = _speedCurve;
            if (_overrideTranslationCurve)
                projectileData.translationCurve = _translationCurve;
            if (_overrideIsReturning)
                projectileData.isReturning = _isReturning;
        }
    }
}
