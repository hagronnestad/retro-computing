<Query Kind="Program" />

void Main()
{
	var frequency = 500;

	Enumerable.Range(0, frequency).Chart(x => x)
		.AddYSeries(x => GetSine(x, frequency), LINQPad.Util.SeriesType.Line, "Sine")
		.AddYSeries(x => GetSquare(x, frequency), LINQPad.Util.SeriesType.Line, "Square")
		.AddYSeries(x => GetSquarePulseWidth(x, frequency, 0.75f), LINQPad.Util.SeriesType.Line, "Square PW")
		.AddYSeries(x => GetTriangle(x, frequency), LINQPad.Util.SeriesType.Line, "Triangle")
		.AddYSeries(x => GetSawToth(x, frequency), LINQPad.Util.SeriesType.Line, "Saw")
		.Dump();
}

private const double TwoPi = 2 * Math.PI;
private const int SampleRate = 44100;
private const double Gain = 1;

double GetSquarePulseWidth(int nSample, double frequency, float pulseWidth)
{
	var multiple = 2 * frequency / SampleRate;
	var sampleSaw = nSample * multiple % 2;
	var sampleValue = sampleSaw >= pulseWidth * 2 ? -Gain : Gain;

	nSample++;
	sampleSaw.Dump();
	return sampleValue;
}

double GetSine(int nSample, double frequency)
{
	var multiple = TwoPi * frequency / SampleRate;
	var sampleValue = Gain * Math.Sin(nSample * multiple);
	nSample++;
	return sampleValue;
}

double GetSquare(int nSample, double frequency)
{
	var multiple = 2 * frequency / SampleRate;
	var sampleSaw = nSample * multiple % 2 - 1;
	var sampleValue = sampleSaw >= 0 ? Gain : -Gain;
	nSample++;
	return sampleValue;
}

double GetTriangle(int nSample, double frequency)
{
	var multiple = 2 * frequency / SampleRate;
	var sampleSaw = nSample * multiple % 2;
	var sampleValue = 2 * sampleSaw;

	if (sampleValue > 1)
		sampleValue = 2 - sampleValue;
	if (sampleValue < -1)
		sampleValue = -2 - sampleValue;

	sampleValue *= Gain;
	nSample++;
	return sampleValue;
}

double GetSawToth(int nSample, double frequency)
{
	var multiple = 2 * frequency / SampleRate;
	var sampleSaw = nSample * multiple % 2 - 1;
	var sampleValue = Gain * sampleSaw;
	nSample++;
	return sampleValue;
}