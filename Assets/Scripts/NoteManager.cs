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
    private List<TimingData>    timings;

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
            
            Debug.Log("Game Start!");

            startTime = Time.time;
		}

        if (start)
		{
            float interval = 60 / bpm;
            float beat = (Time.time - startTime) / interval;

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

            foreach (TimingData timing in timings.ToList())
            {
                if (timing.beat < beat)
				{
                    
                    // Bpmdata / SpeedData
                    if (timing.IsBpmData()) // typeof(timing) == BpmData
					{
                        bpm = timing.value;
                        timings.Remove(timing);
                        Debug.Log("bpm: " + bpm);
					}
                    else if (timing.IsSpeedData()) // typeof(timing) == SpeedData
					{
                        speed = timing.value;
                        timings.Remove(timing);
                        Debug.Log("speed: " + speed);
                    }
					else
					{
                        Debug.Log("Timing Data Type Error");
					}
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
    
    public void SetTimings(List<TimingData> timings)
	{
        this.timings = timings;
	}

    public void AddNote(NoteData noteData)
    {
        GetNoteListInstance().Add(noteData);
    }

    public float GetSpeed()
    {
        return speed;
    }

    private bool AlmostEqual(float a, float b)
	{
        return Mathf.Abs(a - b) < 0.0001;
	}
}
