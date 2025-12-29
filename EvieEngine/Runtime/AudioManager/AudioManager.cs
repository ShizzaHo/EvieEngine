using System;
using System.Collections;
using TriInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace EvieEngine.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer mixer;

        public enum AudioEffect
        {
            LowPass,
            HighPass,
        }

        public void SetVolume(string name, float volume)
        {
            mixer.SetFloat(name, volume);
        }

        public void SetVolumeSmooth(string name, float volume, float duration = 1f)
        {
            StartCoroutine(VolumeSmoothRoutine(name, volume, duration));
        }

        private IEnumerator VolumeSmoothRoutine(string param, float target, float duration)
        {
            mixer.GetFloat(param, out float start);
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float v = Mathf.Lerp(start, target, t / duration);
                mixer.SetFloat(param, v);
                yield return null;
            }

            mixer.SetFloat(param, target);
        }

        public void PlayEffect(AudioSource audio, AudioEffect effect, float startValue, float endValue, float duration)
        {
            Component filter = GetOrAddFilter(audio, effect);
            SetFilterValue(filter, effect, startValue);
            StartCoroutine(FilterRoutine(filter, effect, startValue, endValue, duration));
        }

        public void PlayEffect(AudioSource audio, AudioEffect effect, float endValue, float duration)
        {
            Component filter = GetOrAddFilter(audio, effect);
            float startValue = GetFilterValue(filter, effect);
            StartCoroutine(FilterRoutine(filter, effect, startValue, endValue, duration));
        }

        private IEnumerator FilterRoutine(Component filter, AudioEffect effect, float start, float end, float duration)
        {
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float v = Mathf.Lerp(start, end, t / duration);
                SetFilterValue(filter, effect, v);
                yield return null;
            }

            SetFilterValue(filter, effect, end);
        }

        private Component GetOrAddFilter(AudioSource audio, AudioEffect effect)
        {
            if (audio == null) return null;

            switch (effect)
            {
                case AudioEffect.LowPass:
                {
                    var f = audio.GetComponent<AudioLowPassFilter>();
                    if (f == null)
                    {
                        #if UNITY_EDITOR
                        if (!Application.isPlaying)
                            f = UnityEditor.Undo.AddComponent<AudioLowPassFilter>(audio.gameObject);
                        else
                            f = audio.gameObject.AddComponent<AudioLowPassFilter>();
                        #else
                        f = audio.gameObject.AddComponent<AudioLowPassFilter>();
                        #endif
                    }

                    f.enabled = true;
                    return f;
                }

                case AudioEffect.HighPass:
                {
                    var f = audio.GetComponent<AudioHighPassFilter>();
                    if (f == null)
                    {
                        #if UNITY_EDITOR
                        if (!Application.isPlaying)
                            f = UnityEditor.Undo.AddComponent<AudioHighPassFilter>(audio.gameObject);
                        else
                            f = audio.gameObject.AddComponent<AudioHighPassFilter>();
                        #else
                        f = audio.gameObject.AddComponent<AudioHighPassFilter>();
                        #endif
                    }

                    f.enabled = true;
                    return f;
                }
            }

            return null;
        }


        private float GetFilterValue(Component filter, AudioEffect effect)
        {
            switch (effect)
            {
                case AudioEffect.LowPass:
                    return ((AudioLowPassFilter)filter).cutoffFrequency;

                case AudioEffect.HighPass:
                    return ((AudioHighPassFilter)filter).cutoffFrequency;
            }

            return 0f;
        }

        private void SetFilterValue(Component filter, AudioEffect effect, float value)
        {
            switch (effect)
            {
                case AudioEffect.LowPass:
                    ((AudioLowPassFilter)filter).cutoffFrequency = value;
                    break;

                case AudioEffect.HighPass:
                    ((AudioHighPassFilter)filter).cutoffFrequency = value;
                    break;
            }
        }
    }
}
