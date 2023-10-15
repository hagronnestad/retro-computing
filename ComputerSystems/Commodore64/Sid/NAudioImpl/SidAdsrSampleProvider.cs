using NAudio.Wave;
using System;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidAdsrSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private int position;
        private readonly int sampleRate;
        private bool gate;
        private float lastLevel;

        private int attackSamples, decaySamples, releaseSamples;
        private float sustainLevel;

        public float AttackSeconds
        {
            get => attackSamples / (float)sampleRate;
            set => attackSamples = (int)(sampleRate * value);
        }

        public float DecaySeconds
        {
            get => decaySamples / (float)sampleRate;
            set => decaySamples = (int)(sampleRate * value);
        }

        public float SustainLevel
        {
            get => sustainLevel;
            set => sustainLevel = value;
        }

        public float ReleaseSeconds
        {
            get => releaseSamples / (float)sampleRate;
            set => releaseSamples = (int)(sampleRate * value);
        }


        public SidAdsrSampleProvider(ISampleProvider source)
        {
            this.source = source;
            sampleRate = source.WaveFormat.SampleRate;
        }

        public void Gate(bool isOpen)
        {
            gate = isOpen;
            if (!gate) // If gate is closed, it's the start of the Release phase
            {
                position = 0;
            }
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamplesRead = source.Read(buffer, offset, count);

            for (int n = 0; n < sourceSamplesRead; n++)
            {
                float envMultiplier = GetEnvelopeMultiplier(position++);
                buffer[offset + n] *= envMultiplier;
            }

            return sourceSamplesRead;
        }

        private float GetEnvelopeMultiplier(int position)
        {
            if (gate) // Attack and Decay
            {
                if (position < attackSamples)
                {
                    return lastLevel = (float)position / attackSamples; // Linear Attack
                }

                int decayPosition = position - attackSamples;
                if (decayPosition < decaySamples)
                {
                    float decayRate = (sustainLevel - lastLevel) / decaySamples; // Calculate the rate of decay
                    return lastLevel += decayRate; // Linear Decay
                }

                return lastLevel = sustainLevel; // Sustain
            }
            else // Release
            {
                if (position < releaseSamples)
                {
                    float releaseRate = lastLevel / releaseSamples; // Calculate the rate of release
                    return lastLevel -= releaseRate; // Linear Release
                }
                return 0f;
            }
        }

        private float GetEnvelopeMultiplierExponential(int position)
        {
            if (gate) // Attack and Decay
            {
                if (position < attackSamples)
                {
                    return lastLevel = (float)Math.Pow(position / (double)attackSamples, 0.3f); // Exponential Attack
                }
                
                int decayPosition = position - attackSamples;
                if (decayPosition < decaySamples)
                {
                    return lastLevel *= (float)Math.Pow(sustainLevel / lastLevel, 1.0f / decaySamples); // Exponential Decay
                }
                
                return lastLevel = sustainLevel; // Sustain
            }
            else // Release
            {
                if (position < releaseSamples)
                {
                    float releaseFactor = 1.0f - (position / (float)releaseSamples);
                    return lastLevel *= (float)Math.Pow(releaseFactor, 0.03f); // Exponential Release
                }
                return 0f;
            }
        }
    }
}
