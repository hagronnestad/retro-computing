using Commodore64.Sid.Enums;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Commodore64.Sid.NAudio
{
    public class NAudioSid : SidBase
    {
        private const int DESIRED_LATENCY = 50;

        private WavePlayerType _wavePlayerType = WavePlayerType.DirectSoundOut;

        public SignalGenerator _sgVoice1;
        public SignalGenerator _sgVoice2;
        public SignalGenerator _sgVoice3;
        public MixingSampleProvider _mixingSampleProvider;
        public MeteringSampleProvider _meteringSampleProvider;
        public VolumeSampleProvider _volumeSampleProvider;
        public IWavePlayer _audioOutEvent;

        public NAudioSid()
        {
            Init();

            RegisterChanged += NAudioSid_RegisterChange;
            Voice1Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice1, e);
            Voice2Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice2, e);
            Voice3Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice3, e);
        }

        private void Init()
        {
            _sgVoice1 = new SignalGenerator() { Gain = 1, Frequency = 0 };
            _sgVoice2 = new SignalGenerator() { Gain = 1, Frequency = 0 };
            _sgVoice3 = new SignalGenerator() { Gain = 1, Frequency = 0 };

            _mixingSampleProvider = new MixingSampleProvider(
                new List<ISampleProvider>() {
                    _sgVoice1.ToMono(),
                    _sgVoice2.ToMono(),
                    _sgVoice3.ToMono()
                }
            );
            _meteringSampleProvider = new MeteringSampleProvider(_mixingSampleProvider.ToMono());

            _volumeSampleProvider = new VolumeSampleProvider(_meteringSampleProvider);
            _volumeSampleProvider.Volume = 0.05f;

            switch (_wavePlayerType)
            {
                case WavePlayerType.WasapiOut:
                    _audioOutEvent = new WasapiOut(AudioClientShareMode.Shared, DESIRED_LATENCY);
                    break;
                case WavePlayerType.WaveOutEvent:
                    _audioOutEvent = new WaveOutEvent
                    {
                        DesiredLatency = DESIRED_LATENCY
                    };
                    break;
                case WavePlayerType.DirectSoundOut:
                    _audioOutEvent = new DirectSoundOut(DESIRED_LATENCY);
                    break;
                default:
                    break;
            }

            _audioOutEvent.Init(_volumeSampleProvider);
            _audioOutEvent.Play();
        }

        public float Volume
        {
            get => _volumeSampleProvider?.Volume ?? 0; set
            {
                if (_volumeSampleProvider != null) _volumeSampleProvider.Volume = value;
            }
        }

        public bool IsPlaying => _audioOutEvent.PlaybackState == PlaybackState.Playing;


        public void Stop()
        {
            _audioOutEvent?.Stop();
        }

        public void Pause()
        {
            _audioOutEvent?.Pause();
        }

        public void Play()
        {
            _audioOutEvent?.Play();
        }

        public void Reset()
        {
            Stop();
            Array.Clear(_registers);
        }

        private void NAudioSid_RegisterChange(object sender, SidRegister reg)
        {
            switch (reg)
            {
                case SidRegister.VOLUME_FILTER_MODES:
                    if (_sgVoice1.Gain != SidVolume) _sgVoice1.Gain = SidVolume;
                    if (_sgVoice2.Gain != SidVolume) _sgVoice2.Gain = SidVolume;
                    if (_sgVoice3.Gain != SidVolume) _sgVoice3.Gain = SidVolume;
                    break;
            }
        }

        private void UpdateSignalGeneratorFromVoice(SignalGenerator sg, Voice v)
        {
            // Update waveform
            switch (v.WaveForm)
            {
                case VoiceWaveForm.Triangle:
                    if (sg.Type != SignalGeneratorType.Triangle) sg.Type = SignalGeneratorType.Triangle;
                    break;
                case VoiceWaveForm.SawTooth:
                    if (sg.Type != SignalGeneratorType.SawTooth) sg.Type = SignalGeneratorType.SawTooth;
                    break;
                case VoiceWaveForm.Square:
                    if (sg.Type != SignalGeneratorType.Square) sg.Type = SignalGeneratorType.Square;
                    break;
                case VoiceWaveForm.Noise:
                    if (sg.Type != SignalGeneratorType.White) sg.Type = SignalGeneratorType.White;
                    break;
                default:
                    sg.Frequency = 0f;
                    break;
            }

            // Update frequency
            if (sg.Frequency != v.Frequency) sg.Frequency = v.Frequency;

            // Set frequency to 0 if voice is disabled or gate is low
            if ((!v.Gate || v.Disabled) && sg.Frequency != 0) sg.Frequency = 0;
        }
    }
}
