using SuspiciousGames.Saligia.Audio;
using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;
using UnityEngine.Audio;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newMugSmash", menuName = "Saligia/Skills/Monster/Boss/Mug Smash")]
    public class MugSmash : BaseSkill
    {
        [SerializeField] private AudioClip _mugExplosionAudioClip;
        [SerializeField] private AudioMixerGroup _mugExplosionAudioMixerGroup;
        [SerializeField, Range(0.0f, 1.0f)] private float _volumeScale = 1.0f;
        public override void CleanUp()
        {
            if ((BossEntity)CasterEntity)
                ((BossEntity)CasterEntity).MugObject.SetActive(true);
        }

        public override void AnimationTriggeredLogic()
        {
            base.AnimationTriggeredLogic();
            if ((BossEntity)CasterEntity)
                ((BossEntity)CasterEntity).MugObject.SetActive(false);
            skillObject.SetActive(true);
            skillObject.transform.position = TargetData.GetTargetPosition() + CasterEntity.transform.forward;
            if (_mugExplosionAudioClip)
                AudioSourcePooler.Instance.Get(_mugExplosionAudioMixerGroup).PlayOneShot(_mugExplosionAudioClip, _volumeScale);
        }

        protected override void Logic()
        {
            skillObject = CreateSkillobject();
            skillObject.SetActive(false);
        }
    }
}
