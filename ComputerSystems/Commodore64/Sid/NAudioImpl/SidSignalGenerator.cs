using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidSignalGenerator : ISampleProvider
    {
        private const float TwoPi = (float)(2.0f * Math.PI);

        // Wave format
        private readonly WaveFormat waveFormat;

        // Random Number for the White Noise
        private readonly Random random = new Random();

        private float[] samples = new float[111];
        private int samplesIndex = 0;

        public List<float> Samples => samples.ToList();

        // Generator variable
        private long nSample;

        /// <summary>
        /// Initializes a new instance for the Generator
        /// Default: 44.1Khz, 1 channel
        /// </summary>
        public SidSignalGenerator() : this(44100, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance for the Generator
        /// </summary>
        /// <param name="sampleRate">Desired sample rate</param>
        /// <param name="channel">Number of channels</param>
        public SidSignalGenerator(int sampleRate, int channel)
        {
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
        }

        /// <summary>
        /// The waveformat of this WaveProvider (same as the source)
        /// </summary>
        public WaveFormat WaveFormat => waveFormat;

        /// <summary>
        /// Frequency for the Generator. (20.0 - 20000.0 Hz)
        /// Sin, Square, Triangle, SawTooth, Sweep (Start Frequency).
        /// </summary>
        public float Frequency { get; set; } = 0;

        public float PulseWidth { get; set; } = 0;

        /// <summary>
        /// Gain for the Generator. (0.0 to 1.0)
        /// </summary>
        public float Gain { get; set; } = 0;

        /// <summary>
        /// Type of Generator.
        /// </summary>
        //public SidSignalGeneratorType Type { get; set; }

        public bool WaveformSquareActive { get; set; }
        public bool WaveformTriangleActive { get; set; }
        public bool WaveformSawToothActive { get; set; }
        public bool WaveformNoiseActive { get; set; }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            int outIndex = offset;

            // Generator current value
            float multiple;
            float sampleValue;
            float sampleSaw;


            // Complete Buffer
            for (int sampleCount = 0; sampleCount < count / waveFormat.Channels; sampleCount++)
            {
                float? sampleValueSquare = null;
                float? sampleValueTriangle = null;
                float? sampleValueSawTooth = null;
                float? sampleValueWhiteNoise = null;

                // Square
                if (WaveformSquareActive)
                {
                    multiple = 2 * Frequency / waveFormat.SampleRate;
                    sampleSaw = nSample * multiple % 2;
                    sampleValueSquare = sampleSaw >= PulseWidth * 2 ? -Gain : Gain;
                }

                // Triangle
                if (WaveformTriangleActive)
                {
                    multiple = 2 * Frequency / waveFormat.SampleRate;
                    sampleSaw = nSample * multiple % 2;
                    sampleValueTriangle = 2 * sampleSaw;
                    if (sampleValueTriangle > 1) sampleValueTriangle = 2 - sampleValueTriangle;
                    if (sampleValueTriangle < -1) sampleValueTriangle = -2 - sampleValueTriangle;
                    sampleValueTriangle *= Gain;
                }

                // Saw Tooth
                if (WaveformSawToothActive)
                {
                    multiple = 2 * Frequency / waveFormat.SampleRate;
                    sampleSaw = nSample * multiple % 2 - 1;
                    sampleValueSawTooth = Gain * sampleSaw;
                }

                // White Noise
                if (WaveformNoiseActive)
                {
                    sampleValueWhiteNoise = Gain * NextRandomTwo();
                }


                // Combine waveforms

                byte sampleValueSquareByte = 0;
                byte sampleValueTriangleByte = 0;
                byte sampleValueSawToothByte = 0;
                byte sampleValueWhiteNoiseByte = 0;

                // Normalize to 0 to 255 and convert to byte
                if (sampleValueSquare != null) sampleValueSquareByte = (byte)((sampleValueSquare + 1.0f) * 127.5f);
                if (sampleValueTriangle != null) sampleValueTriangleByte = (byte)((sampleValueTriangle + 1.0f) * 127.5f);
                if (sampleValueSawTooth != null) sampleValueSawToothByte = (byte)((sampleValueSawTooth + 1.0f) * 127.5f);
                if (sampleValueWhiteNoise != null) sampleValueWhiteNoiseByte = (byte)((sampleValueWhiteNoise + 1.0f) * 127.5f);

                // Perform bitwise AND
                byte resultByte = 0xFF;
                if (sampleValueSquare != null) resultByte &= sampleValueSquareByte;
                if (sampleValueTriangle != null) resultByte &= sampleValueTriangleByte;
                if (sampleValueSawTooth != null) resultByte &= sampleValueSawToothByte;
                if (sampleValueWhiteNoise != null) resultByte &= sampleValueWhiteNoiseByte;

                // Convert back to float and normalize to -1.0 to 1.0
                sampleValue = (resultByte / 127.5f) - 1.0f;


                nSample++;
                buffer[outIndex++] = sampleValue;

                //if (nSample % 55 == 0)
                //{
                //    samples[samplesIndex] = sampleValue / 2.0f + 0.5f;
                //    samplesIndex++;
                //    if (samplesIndex > samples.Length - 1) samplesIndex = 0;
                //}
            }
            return count;
        }

        /// <summary>
        /// Random for WhiteNoise
        /// </summary>
        /// <returns>Random value from -1 to +1</returns>
        private float NextRandomTwo()
        {
            return 2.0f * (float)random.NextDouble() - 1.0f;
        }

    }

    /// <summary>
    /// Signal Generator type
    /// </summary>
    public enum SidSignalGeneratorType
    {
        Triangle,
        SawTooth,
        Square,
        SquarePulseWidth,
        White,
        Sin,
        None
    }

}
