using NAudio.Wave;
using System;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidAdsrSampleProvider2 : ISampleProvider
    {
        private readonly ISampleProvider source;

        private readonly SidEnvelopeGenerator adsr;

        private float attackSeconds;
        private float decaySeconds;
        private float sustainLevel;
        private float releaseSeconds;


        public float AttackSeconds
        {
            get
            {
                return attackSeconds;
            }
            set
            {
                attackSeconds = value;
                adsr.AttackRate = attackSeconds * WaveFormat.SampleRate;
            }
        }

        public float DecaySeconds
        {
            get
            {
                return decaySeconds;
            }
            set
            {
                decaySeconds = value;
                adsr.DecayRate = decaySeconds * WaveFormat.SampleRate;
            }
        }

        public float SustainLevel
        {
            get
            {
                return sustainLevel;
            }
            set
            {
                sustainLevel = value;
                adsr.SustainLevel = sustainLevel;
            }
        }

        public float ReleaseSeconds
        {
            get
            {
                return releaseSeconds;
            }
            set
            {
                releaseSeconds = value;
                adsr.ReleaseRate = releaseSeconds * WaveFormat.SampleRate;
            }
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public SidAdsrSampleProvider2(ISampleProvider source)
        {
            if (source.WaveFormat.Channels > 1)
            {
                throw new ArgumentException("Currently only supports mono inputs");
            }

            this.source = source;
            adsr = new SidEnvelopeGenerator();
            AttackSeconds = 0.01f;
            adsr.SustainLevel = 1f;
            adsr.DecayRate = 0f * WaveFormat.SampleRate;
            ReleaseSeconds = 0.3f;
            adsr.Gate(gate: true);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (adsr.State == SidEnvelopeGenerator.EnvelopeState.Idle)
            {
                return 0;
            }

            int num = source.Read(buffer, offset, count);
            for (int i = 0; i < num; i++)
            {
                buffer[offset++] *= adsr.Process();
            }

            return num;
        }

        public void Gate(bool gate)
        {
            adsr.Gate(gate);
        }
    }
}
