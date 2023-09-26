using Commodore64.Sid.Enums;
using NAudio.CoreAudioApi;
using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commodore64.Sid.NAudioImpl
{
    public class NAudioSid : SidBase
    {
        public event EventHandler<double> VolumeMeterUpdate;

        private const int DESIRED_LATENCY = 50;
        private WavePlayerType _wavePlayerType = WavePlayerType.DirectSoundOut;

        public SidSignalGenerator _sgVoice1;
        public SidSignalGenerator _sgVoice2;
        public SidSignalGenerator _sgVoice3;

        public SidFilter _sgVoice1Filtered;
        public SidFilter _sgVoice2Filtered;
        public SidFilter _sgVoice3Filtered;

        private bool _bypassFilters = true;
        public bool BypassFilters
        {
            get
            {
                return _bypassFilters;
            }
            set
            {
                _bypassFilters = value;
                _sgVoice1Filtered.Bypass = value;
                _sgVoice2Filtered.Bypass = value;
                _sgVoice3Filtered.Bypass = value;
            }
        }
        public double VolumeMeter { get; private set; }

        private MixingSampleProvider _mixingSampleProvider;
        private MeteringSampleProvider _meteringSampleProvider;
        private VolumeSampleProvider _volumeSampleProvider;

        private IWavePlayer _audioOutEvent;

        public NAudioSid()
        {
            Init();

            RegisterChanged += NAudioSid_RegisterChange;
            FilterChanged += NAudioSid_FilterChanged;
            Voice1Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice1, e);
            Voice2Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice2, e);
            Voice3Changed += (s, e) => UpdateSignalGeneratorFromVoice(_sgVoice3, e);
        }

        private void NAudioSid_FilterChanged(object sender, SidRegister e)
        {
            _sgVoice1Filtered.SetFrequency(FilterFrequency);
            _sgVoice1Filtered.LowPassEnabled = !BypassFilters && Voice1.Filtered && FilterLowPassEnabled;
            _sgVoice1Filtered.BandPassEnabled = !BypassFilters && Voice1.Filtered && FilterBandPassEnabled;
            _sgVoice1Filtered.HighPassEnabled = !BypassFilters && Voice1.Filtered && FilterHighPassEnabled;

            _sgVoice2Filtered.SetFrequency(FilterFrequency);
            _sgVoice2Filtered.LowPassEnabled = !BypassFilters && Voice2.Filtered && FilterLowPassEnabled;
            _sgVoice2Filtered.BandPassEnabled = !BypassFilters && Voice2.Filtered && FilterBandPassEnabled;
            _sgVoice2Filtered.HighPassEnabled = !BypassFilters && Voice2.Filtered && FilterHighPassEnabled;

            _sgVoice3Filtered.SetFrequency(FilterFrequency);
            _sgVoice3Filtered.LowPassEnabled = !BypassFilters && Voice3.Filtered && FilterLowPassEnabled;
            _sgVoice3Filtered.BandPassEnabled = !BypassFilters && Voice3.Filtered && FilterBandPassEnabled;
            _sgVoice3Filtered.HighPassEnabled = !BypassFilters && Voice3.Filtered && FilterHighPassEnabled;
        }

        private void Init()
        {
            _sgVoice1 = new SidSignalGenerator();
            _sgVoice2 = new SidSignalGenerator();
            _sgVoice3 = new SidSignalGenerator();

            _sgVoice1Filtered = new SidFilter(_sgVoice1);
            _sgVoice2Filtered = new SidFilter(_sgVoice2);
            _sgVoice3Filtered = new SidFilter(_sgVoice3);

            _mixingSampleProvider = new MixingSampleProvider(
                new List<ISampleProvider>() {
                    _sgVoice1Filtered,
                    _sgVoice2Filtered,
                    _sgVoice3Filtered
                }
            );

            _meteringSampleProvider = new MeteringSampleProvider(
                _mixingSampleProvider, _mixingSampleProvider.WaveFormat.SampleRate / 20);
            _meteringSampleProvider.StreamVolume += (sender, e) =>
            {
                VolumeMeter = e.MaxSampleValues.FirstOrDefault();
                Task.Run(() => VolumeMeterUpdate?.Invoke(this, VolumeMeter));
            };

            _volumeSampleProvider = new VolumeSampleProvider(_meteringSampleProvider);
            _volumeSampleProvider.Volume = 0.05f;

            switch (_wavePlayerType)
            {
                case WavePlayerType.WasapiOut:
                    _audioOutEvent = new WasapiOut(AudioClientShareMode.Shared, DESIRED_LATENCY);
                    break;
                case WavePlayerType.WaveOutEvent:
                    _audioOutEvent = new WaveOutEvent()
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
            get => _volumeSampleProvider?.Volume ?? 0;
            set
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

        private void UpdateSignalGeneratorFromVoice(SidSignalGenerator sg, Voice v)
        {
            // Set gain to 0 if voice is disabled or gate is low
            if (!v.Gate || v.Disabled)
            {
                if (sg.Gain != 0) sg.Gain = 0;
            }
            else
            {
                if (sg.Gain != SidVolume) sg.Gain = SidVolume;
            }

            // Update frequency
            if (sg.Frequency != v.Frequency) sg.Frequency = v.Frequency;

            // Update pulse width
            if (sg.PulseWidth != v.PulseWidth) sg.PulseWidth = v.PulseWidth;

            // Update waveform
            switch (v.WaveForm)
            {
                case VoiceWaveForm.Triangle:
                    if (sg.Type != SidSignalGeneratorType.Triangle) sg.Type = SidSignalGeneratorType.Triangle;
                    break;
                case VoiceWaveForm.SawTooth:
                    if (sg.Type != SidSignalGeneratorType.SawTooth) sg.Type = SidSignalGeneratorType.SawTooth;
                    break;
                case VoiceWaveForm.Square:
                    if (sg.Type != SidSignalGeneratorType.SquarePulseWidth) sg.Type = SidSignalGeneratorType.SquarePulseWidth;
                    break;
                case VoiceWaveForm.Noise:
                    if (sg.Type != SidSignalGeneratorType.White) sg.Type = SidSignalGeneratorType.White;
                    break;
                default:
                    sg.Gain = 0f;
                    break;
            }


        }
    }
}
