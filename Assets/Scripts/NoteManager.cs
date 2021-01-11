using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    //필요한 프리팹
    [SerializeField]
    private GameObject normalNote, longNote, scoreDisplay, hpDisplay, comboDisplay;

    private List<GameObject> createdNotes;
    private float hp = 100;
    private int score = 0;
    private int processedNotes = 0;
    private int combo = 0;

    private int ncount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        createdNotes = new List<GameObject>();
        StartCoroutine("createNote");
    }

    // Update is called once per frame
    private void Update()
    {

        foreach(GameObject g in createdNotes.ToList())
        {
            if (g.transform.position.z < -2.0f)
            {
                createdNotes.Remove(g);
                GameObject.Destroy(g);
                judgement(-2.0f);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject targetNote = null;
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, -0.6f) && g.transform.position.z < 2.0f)
                {
                    float pos = g.transform.position.z;
                    if (targetNote == null || targetNote.transform.position.z > pos)
                        targetNote = g;
                }
            }
            if (targetNote != null)
            {
                createdNotes.Remove(targetNote);
                GameObject.Destroy(targetNote);
                judgement(targetNote.transform.position.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject targetNote = null;
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, -0.2f) && g.transform.position.z < 2.0f)
                {
                    float pos = g.transform.position.z;
                    if (targetNote == null || targetNote.transform.position.z > pos)
                        targetNote = g;
                }
            }
            if (targetNote != null)
            {
                createdNotes.Remove(targetNote);
                GameObject.Destroy(targetNote);
                judgement(targetNote.transform.position.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject targetNote = null;
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, 0.2f) && g.transform.position.z < 2.0f)
                {
                    float pos = g.transform.position.z;
                    if (targetNote == null || targetNote.transform.position.z > pos)
                        targetNote = g;
                }
            }
            if (targetNote != null)
            {
                createdNotes.Remove(targetNote);
                GameObject.Destroy(targetNote);
                judgement(targetNote.transform.position.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject targetNote = null;
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, 0.6f) && g.transform.position.z < 2.0f)
                {
                    float pos = g.transform.position.z;
                    if (targetNote == null || targetNote.transform.position.z > pos)
                        targetNote = g;
                }
            }
            if (targetNote != null)
            {
                createdNotes.Remove(targetNote);
                GameObject.Destroy(targetNote);
                judgement(targetNote.transform.position.z);
            }
        }
    }

    public void judgement(float pos)
    {
        string judgeString = "";
        pos = Mathf.Abs(pos);
        if(pos < 0.4f)
        {
            hp += 0.3f;
            ++combo;
            score += 3;
            judgeString = "Perfect";
        }
        else if(pos < 0.8f)
        {
            hp += 0.2f;
            ++combo;
            score += 2;
            judgeString = "Great";
        }
        else if(pos < 1.6f)
        {
            hp += 0.1f;
            ++combo;
            score += 1;
            judgeString = "Good";
        }
        else
        {
            hp -= 5;
            combo = 0;
            judgeString = "Miss";
        }
        if (hp > 100.0f) hp = 100.0f;
        if (hp < 0.0f) hp = 0.0f;
        ++processedNotes;

        float accuracy = (float)score / (float)processedNotes * 100.0f / 3.0f;

        scoreDisplay.GetComponent<Text>().text = accuracy.ToString("F2") + "%";
        hpDisplay.GetComponent<Image>().fillAmount = hp / 100.0f;
        if (judgeString != "Miss")
            comboDisplay.GetComponent<Text>().text = judgeString + " " + combo;
        else comboDisplay.GetComponent<Text>().text = judgeString;
    }

    private IEnumerator createNote()
    {
        int[] prevLane = { -1, -1, -1 };
        int prevCount = 0;
        float delay = 0.16f;
        
        while (true)
        {
            int count = 0;
            float randomX = 0.0f;
            float randomZ = 0.0f;

            ++ncount;

            int[] randomLane = new int[3];

            count = ncount % 4;

            if (count == 3) count = 1;
            if (count == 0) count = 2;

            if (count > 4 - prevCount)
                count = 4 - prevCount;

            for (int i = 0; i < count; ++i)
            {
                bool flag;
                do
                {
                    flag = false;
                    randomLane[i] = Random.Range(0, 4);
                    for (int j = 0; j < i; ++j)
                    {
                        if (randomLane[j] == randomLane[i])
                            flag = true;
                    }
                    for (int j = 0; j < prevCount; ++j)
                    {
                        if (prevLane[j] == randomLane[i])
                            flag = true;
                    }
                } while (flag);

                randomX = -0.6f + (0.4f * randomLane[i]);

                GameObject note = (Random.Range(0.0f, 1.0f) < 0.3) ? longNote : normalNote;

                GameObject g = GameObject.Instantiate(normalNote, new Vector3(randomX, -0.999f, 7.3f),
                    Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));

                createdNotes.Add(g);
            }

            for (int i = 0; i < count; ++i)
            {
                prevLane[i] = randomLane[i];
            }

            prevCount = count;
            yield return new WaitForSeconds(delay);
        }
    }

    private bool AlmostEqual(float a, float b)
	{
        return Mathf.Abs(a - b) < 0.0001;
	}
}
