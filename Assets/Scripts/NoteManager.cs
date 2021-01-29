using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    //필요한 프리팹
    [SerializeField] private GameObject     scoreDisplay, hpDisplay, comboDisplay;
    [SerializeField] private NoteController noteController;
    [SerializeField] private GameObject     normalNote, longNote;

    private List<GameObject>    createdNotes;
    private List<NoteData>      noteData;
    private List<TimingData>    bpmData;
    private List<TimingData>    speedData;

    private float hp = 100;
    private float bpm, offset, speed = 1.0f;
    private float startTime;

    private int score = 0;
    private int processedNotes = 0;
    private int combo = 0;
    private int ncount = 0;

    private bool start = false;

    private string version, title, artist;

    // Start is called before the first frame update
    private void Start()
    {
        createdNotes    = new List<GameObject>();
        Debug.Log("Press Space to Start");
        // StartCoroutine("createNote");

    }

    // Update is called once per frame
    private void Update()
    {
        if (!start && Input.GetKeyDown(KeyCode.Space))
		{
            start = true;

            bpmData[0].second = 0;
            speedData[0].second = 0;

            // 시간을 알려면 비트를 시간으로 바꾸어야 하니까 bpmSecs를 먼저 구한다.
            // 기본값을 주었으니 인덱스가 1부터 시작.
            for (int i = 1; i < bpmData.Count; ++i)
			{
                // 지금 bpm이 바뀌는 시간은, 직전에 bpm이 바뀐 시간 + 그때와의 beat 차이 * 60 / 직전에 바꾼 bpm
                bpmData[i].second = bpmData[i - 1].second + (bpmData[i].beat - bpmData[i - 1].beat) * 60 / bpmData[i - 1].value;
            }
            
            for (int i = 1; i < speedData.Count; ++i)
			{
                // bpm과 speed가 동시에 수정되는 상황은 고려하지 않음!
                speedData[i].second = BeatToSec(speedData[i].beat);
			}

            foreach (TimingData p in bpmData)
			{
                Debug.Log("bpm: " + p.second + " " + p.value);
			}

            foreach (TimingData p in speedData)
            {
                Debug.Log("speed: " + p.second + " " + p.value);
            }

            Debug.Log("Game Start!");

            startTime = Time.time;
		}

        if (start)
		{
            float interval = 60 / bpm;                          // beat당 시간 간격(초)
            float beat = (Time.time - startTime) / interval;    // 현재 beat

            // note가 beat 오름차순으로 정렬되었다고 가정.
            foreach (NoteData note in noteData.ToList())
			{
                if (note.beat < beat + 1.0f)
                {
                    
                    GameObject g = Instantiate(normalNote, new Vector3(-1.0f + 0.4f * note.key, -0.999f, 7.3f),
                    Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                    g.GetComponent<NoteController>().SetNoteManager(this);
                    g.GetComponent<NoteController>().SetNoteData(note);

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
        string debugLog = "";
        foreach(NoteData noteData in noteData)
        {
            debugLog += noteData.beat.ToString();
            debugLog += ' ';
            debugLog += noteData.key.ToString();
            debugLog += '\n';
        }
        Debug.Log(debugLog);
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

	float BeatToSec(float beat)
	{
        int left = 0, right = bpmData.Count - 1, mid = (left + right) / 2;
        while (left <= right)
		{
            mid = (left + right) / 2;
            if (bpmData[mid].beat == beat)
			{
                break;
			}
            if (bpmData[mid].beat < beat)
			{
                left = mid + 1;
			}
			else
			{
                right = mid - 1;
			}
        }

        // 이진 탐색 성공
        // 찾는 beat 수의 second 정보가 bpmData에 저장되어 있음.
        if (left <= right)
		{
            return bpmData[mid].second;
		}
        // 이진 탐색 실패
        // right가 직전에 변경된 bpm 인덱스를 들고 있음! 그때의 시간과 beat 차이, bpm을 이용해서 시간을 계산.
        else
        {
            return bpmData[right].second + (beat - bpmData[right].beat) * 60 / bpmData[right].value;
		}
	}

    private bool AlmostEqual(float a, float b)
	{
        return Mathf.Abs(a - b) < 0.0001;
	}
}
