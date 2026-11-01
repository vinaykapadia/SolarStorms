using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Builds placeholder sine-wave clips in code, so Phase 0 needs no audio files.
    /// </summary>
    public static class PlaceholderTones
    {
        const int SampleRate = 44100;

        // A few milliseconds of fade at each end stops the clips from clicking.
        const float FadeSeconds = 0.005f;

        public static AudioClip Beep(string name, float frequency, float seconds, float volume)
        {
            int samples = Mathf.Max(1, Mathf.RoundToInt(seconds * SampleRate));
            var data = new float[samples];
            WriteTone(data, 0, samples, frequency, volume);
            return CreateClip(name, data);
        }

        /// <summary>A half-second, two-pulse alarm, meant to loop: the alarm pitch, then a fifth above it.</summary>
        public static AudioClip Alarm(string name, float frequency, float volume)
        {
            int samples = SampleRate / 2;
            int pulse = Mathf.RoundToInt(0.15f * SampleRate);
            var data = new float[samples];
            WriteTone(data, 0, pulse, frequency, volume);
            WriteTone(data, samples / 2, pulse, frequency * 1.5f, volume);
            return CreateClip(name, data);
        }

        static void WriteTone(float[] data, int start, int length, float frequency, float volume)
        {
            int fade = Mathf.Max(1, Mathf.Min(length / 4, Mathf.RoundToInt(FadeSeconds * SampleRate)));
            for (int i = 0; i < length && start + i < data.Length; i++)
            {
                float envelope = Mathf.Min(1f, Mathf.Min(i, length - 1 - i) / (float)fade);
                data[start + i] = Mathf.Sin(2f * Mathf.PI * frequency * i / SampleRate) * volume * envelope;
            }
        }

        static AudioClip CreateClip(string name, float[] data)
        {
            AudioClip clip = AudioClip.Create(name, data.Length, 1, SampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
