public class TimingData
{
	public float beat;
}

public class BpmData : TimingData
{
	public float bpm;

	public BpmData(float beat, float bpm)
	{
		this.beat = beat;
		this.bpm = bpm;
	}
}

public class SpeedData : TimingData
{
	public float speed;

	public SpeedData(float beat, float speed)
	{
		this.beat = beat;
		this.speed = speed;
	}
}

