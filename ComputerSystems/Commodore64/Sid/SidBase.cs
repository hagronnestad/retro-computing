using Commodore64.Sid.Enums;
using System;
using Extensions.Byte;
using Extensions.Enums;
using System.Threading.Tasks;

namespace Commodore64.Sid
{
    public abstract class SidBase
    {
        protected byte[] _registers = new byte[0x20];

        public event EventHandler<SidRegister> RegisterChanged;
        public event EventHandler<SidRegister> FilterChanged;
        public event EventHandler<Voice> Voice1Changed;
        public event EventHandler<Voice> Voice2Changed;
        public event EventHandler<Voice> Voice3Changed;

        public readonly Voice Voice1;
        public readonly Voice Voice2;
        public readonly Voice Voice3;

        public bool FilterLowPassEnabled { get; set; }
        public bool FilterBandPassEnabled { get; set; }
        public bool FilterHighPassEnabled { get; set; }

        public int FilterFrequency { get; set; }
        public int FilterResonance { get; set; }

        public float SidVolume { get; set; }

        public SidBase()
        {
            Voice1 = new Voice(_registers, VoiceOffset.Voice1);
            Voice2 = new Voice(_registers, VoiceOffset.Voice2);
            Voice3 = new Voice(_registers, VoiceOffset.Voice3);
        }


