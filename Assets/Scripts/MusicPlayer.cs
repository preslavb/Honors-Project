using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Yarn.Unity;

namespace Assets.Scripts
{
    public class MusicPlayer : MonoBehaviour
    {
        private const float TRANSITION_TIME = 2.2f;

        [SerializeField] private AudioMixer _audioMix;
        
        [SerializeField] private AudioSource _currentAudioSource;
        [SerializeField] private AudioSource _targetAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        private struct SoundPreset
        {
            public string[] parameters;
            public float[] values;
        }

        private Dictionary<string, SoundPreset> _soundPresets = new Dictionary<string, SoundPreset>
        {
            {
                "default",
                new SoundPreset
                {
                    parameters = new[] {"MusicVolume", "MusicLowpass"},
                    values = new[] {0f, 22000f}
                }
            },
            {
                "introspect",
                new SoundPreset
                {
                    parameters = new[] {"MusicVolume", "MusicLowpass"},
                    values = new[] {-35f, 5000f}
                }
            },
            {
                "quiet",
                new SoundPreset
                {
                    parameters = new[] {"MusicVolume", "MusicHighpass"},
                    values = new[] {0f, 16000f}
                }
            },
        };
        
        private void Start()
        {
            _currentAudioSource.Play();
        }

        [YarnCommand("set_music")]
        public void SwitchMusic(string musicName)
        {
            var clip = musicName == "none" ? null : Resources.Load<AudioClip>(musicName);

            StartCoroutine(SwitchMusicCoroutine(clip));
        }

        [YarnCommand("tune_music")]
        public void TuneMusic(string parameter, string value)
        {
            if (parameter == "preset")
            {
                var preset = _soundPresets[value];

                for (int i = 0; i < preset.parameters.Length; i++)
                {
                    StartCoroutine(ChangeMix(preset.parameters[i], preset.values[i]));
                }

                return;
            }
            
            float parsedValue = float.Parse(value);

            StartCoroutine(ChangeMix(parameter, parsedValue));
        }
        
        [YarnCommand("play_sfx")]
        public void PlaySfx(string effectName)
        {
            var clip = Resources.Load<AudioClip>(effectName);

            _sfxAudioSource.PlayOneShot(clip);
        }

        private IEnumerator ChangeMix(string parameter, float newValue)
        {
            var timer = 0f;

            float oldValue;
            float value;

            _audioMix.GetFloat(parameter, out oldValue);

            while (timer < TRANSITION_TIME)
            {
                timer += Time.deltaTime;

                value = Mathf.Lerp(oldValue, newValue, timer / TRANSITION_TIME);

                _audioMix.SetFloat(parameter, value);

                yield return new WaitForEndOfFrame();
            }
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