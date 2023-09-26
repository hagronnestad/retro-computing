using NAudio.Wave;
using System;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidSignalGenerator : ISampleProvider
    {
        private const double TwoPi = 2 * Math.PI;

        // Wave format
        private readonly WaveFormat waveFormat;

        // Random Number for the White Noise
        private readonly Random random = new Random();

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

            Type = SidSignalGeneratorType.None;
            Frequency = 0;
            Gain = 0;
        }

        /// <summary>
        /// The waveformat of this WaveProvider (same as the source)
        /// </summary>
        public WaveFormat WaveFormat => waveFormat;

        /// <summary>
        /// Frequency for the Generator. (20.0 - 20000.0 Hz)
        /// Sin, Square, Triangle, SawTooth, Sweep (Start Frequency).
        /// </summary>
        public double Frequency { get; set; }

        public double PulseWidth { get; set; }

        /// <summary>
        /// Gain for the Generator. (0.0 to 1.0)
        /// </summary>
        public double Gain { get; set; }

        /// <summary>
        /// Type of Generator.
        /// </summary>
        public SidSignalGeneratorType Type { get; set; }

        /// <summary>
        /// Reads from this provider.
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            int outIndex = offset;

            // Generator current value
            double multiple;
            double sampleValue;
            double sampleSaw;

            // Complete Buffer
            for (int sampleCount = 0; sampleCount < count / waveFormat.Channels; sampleCount++)
            {
                switch (Type)
                {
                    case SidSignalGeneratorType.Sin:
                        multiple = TwoPi * Frequency / waveFormat.SampleRate;
                        sampleValue = Gain * Math.Sin(nSample * multiple);
                        nSample++;
                        break;

                    case SidSignalGeneratorType.Square:
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = nSample * multiple % 2 - 1;
                        sampleValue = sampleSaw >= 0 ? Gain : -Gain;
                        nSample++;
                        break;

                    case SidSignalGeneratorType.SquarePulseWidth:
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = nSample * multiple % 2;
                        sampleValue = sampleSaw >= PulseWidth * 2 ? -Gain : Gain;
                        nSample++;
                        break;

                    case SidSignalGeneratorType.Triangle:
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = nSample * multiple % 2;
                        sampleValue = 2 * sampleSaw;

                        if (sampleValue > 1)
                            sampleValue = 2 - sampleValue;
                        if (sampleValue < -1)
                            sampleValue = -2 - sampleValue;

                        sampleValue *= Gain;
                        nSample++;
                        break;

                    case SidSignalGeneratorType.SawTooth:
                        multiple = 2 * Frequency / waveFormat.SampleRate;
                        sampleSaw = nSample * multiple % 2 - 1;
                        sampleValue = Gain * sampleSaw;
                        nSample++;
                        break;

                    case SidSignalGeneratorType.White:
                        sampleValue = Gain * NextRandomTwo();
                        break;

                    default:
                        sampleValue = 0.0;
                        break;
                }

                buffer[outIndex++] = (float)sampleValue;
            }
            return count;
        }

        /// <summary>
        /// Random for WhiteNoise
        /// </summary>
        /// <returns>Random value from -1 to +1</returns>
        private double NextRandomTwo()
        {
            return 2 * random.NextDouble() - 1;
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
