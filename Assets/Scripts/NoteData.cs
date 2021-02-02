public class NoteData
{
	public float beat;
	public float startSecond;	// 노트가 생성되는 시간.
	public float second;		// 노트가 판정선에 도달하는 시간.
	public int key;

	public NoteData(float beat, int key)
	{
		this.beat = beat;
		this.key = key;
		this.second = -1.0f;
		this.startSecond = -1.0f;
	}
}
