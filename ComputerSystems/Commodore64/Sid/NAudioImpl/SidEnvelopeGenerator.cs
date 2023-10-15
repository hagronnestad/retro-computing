using System;

namespace Commodore64.Sid.NAudioImpl
{
    public class SidEnvelopeGenerator
    {
        public enum EnvelopeState
        {
            Idle,
            Attack,
            Decay,
            Sustain,
            Release
        }

        private EnvelopeState state;

        private float output;

        private float attackRate;

        private float decayRate;

        private float releaseRate;

        private float attackCoef;

        private float decayCoef;

        private float releaseCoef;

        private float sustainLevel;

        private float targetRatioAttack;

        private float targetRatioDecayRelease;

        private float attackBase;

        private float decayBase;

        private float releaseBase;

        public float AttackRate
        {
            get
            {
                return attackRate;
            }
            set
            {
                attackRate = value;
                attackCoef = CalcCoef(value, targetRatioAttack);
                attackBase = (1f + targetRatioAttack) * (1f - attackCoef);
            }
        }

        public float DecayRate
        {
            get
            {
                return decayRate;
            }
            set
            {
                decayRate = value;
                decayCoef = CalcCoef(value, targetRatioDecayRelease);
                decayBase = (sustainLevel - targetRatioDecayRelease) * (1f - decayCoef);
            }
        }

        public float ReleaseRate
        {
            get
            {
                return releaseRate;
            }
            set
            {
                releaseRate = value;
                releaseCoef = CalcCoef(value, targetRatioDecayRelease);
                releaseBase = (0f - targetRatioDecayRelease) * (1f - releaseCoef);
            }
        }

        public float SustainLevel
        {
            get
            {
                return sustainLevel;
            }
            set
            {
                sustainLevel = value;
                decayBase = (sustainLevel - targetRatioDecayRelease) * (1f - decayCoef);
            }
        }

        public EnvelopeState State => state;

        public SidEnvelopeGenerator()
        {
            Reset();
            AttackRate = 0f;
            DecayRate = 0f;
            ReleaseRate = 0f;
            SustainLevel = 1f;
            SetTargetRatioAttack(0.3f);
            SetTargetRatioDecayRelease(0.0001f);
        }

        private static float CalcCoef(float rate, float targetRatio)
        {
            return (float)Math.Exp((0.0 - Math.Log((1f + targetRatio) / targetRatio)) / (double)rate);
        }

        private void SetTargetRatioAttack(float targetRatio)
        {
            if (targetRatio < 1E-09f)
            {
                targetRatio = 1E-09f;
            }

            targetRatioAttack = targetRatio;
            attackBase = (1f + targetRatioAttack) * (1f - attackCoef);
        }

        private void SetTargetRatioDecayRelease(float targetRatio)
        {
            if (targetRatio < 1E-09f)
            {
                targetRatio = 1E-09f;
            }

            targetRatioDecayRelease = targetRatio;
            decayBase = (sustainLevel - targetRatioDecayRelease) * (1f - decayCoef);
            releaseBase = (0f - targetRatioDecayRelease) * (1f - releaseCoef);
        }

        public float Process()
        {
            switch (state)
            {
                case EnvelopeState.Attack:
                    output = attackBase + output * attackCoef;
                    if (output >= 1f)
                    {
                        output = 1f;
                        state = EnvelopeState.Decay;
                    }

                    break;
                case EnvelopeState.Decay:
                    output = decayBase + output * decayCoef;
                    if (output <= sustainLevel)
                    {
                        output = sustainLevel;
                        state = EnvelopeState.Sustain;
                    }

                    break;
                case EnvelopeState.Release:
                    output = releaseBase + output * releaseCoef;
                    if ((double)output <= 0.0)
                    {
                        output = 0f;
                        state = EnvelopeState.Idle;
                    }

                    break;
            }

            return output;
        }

        public void Gate(bool gate)
        {
            if (gate)
            {
                state = EnvelopeState.Attack;
            }
            else if (state != 0)
            {
                state = EnvelopeState.Release;
            }
        }

        public void Reset()
        {
            state = EnvelopeState.Idle;
            output = 0f;
        }

        public float GetOutput()
        {
            return output;
        }
    }
}
