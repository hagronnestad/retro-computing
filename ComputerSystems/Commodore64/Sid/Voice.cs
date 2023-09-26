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

        
        private double _frequency;
        public double Frequency
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

        private double _pulseWidth;
        public double PulseWidth
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

        private VoiceWaveForm _waveForm;
        public VoiceWaveForm WaveForm
        {
            get
            {
                return _waveForm;
            }
            set
            {
                _waveForm = value;
            }
        }

        public bool Filtered { get; set; }
    }
}
