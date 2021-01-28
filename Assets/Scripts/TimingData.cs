public class TimingData
{
	public enum TYPE {
		bpm = 0,
		speed = 1
	}

	public TYPE type;
	public float beat;
	public float value;

	public TimingData(TYPE type, float beat, float value)
	{
		this.type = type;
		this.beat = beat;
		this.value = value;
	}

	public bool IsBpmData()
	{
		return type == TYPE.bpm;
	}
	public bool IsSpeedData()
	{
		return type == TYPE.speed;
	}
}