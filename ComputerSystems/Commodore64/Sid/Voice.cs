using Commodore64.Sid.Enums;

namespace Commodore64.Sid
{
    public enum VoiceOffset
    {
        Voice1 = 0x00,
        Voice2 = 0x07,
        Voice3 = 0x0E,
    }

    public class Voice
    {
        private readonly byte[] _sidRegisters;
        private readonly VoiceOffset _voiceOffset;


        public Voice(byte[] sidRegisters, VoiceOffset voiceOffset)
        {
            _sidRegisters = sidRegisters;
            _voiceOffset = voiceOffset;
        }


        private float _frequency;
        public float Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
            }
        }

        private float _pulseWidth;
        public float PulseWidth
        {
            get
            {
                return _pulseWidth;
            }
            set
            {
                _pulseWidth = value;
            }
        }

        private bool _gate;
        public bool Gate
        {
            get
            {
                return _gate;
            }
            set
            {
                _gate = value;
            }
        }

        private bool _synchronization;
        public bool Synchronization
        {
            get
            {
                return _synchronization;
            }
            set
            {
                _synchronization = value;
            }
        }

        private bool _ringModulation;
        public bool RingModulation
        {
            get
            {
                return _ringModulation;
            }
            set
            {
                _ringModulation = value;
            }
        }

        private bool _disabled;
        public bool Disabled
        {
            get
            {
                return _disabled;
            }
            set
            {
                _disabled = value;
            }
        }

        //private VoiceWaveForm _waveForm;
        //public VoiceWaveForm WaveForm
        //{
        //    get
        //    {
        //        return _waveForm;
        //    }
        //    set
        //    {
        //        _waveForm = value;
        //    }
        //}

        public bool WaveformSquareActive { get; set; }
        public bool WaveformTriangleActive { get; set; }
        public bool WaveformSawToothActive { get; set; }
        public bool WaveformNoiseActive { get; set; }


        public bool Filtered { get; set; }

        public static readonly float[] ATTACK_SECONDS_LUT = {
            0.002f, 0.008f, 0.016f, 0.024f, 0.038f, 0.056f, 0.068f, 0.080f, 0.100f, 0.250f, 0.500f, 0.800f, 1f, 3f, 5f, 8f
        };

        public static readonly float[] DECAY_SECONDS_LUT = {
            0.006f, 0.024f, 0.048f, 0.075f, 0.114f, 0.168f, 0.204f, 0.240f, 0.300f, 0.750f, 1.5f, 2.4f, 3f, 9f, 15f, 24f
        };

        public static readonly float[] RELEASE_SECONDS_LUT = {
            0.006f, 0.024f, 0.048f, 0.075f, 0.114f, 0.168f, 0.204f, 0.240f, 0.300f, 0.750f, 1.5f, 2.4f, 3f, 9f, 15f, 24f
        };

        public float AttackSeconds { get; set; }
        public float DecaySeconds { get; set; }
        public float SustainLevel { get; set; }
        public float ReleaseSeconds { get; set; }
    }
}
