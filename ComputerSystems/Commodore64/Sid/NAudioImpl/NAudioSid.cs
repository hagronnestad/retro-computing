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
        public event EventHandler<float[]> MeteringUpdate;

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
        public float VolumeMeter { get; private set; }

        private SidAdsrSampleProvider _adsrVoice1;
        private SidAdsrSampleProvider _adsrVoice2;
        private SidAdsrSampleProvider _adsrVoice3;

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

            _adsrVoice1 = new SidAdsrSampleProvider(_sgVoice1);
            _adsrVoice2 = new SidAdsrSampleProvider(_sgVoice2);
            _adsrVoice3 = new SidAdsrSampleProvider(_sgVoice3);

            _sgVoice1Filtered = new SidFilter(_adsrVoice1);
            _sgVoice2Filtered = new SidFilter(_adsrVoice2);
            _sgVoice3Filtered = new SidFilter(_adsrVoice3);


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
                //MeteringUpdate?.Invoke(this, e.MaxSampleValues);
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

                case SidRegister.VOICE1_CONTROL_REGISTER:
                    _adsrVoice1.Gate(Voice1.Gate);
                    break;

                case SidRegister.VOICE2_CONTROL_REGISTER:
                    _adsrVoice2.Gate(Voice2.Gate);
                    break;

                case SidRegister.VOICE3_CONTROL_REGISTER:
                    _adsrVoice3.Gate(Voice3.Gate);
                    break;

                case SidRegister.VOICE1_SUSTAIN_RELEASE:
                    _adsrVoice1.AttackSeconds = Voice1.AttackSeconds;
                    _adsrVoice1.DecaySeconds = Voice1.DecaySeconds;
                    _adsrVoice1.SustainLevel = Voice1.SustainLevel;
                    _adsrVoice1.ReleaseSeconds = Voice1.ReleaseSeconds;
                    break;

                case SidRegister.VOICE2_SUSTAIN_RELEASE:
                    _adsrVoice2.AttackSeconds = Voice2.AttackSeconds;
                    _adsrVoice2.DecaySeconds = Voice2.DecaySeconds;
                    _adsrVoice2.SustainLevel = Voice2.SustainLevel;
                    _adsrVoice2.ReleaseSeconds = Voice2.ReleaseSeconds;
                    break;

                case SidRegister.VOICE3_SUSTAIN_RELEASE:
                    _adsrVoice3.AttackSeconds = Voice3.AttackSeconds;
                    _adsrVoice3.DecaySeconds = Voice3.DecaySeconds;
                    _adsrVoice3.SustainLevel = Voice3.SustainLevel;
                    _adsrVoice3.ReleaseSeconds = Voice3.ReleaseSeconds;
                    break;
            }
        }

        private void UpdateSignalGeneratorFromVoice(SidSignalGenerator sg, Voice v)
        {
            ////Set gain to 0 if voice is disabled or gate is low
            //if (!v.Gate)
            //{
            //    //sg.Gain = 0f; // if (sg.Gain != 0) sg.Gain = 0;
            //}
            //else 
            if (v.Disabled)
            {
                sg.Gain = 0f; // if (sg.Gain != 0) sg.Gain = 0;
            }
            else
            {
                sg.Gain = SidVolume; // if (sg.Gain != SidVolume) sg.Gain = SidVolume;
            }

            // Update frequency
            sg.Frequency = v.Frequency;

            // Update pulse width
            sg.PulseWidth = v.PulseWidth;

            sg.WaveformSquareActive = v.WaveformSquareActive;
            sg.WaveformTriangleActive = v.WaveformTriangleActive;
            sg.WaveformSawToothActive = v.WaveformSawToothActive;
            sg.WaveformNoiseActive = v.WaveformNoiseActive;

        }
    }
}
