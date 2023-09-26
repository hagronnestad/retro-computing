using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidFilter : ISampleProvider
    {
        private readonly ISampleProvider _sourceProvider;
        private readonly int _sampleRate;
        private int _freq = 0;
        private readonly BiQuadFilter _lowPassFilter;
        private readonly BiQuadFilter _bandPassFilter;
        private readonly BiQuadFilter _highPassFilter;

        public bool Bypass { get; set; }
        public bool LowPassEnabled { get; set; } = false;
        public bool BandPassEnabled { get; set; } = false;
        public bool HighPassEnabled { get; set; } = false;

        public SidFilter(ISampleProvider sourceProvider)
        {
            if (sourceProvider.WaveFormat.Channels > 1)
            {
                throw new NotSupportedException("This filter supports a single channel only.");
            }

            _sourceProvider = sourceProvider;
            _sampleRate = _sourceProvider.WaveFormat.SampleRate;

            _lowPassFilter = BiQuadFilter.LowPassFilter(_sampleRate, _freq, 1);
            _bandPassFilter = BiQuadFilter.PeakingEQ(_sampleRate, _freq, 1, 1);
            _highPassFilter = BiQuadFilter.LowPassFilter(_sampleRate, _freq, 1);
        }

        public void SetFrequency(int freq)
        {
            _freq = freq;
            _lowPassFilter.SetLowPassFilter(_sampleRate, _freq, 1);
            _bandPassFilter.SetPeakingEq(_sampleRate, _freq, 100, 0);
            _highPassFilter.SetLowPassFilter(_sampleRate, _freq, 1);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = _sourceProvider.Read(buffer, offset, count);
            if (Bypass) return samplesRead;
            if (_freq == 0) return samplesRead;
            if (!LowPassEnabled && !BandPassEnabled && !HighPassEnabled) return samplesRead;

            for (int i = 0; i < samplesRead; i++)
            {
                if (LowPassEnabled)
                {
                    buffer[offset + i] = _lowPassFilter.Transform(buffer[offset + i]);
                }

                if (BandPassEnabled)
                {
                    buffer[offset + i] = _bandPassFilter.Transform(buffer[offset + i]);
                }

                if (HighPassEnabled)
                {
                    buffer[offset + i] = _highPassFilter.Transform(buffer[offset + i]);
                }
            }

            return samplesRead;
        }

        public WaveFormat WaveFormat => _sourceProvider.WaveFormat;
    }
}