        public byte this[SidRegister register]
        {
            get
            {
                var index = (int)register;
                switch (register)
                {
                    default:
                        return _registers[index];
                }
            }
            set
            {
                var index = (int)register;

                switch (register)
                {
                    case SidRegister.VOICE1_FREQ_LOW:
                        //case SidRegister.VOICE1_FREQ_HIGH:
                        {
                            float frequency = (4000.0f / (ushort.MaxValue + 1)) *
                                (_registers[(int)SidRegister.VOICE1_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE1_FREQ_LOW]);

                            Voice1.Frequency = frequency;
                            Voice1Changed?.Invoke(this, Voice1);
                        }
                        break;

                    case SidRegister.VOICE1_PULSE_WIDTH_LOW:
                        //case SidRegister.VOICE1_PULSE_WIDTH_HIGH:
                        {
                            float pulseWidth = (1.0f / 4096) *
                                ((_registers[(int)SidRegister.VOICE1_PULSE_WIDTH_HIGH] & 0b00001111) << 8 |
                                _registers[(int)SidRegister.VOICE1_PULSE_WIDTH_LOW]);

                            Voice1.PulseWidth = pulseWidth;
                            Voice1Changed?.Invoke(this, Voice1);
                        }
                        break;

                    case SidRegister.VOICE1_CONTROL_REGISTER:
                        Voice1.Disabled = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_DISABLE);
                        Voice1.Gate = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_GATE);
                        Voice1.Synchronization = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.SYNCHRONIZATION);
                        Voice1.RingModulation = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.RING_MODULATION);
                        Voice1.WaveformSquareActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE);
                        Voice1.WaveformTriangleActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE);
                        Voice1.WaveformSawToothActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW);
                        Voice1.WaveformNoiseActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE);

                        Task.Run(() => Voice1Changed?.Invoke(this, Voice1));
                        break;

                    case SidRegister.VOICE1_ATTACK_DECAY:
                        {
                            int attackSecondsIndex = value & 0b00001111;
                            int decaySecondsIndex = (value >> 4) & 0b00001111;
                            Voice1.AttackSeconds = Voice.ATTACK_SECONDS_LUT[attackSecondsIndex];
                            Voice1.DecaySeconds = Voice.DECAY_SECONDS_LUT[decaySecondsIndex];
                        }
                        break;

                    case SidRegister.VOICE1_SUSTAIN_RELEASE:
                        {
                            int releaseSecondsIndex = value & 0b00001111;
                            float sustainLevel = ((value >> 4) & 0b00001111) / 15f;
                            Voice1.ReleaseSeconds = Voice.RELEASE_SECONDS_LUT[releaseSecondsIndex];
                            Voice1.SustainLevel = sustainLevel;
                        }
                        break;


                    case SidRegister.VOICE2_FREQ_LOW:
                        //case SidRegister.VOICE2_FREQ_HIGH:
                        {
                            float frequency = (4000f / (ushort.MaxValue + 1)) *
                                (_registers[(int)SidRegister.VOICE2_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE2_FREQ_LOW]);

                            Voice2.Frequency = frequency;
                            Task.Run(() => Voice2Changed?.Invoke(this, Voice2));
                        }
                        break;

                    case SidRegister.VOICE2_PULSE_WIDTH_LOW:
                        //case SidRegister.VOICE2_PULSE_WIDTH_HIGH:
                        {
                            float pulseWidth = (1.0f / 4096) *
                                ((_registers[(int)SidRegister.VOICE2_PULSE_WIDTH_HIGH] & 0b00001111) << 8 |
                                _registers[(int)SidRegister.VOICE2_PULSE_WIDTH_LOW]);

                            Voice2.PulseWidth = pulseWidth;
                            Voice2Changed?.Invoke(this, Voice2);
                        }
                        break;

                    case SidRegister.VOICE2_CONTROL_REGISTER:
                        Voice2.Disabled = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_DISABLE);
                        Voice2.Gate = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_GATE);
                        Voice2.Synchronization = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.SYNCHRONIZATION);
                        Voice2.RingModulation = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.RING_MODULATION);
                        Voice2.WaveformSquareActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE);
                        Voice2.WaveformTriangleActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE);
                        Voice2.WaveformSawToothActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW);
                        Voice2.WaveformNoiseActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE);

                        Task.Run(() => Voice2Changed?.Invoke(this, Voice2));
                        break;

                    case SidRegister.VOICE2_ATTACK_DECAY:
                        {
                            int attackSecondsIndex = value & 0b00001111;
                            int decaySecondsIndex = (value >> 4) & 0b00001111;
                            Voice2.AttackSeconds = Voice.ATTACK_SECONDS_LUT[attackSecondsIndex];
                            Voice2.DecaySeconds = Voice.DECAY_SECONDS_LUT[decaySecondsIndex];
                        }
                        break;

                    case SidRegister.VOICE2_SUSTAIN_RELEASE:
                        {
                            int releaseSecondsIndex = value & 0b00001111;
                            float sustainLevel = ((value >> 4) & 0b00001111) / 15f;
                            Voice2.ReleaseSeconds = Voice.RELEASE_SECONDS_LUT[releaseSecondsIndex];
                            Voice2.SustainLevel = sustainLevel;
                        }
                        break;


                    case SidRegister.VOICE3_FREQ_LOW:
                        //case SidRegister.VOICE3_FREQ_HIGH:
                        {
                            float frequency = (4000f / (ushort.MaxValue + 1)) *
                                (_registers[(int)SidRegister.VOICE3_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE3_FREQ_LOW]);

                            Voice3.Frequency = frequency;
                            Task.Run(() => Voice3Changed?.Invoke(this, Voice3));
                        }
                        break;

                    case SidRegister.VOICE3_PULSE_WIDTH_LOW:
                        //case SidRegister.VOICE3_PULSE_WIDTH_HIGH:
                        {
                            float pulseWidth = (1.0f / 4096) *
                                ((_registers[(int)SidRegister.VOICE3_PULSE_WIDTH_HIGH] & 0b00001111) << 8 |
                                _registers[(int)SidRegister.VOICE3_PULSE_WIDTH_LOW]);

                            Voice3.PulseWidth = pulseWidth;
                            Voice3Changed?.Invoke(this, Voice3);
                        }
                        break;

                    case SidRegister.VOICE3_CONTROL_REGISTER:
                        Voice3.Disabled = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_DISABLE);
                        Voice3.Gate = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.VOICE_GATE);
                        Voice3.Synchronization = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.SYNCHRONIZATION);
                        Voice3.RingModulation = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.RING_MODULATION);
                        Voice3.WaveformSquareActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE);
                        Voice3.WaveformTriangleActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE);
                        Voice3.WaveformSawToothActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW);
                        Voice3.WaveformNoiseActive = value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE);

                        Task.Run(() => Voice3Changed?.Invoke(this, Voice3));
                        break;

                    case SidRegister.VOICE3_ATTACK_DECAY:
                        {
                            int attackSecondsIndex = value & 0b00001111;
                            int decaySecondsIndex = (value >> 4) & 0b00001111;
                            Voice3.AttackSeconds = Voice.ATTACK_SECONDS_LUT[attackSecondsIndex];
                            Voice3.DecaySeconds = Voice.DECAY_SECONDS_LUT[decaySecondsIndex];
                        }
                        break;

                    case SidRegister.VOICE3_SUSTAIN_RELEASE:
                        {
                            int releaseSecondsIndex = value & 0b00001111;
                            float sustainLevel = ((value >> 4) & 0b00001111) / 15f;
                            Voice3.ReleaseSeconds = Voice.RELEASE_SECONDS_LUT[releaseSecondsIndex];
                            Voice3.SustainLevel = sustainLevel;
                        }
                        break;


                    case SidRegister.FILTER_CUT_OFF_FREQ_LOW:
                        //case SidRegister.FILTER_CUT_OFF_FREQ_HIGH:
                        var filterFrequency =
                            ((_registers[(int)SidRegister.FILTER_CUT_OFF_FREQ_HIGH] << 3) |
                            (_registers[(int)SidRegister.FILTER_CUT_OFF_FREQ_LOW] & 0b00000111));

                        FilterFrequency = filterFrequency;
                        Task.Run(() => FilterChanged?.Invoke(this, register));
                        break;

                    case SidRegister.FILTER_CONTROL:
                        Voice1.Filtered = value.IsBitSet((BitFlag)FilterControlFlags.VOICE1_FILTERED);
                        Voice2.Filtered = value.IsBitSet((BitFlag)FilterControlFlags.VOICE2_FILTERED);
                        Voice3.Filtered = value.IsBitSet((BitFlag)FilterControlFlags.VOICE3_FILTERED);

                        FilterResonance = (_registers[(int)SidRegister.FILTER_CONTROL] >> 4);

                        Task.Run(() => FilterChanged?.Invoke(this, register));
                        break;

                    case SidRegister.VOLUME_FILTER_MODES:
                        // Bits #0-#3: Volume.
                        // Normalize 0-15 to 0-1f
                        SidVolume = ((value & 0b00001111) / 15f);

                        // Bit #4: 1 = Low pass filter enabled.
                        FilterLowPassEnabled = value.IsBitSet(BitIndex.BIT_4);

                        // Bit #5: 1 = Band pass filter enabled.
                        FilterBandPassEnabled = value.IsBitSet(BitIndex.BIT_5);

                        // Bit #6: 1 = High pass filter enabled.
                        FilterHighPassEnabled = value.IsBitSet(BitIndex.BIT_6);

                        // Bit #7: 1 = Voice #3 disabled.
                        Voice3.Disabled = value.IsBitSet(BitIndex.BIT_7);

                        Task.Run(() => Voice3Changed?.Invoke(this, Voice3));
                        Task.Run(() => FilterChanged?.Invoke(this, register));
                        break;

                    case SidRegister.PADDLE_X_VALUE:
                        break;

                    case SidRegister.PADDLE_Y_VALUE:
                        break;

                    case SidRegister.VOICE3_WAVEFORM_OUTPUT:
                        break;

                    case SidRegister.VOICE3_ADSR_OUTPUT:
                        break;

                    default:
                        break;
                }

                // Update register and invoke event if the value has changed
                if (_registers[index] != value)
                {
                    _registers[index] = value;
                    Task.Run(() => RegisterChanged?.Invoke(this, register));
                }
            }
        }
    }
}
