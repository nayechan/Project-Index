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
    private List<NoteData>      notes;
    private List<TimingData>    bpms;
    private List<TimingData>    speeds;

    // note, bpm, speed 정보를 비트 수가 아니라 초 단위로 변환하여 저장.
    // key가 sec, value가 노트 번호, 바뀌는 bpm과 speed 값.
    private List<KeyValuePair<float, int>> noteSecs;
    private List<KeyValuePair<float, float>> bpmSecs;
    private List<KeyValuePair<float, float>> speedSecs;

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

            // beat, speed가 바뀌거나 note가 놓이는 시간이 key
            bpmSecs     = new List<KeyValuePair<float, float>>();
            speedSecs   = new List<KeyValuePair<float, float>>();
            noteSecs    = new List<KeyValuePair<float, int>>();

            // chart manager에게서 받은 #info의 bpm, speed를 0초에 기본값으로 추가.
            bpmSecs.Add(new KeyValuePair<float, float>(0, bpm));
            speedSecs.Add(new KeyValuePair<float, float>(0, speed));


            // 시간을 알려면 비트를 시간으로 바꾸어야 하니까 bpmSecs를 먼저 구한다.
            // 기본값을 주었으니 인덱스가 1부터 시작.
            for (int i = 1; i < bpms.Count; ++i)
			{
                // 지금 bpm이 바뀌는 시간은, 직전에 bpm이 바뀐 시간 + 그때와의 beat 차이 * 60 / 직전에 바꾼 bpm
                bpmSecs.Add(new KeyValuePair<float, float>
                    (bpmSecs[i - 1].Key + (bpms[i].beat - bpms[i - 1].beat) * 60 / bpmSecs[i - 1].Value, bpms[i].value));
            }
            
            for (int i = 1; i < speeds.Count; ++i)
			{
                // bpm과 speed가 동시에 수정되는 상황은 고려하지 않음!
                speedSecs.Add(new KeyValuePair<float, float>
                    (BeatToSecs(speeds[i].beat), speeds[i].value));
			}
            
            Debug.Log("Game Start!");

            startTime = Time.time;
		}

        if (start)
		{
            float interval = 60 / bpm;                          // beat당 시간 간격(초)
            float beat = (Time.time - startTime) / interval;    // 현재 beat

            // note가 beat 오름차순으로 정렬되었다고 가정.
            foreach (NoteData note in notes.ToList())
			{
                if (note.beat < beat + 1.0f)
                {
                    
                    GameObject g = Instantiate(normalNote, new Vector3(-1.0f + 0.4f * note.key, -0.999f, 7.3f),
                    Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                    g.GetComponent<NoteController>().SetNoteManager(this);
                    g.GetComponent<NoteController>().SetNoteData(note);

                    notes.Remove(note);
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
        foreach(NoteData noteData in notes)
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
        if (notes == null)
            notes = new List<NoteData>();
        return notes;
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
    
    public void SetBpms(List<TimingData> timings)
	{
        this.bpms = timings;
	}

    public void SetSpeeds(List<TimingData> timings)
	{
        this.speeds = timings;
	}

    public void AddNote(NoteData noteData)
    {
        GetNoteListInstance().Add(noteData);
    }

    public float GetSpeed()
    {
        return speed;
    }

	float BeatToSecs(float beat)
	{
        int left = 0, right = bpms.Count - 1, mid = (left + right) / 2;
        while (left <= right)
		{
            mid = (left + right) / 2;
            if (bpms[mid].beat == beat)
			{
                break;
			}
            if (bpms[mid].beat < beat)
			{
                left = mid + 1;
			}
			else
			{
                right = mid - 1;
			}
        }

        // 이진 탐색 성공
        if (bpms[mid].beat == beat)
		{
            return bpmSecs[mid].Key;
		}
        // 이진 탐색 실패(right가 직전에 변경된 bpm 인덱스를 들고 있음)
		else
		{
            return bpmSecs[right].Key + (beat - bpms[right].beat) * 60 / bpmSecs[right].Value;
		}
	}

    private bool AlmostEqual(float a, float b)
	{
        return Mathf.Abs(a - b) < 0.0001;
	}
}
