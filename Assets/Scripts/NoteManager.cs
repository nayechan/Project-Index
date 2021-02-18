using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
	//필요한 프리팹
	[SerializeField] private GameObject		scoreDisplay, hpDisplay, comboDisplay;
	// [SerializeField] private NoteController	noteController;
	[SerializeField] private GameObject		normalNote, longNote;

	private List<NoteData>		noteData;
	private List<TimingData>	bpmData;
	private List<TimingData>	speedData;

	// private float hp = 100;
	private float bpm, offset, speed = 1.0f;
	public float startTime;	// 임시로 public

	// private int score = 0;
	// private int processedNotes = 0;
	// private int combo = 0;
	// private int ncount = 0;

	private bool start = false;

	private string version, title, artist;

	private float mapZ = 8.0f;

	// Start is called before the first frame update
	private void Start()
	{
		Debug.Log("Press Space to Start");
	}

	// Update is called once per frame
	private void Update()
	{
		if (!start && Input.GetKeyDown(KeyCode.Space))
		{
			start = true;

			bpmData[0].second = 0;
			for (int i = 1; i < bpmData.Count; ++i)
			{
				// 지금 bpm이 바뀌는 시간은, 직전에 bpm이 바뀐 시간 + 그때와의 beat 차이 * 60 / 직전에 바꾼 bpm
				bpmData[i].second = bpmData[i - 1].second + (bpmData[i].beat - bpmData[i - 1].beat) * 60 / bpmData[i - 1].value;
			}

			speedData[0].second = BeatToSec(speedData[0].beat);
			for (int i = 1; i < speedData.Count; ++i)
			{
				speedData[i].second = BeatToSec(speedData[i].beat);
			}

			foreach (NoteData note in noteData)
			{
				note.second = BeatToSec(note.beat);
				note.startSecond = GetStartSecond(note.second);

				if (note.GetType().Equals(typeof(LongNoteData)))
				{
					LongNoteData n = note as LongNoteData;
					n.endSecond = BeatToSec(n.endBeat);
					n.length = GetNoteLength(n);
				}
			}

			string bpmLog = "bpmLog\n", speedLog = "speedLog\n", noteLog = "noteLog\n";

			foreach (TimingData d in bpmData)
			{
				bpmLog += "bpm: " + d.second + " " + d.value + "\n";
			}

			foreach (TimingData d in speedData)
			{
				speedLog += "speed: " + d.second + " " + d.value + "\n";
			}

			foreach (NoteData note in noteData)
			{
				noteLog += "note: " + note.startSecond +" " + note.second + " " + note.key + "\n";
			}

			Debug.Log(bpmLog);
			Debug.Log(speedLog);
			Debug.Log(noteLog);

			Debug.Log("Game Start!");

			// 게임은 2초부터 시작한다. 노트가 음수 시간에 생성되는 경우에는 그 시간만큼 더 먼저 시작한다.
			startTime = Time.time + 2.0f;
			if (noteData[0].startSecond < 0)
			{
				startTime -= noteData[0].startSecond;
			}
		}

		if (start)
		{
			float currentTime = Time.time - startTime;

			foreach (TimingData d in speedData)
			{
				if (currentTime >= d.second)
				{
					speed = d.value;
				}
			}

			foreach (NoteData note in noteData.ToList())
			{
				if (currentTime >= note.startSecond)
				{
					if (note.GetType().Equals(typeof(NormalNoteData)))
					{
						GameObject g = Instantiate(normalNote, new Vector3(-1.0f + 0.4f * note.key, -0.999f, mapZ - (currentTime - note.startSecond) * speed * 10.0f),
Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
						g.GetComponent<NoteController>().SetNoteManager(this);
						g.GetComponent<NoteController>().SetNoteData(note);
					}
					else if (note.GetType().Equals(typeof(LongNoteData)))
					{
						LongNoteData n = note as LongNoteData;
						GameObject g = Instantiate(longNote, new Vector3(-1.0f + 0.4f * note.key, -0.999f, mapZ - (currentTime - note.startSecond) * speed * 10.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
						g.transform.localScale += new Vector3(0, GetNoteLength(n), 0);
						if (g != null)
						{
							g.GetComponent<NoteController>().SetNoteData(note);
							g.GetComponent<NoteController>().SetNoteManager(this);
						}
					}

					noteData.Remove(note);
				}
				else
				{
					break;
				}
			}

		}
	}

	public void SortNoteData()
	{
		string debugLog = "sort note\n";
		foreach(NoteData noteData in noteData)
		{
			debugLog += noteData.beat.ToString();
			debugLog += ' ';
			debugLog += noteData.key.ToString();
			debugLog += '\n';
		}
		// Debug.Log(debugLog);
	}

	public List<NoteData> GetNoteListInstance()
	{
		if (noteData == null)
			noteData = new List<NoteData>();
		return noteData;
	}

	public void SetVersion(string version)
	{
		this.version = version;
	}

	public void SetTitle(string title)
	{
		this.title = title;
	}

	public void SetArtist(string artist)
	{
		this.artist = artist;
	}

	public void SetBpm(float bpm)
	{
		this.bpm = bpm;
	}

	public void SetOffset(float offset)
	{
		this.offset = offset;
	}
	
	public void SetBpmData(List<TimingData> timings)
	{
		this.bpmData = timings;
	}

	public void SetSpeedData(List<TimingData> timings)
	{
		this.speedData = timings;
	}

	public void AddNote(NoteData noteData)
	{
		GetNoteListInstance().Add(noteData);
	}

	public float GetSpeed()
	{
		return speed;
	}

	public float BeatToSec(float beat)
	{
		int left = 0, right = bpmData.Count - 1, mid = (left + right) / 2;
		while (left <= right)
		{
			mid = (left + right) / 2;

			// 이진 탐색 성공
			// 찾는 beat 수의 second 정보가 bpmData에 저장되어 있음.
			if (IsAlmostEqual(beat, bpmData[mid].beat))
			{
				return bpmData[mid].second;
			}
			if (beat > bpmData[mid].beat)
			{
				left = mid + 1;
			}
			else
			{
				right = mid - 1;
			}
		}

		// 첫 bpmData(0)보다 빠른 beat가 들어올 때 예외 처리.
		if (right == -1)
		{
			right = 0;
		}
		return bpmData[right].second + (beat - bpmData[right].beat) * 60 / bpmData[right].value;
	}

	// dir은 양이나 음의 정수로 방향을 표시함.
	private int GetLastSpeed(float second, int dir)
	{
		int left = 0, right = speedData.Count - 1, mid = (left + right) / 2;
		while (left <= right)
		{
			mid = (left + right) / 2;

			if (IsAlmostEqual(second, speedData[mid].second))
			{
				// dir이 음수면 mid - 1, dir이 양수면 mid
				if (dir < 0)
				{
					--mid;
				}
				return mid;
			}
			if (second > speedData[mid].second)
			{
				left = mid + 1;
			}
			else
			{
				right = mid - 1;
			}
		}

		// speedData[0].value를 float.minValue로 초기화해서 에러 처리 필요 없음.
		return right;
	}

	private float GetNoteLength(LongNoteData longNote)
	{
		float startSecond = longNote.startSecond, endSecond = GetStartSecond(longNote.endSecond);
		int last = GetLastSpeed(startSecond, 1);
		float distanceSum = 0;
		int cnt = 0;
		while (true)
		{
			// 다음 변속 전에 노트가 끝나면
			if (endSecond <= speedData[last + 1].second)
			{
				distanceSum += (endSecond - startSecond) * speedData[last].value * 10.0f;
				return distanceSum;
			}

			distanceSum += (speedData[last + 1].second - startSecond) * speedData[last].value * 10.0f;
			startSecond = speedData[last + 1].second;
			++last;
			if (++cnt > 100)
			{
				Debug.Log("INFINITE LOOP");
				return 0;
			}
		}
	}

	private float GetStartSecond(float second)
	{
		int last = GetLastSpeed(second, -1);
		float distanceSum = 0, timeSum = 0;
		while (true)
		{
			// 변속이 바뀌기 전까지 움직이는 거리
			float nextDistance = (second - speedData[last].second) * speedData[last].value * 10.0f;

			// 이 속도 구간에서 맵 끝에 도달할 수 있으면
			if (distanceSum + nextDistance >= mapZ)
			{
				// 남은 거리 / 속도만큼 시간 추가하고 루프 종료.
				timeSum += (mapZ - distanceSum) / (speedData[last].value * 10.0f);
				return second - timeSum;
			}

			// 이 변속 구간에서 맵 끝에 도달하지 못하면
			distanceSum += nextDistance;
			timeSum += second - speedData[last].second ;
			--last;
		}
	}

	private bool IsAlmostEqual(float a, float b)
	{
		return Mathf.Abs(a - b) < 0.0001;
	}
}
