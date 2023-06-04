using SuspiciousGames.Saligia.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newPlayAudioExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/PlayAudioExecutionData")]
    public class PlayAudioExecutionData : ModuleExecutionData
    {
        [SerializeField] private AudioClip _audioClipToPlay;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField, Range(0.0f, 1.0f),] private float _volumeScale = 1.0f;
        protected override void Logic(GameObject target)
        {
            var audioSource = AudioSourcePooler.Instance.Get(_audioMixerGroup);
            audioSource.volume = baseSkill.CasterEntity.SpellAudioPlayer.AudioSource.volume;
            if (_audioMixerGroup)
                audioSource.outputAudioMixerGroup = _audioMixerGroup;
            else
                audioSource.outputAudioMixerGroup = baseSkill.CasterEntity.SpellAudioPlayer.AudioSource.outputAudioMixerGroup;
            audioSource.transform.position = target.transform.position;
            audioSource.PlayOneShot(_audioClipToPlay, _volumeScale);
        }
    }
}




