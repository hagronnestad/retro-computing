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
        public event EventHandler<Voice> Voice1Changed;
        public event EventHandler<Voice> Voice2Changed;
        public event EventHandler<Voice> Voice3Changed;

        public readonly Voice Voice1;
        public readonly Voice Voice2;
        public readonly Voice Voice3;

        public bool FilterLowPassEnabled { get; set; }
        public bool FilterBandPassEnabled { get; set; }
        public bool FilterHighPassEnabled { get; set; }

        public double SidVolume { get; set; }

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
                    case SidRegister.VOICE1_FREQ_HIGH:
                        {
                            var frequency = (4000f / 65535f) *
                                (_registers[(int)SidRegister.VOICE1_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE1_FREQ_LOW]);

                            Voice1.Frequency = frequency;
                            Voice1Changed?.Invoke(this, Voice1);
                        }
                        break;

                    case SidRegister.VOICE1_PULSE_WIDTH_LOW:
                    case SidRegister.VOICE1_PULSE_WIDTH_HIGH:
                        {
                            var pulseWidth =
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

                        if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE))
                            Voice1.WaveForm = VoiceWaveForm.Triangle;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW))
                            Voice1.WaveForm = VoiceWaveForm.SawTooth;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE))
                            Voice1.WaveForm = VoiceWaveForm.Square;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE))
                            Voice1.WaveForm = VoiceWaveForm.Noise;
                        else
                            Voice1.WaveForm = VoiceWaveForm.None;

                        Task.Run(() => Voice1Changed?.Invoke(this, Voice1));
                        break;

                    case SidRegister.VOICE1_ATTACK_DECAY:
                        break;

                    case SidRegister.VOICE1_SUSTAIN_RELEASE:
                        break;


                    case SidRegister.VOICE2_FREQ_LOW:
                    case SidRegister.VOICE2_FREQ_HIGH:
                        {
                            var frequency = (4000f / 65535f) *
                                (_registers[(int)SidRegister.VOICE2_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE2_FREQ_LOW]);

                            Voice2.Frequency = frequency;
                            Task.Run(() => Voice2Changed?.Invoke(this, Voice2));
                        }
                        break;

                    case SidRegister.VOICE2_PULSE_WIDTH_LOW:
                    case SidRegister.VOICE2_PULSE_WIDTH_HIGH:
                        {
                            var pulseWidth =
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

                        if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE))
                            Voice2.WaveForm = VoiceWaveForm.Triangle;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW))
                            Voice2.WaveForm = VoiceWaveForm.SawTooth;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE))
                            Voice2.WaveForm = VoiceWaveForm.Square;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE))
                            Voice2.WaveForm = VoiceWaveForm.Noise;
                        else
                            Voice2.WaveForm = VoiceWaveForm.None;

                        Task.Run(() => Voice2Changed?.Invoke(this, Voice2));
                        break;

                    case SidRegister.VOICE2_ATTACK_DECAY:
                        break;

                    case SidRegister.VOICE2_SUSTAIN_RELEASE:
                        break;


                    case SidRegister.VOICE3_FREQ_LOW:
                    case SidRegister.VOICE3_FREQ_HIGH:
                        {
                            var frequency = (4000f / 65535f) *
                                (_registers[(int)SidRegister.VOICE3_FREQ_HIGH] << 8 |
                                _registers[(int)SidRegister.VOICE3_FREQ_LOW]);

                            Voice3.Frequency = frequency;
                            Task.Run(() => Voice3Changed?.Invoke(this, Voice3));
                        }
                        break;

                    case SidRegister.VOICE3_PULSE_WIDTH_LOW:
                    case SidRegister.VOICE3_PULSE_WIDTH_HIGH:
                        {
                            var pulseWidth =
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

                        if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_TRIANGLE))
                            Voice3.WaveForm = VoiceWaveForm.Triangle;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SAW))
                            Voice3.WaveForm = VoiceWaveForm.SawTooth;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_SQUARE))
                            Voice3.WaveForm = VoiceWaveForm.Square;
                        else if (value.IsBitSet((BitFlag)SidControlRegisterBitFlags.WAVEFORM_NOISE))
                            Voice3.WaveForm = VoiceWaveForm.Noise;
                        else
                            Voice3.WaveForm = VoiceWaveForm.None;

                        Task.Run(() => Voice3Changed?.Invoke(this, Voice3));
                        break;

                    case SidRegister.VOICE3_ATTACK_DECAY:
                        break;

                    case SidRegister.VOICE3_SUSTAIN_RELEASE:
                        break;


                    case SidRegister.FILTER_CUT_OFF_FREQ_LOW:
                        break;

                    case SidRegister.FILTER_CUT_OFF_FREQ_HIGH:
                        break;

                    case SidRegister.FILTER_CONTROL:
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
