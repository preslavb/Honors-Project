using System;
using System.Collections;
using Doozy.Engine.Soundy;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

namespace Assets.Scripts
{
    public class MusicPlayer : MonoBehaviour
    {
        private const float TRANSITION_TIME = 1.2f;
        
        [SerializeField] private AudioSource _currentAudioSource;
        [SerializeField] private AudioSource _targetAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private void Start()
        {
            _currentAudioSource.Play();
        }

        [YarnCommand("set_music")]
        private void SwitchMusic(string musicName)
        {
            var clip = musicName == "none" ? null : Resources.Load<AudioClip>(musicName);

            StartCoroutine(SwitchMusicCoroutine(clip));
        }
        
        [YarnCommand("play_sfx")]
        private void PlaySfx(string effectName)
        {
            var clip = Resources.Load<AudioClip>(effectName);

            _sfxAudioSource.PlayOneShot(clip);
        }
        
        private IEnumerator SwitchMusicCoroutine(AudioClip targetAudio)
        {
            _targetAudioSource.clip = targetAudio;
            _targetAudioSource.volume = 0;
            _targetAudioSource.Play();

            var timer = 0f;

            while (timer < TRANSITION_TIME)
            {
                timer += Time.deltaTime;

                _targetAudioSource.volume = Mathf.Lerp(0, 1, timer / TRANSITION_TIME);
                _currentAudioSource.volume = Mathf.Lerp(1, 0, timer / TRANSITION_TIME);

                yield return new WaitForEndOfFrame();
            }

            var swapTemp = _currentAudioSource;
            _currentAudioSource = _targetAudioSource;
            _targetAudioSource = swapTemp;
            _targetAudioSource.Stop();
        }
    }
}