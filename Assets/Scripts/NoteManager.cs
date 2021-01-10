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
            if (g.transform.position.z < -1.5f)
            {
                createdNotes.Remove(g);
                GameObject.Destroy(g);
                judgement(-1.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, -0.6f) && g.transform.position.z < 1.5f)
                {
                    float pos = g.transform.position.z;
                    createdNotes.Remove(g);
                    GameObject.Destroy(g);
                    judgement(pos);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, -0.2f) && g.transform.position.z < 1.5f)
                {
                    float pos = g.transform.position.z;
                    createdNotes.Remove(g);
                    GameObject.Destroy(g);
                    judgement(pos);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, 0.2f) && g.transform.position.z < 1.5f)
                {
                    float pos = g.transform.position.z;
                    createdNotes.Remove(g);
                    GameObject.Destroy(g);
                    judgement(pos);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject g in createdNotes.ToList())
            {
                if (AlmostEqual(g.transform.position.x, 0.6f) && g.transform.position.z < 1.5f)
                {
                    float pos = g.transform.position.z;
                    createdNotes.Remove(g);
                    GameObject.Destroy(g);
                    judgement(pos);
                    break;
                }
            }
        }
    }

    public void judgement(float pos)
    {
        pos = Mathf.Abs(pos);
        if(pos < 0.3f)
        {
            hp += 0.5f;
            ++combo;
            score+=2;
        }
        else if(pos < 0.7f)
        {
            hp += 0.3f;
            ++combo;
            score += 1;
        }
        else if(pos < 1.2f)
        {
            hp += 0.1f;
            ++combo;
        }
        else
        {
            hp -= 5;
            combo = 0;
        }
        if (hp > 100.0f) hp = 100.0f;
        if (hp < 0.0f) hp = 0.0f;
        ++processedNotes;

        float accuracy = (float)score / (float)processedNotes * 50.0f;

        scoreDisplay.GetComponent<Text>().text = accuracy.ToString("F2") + "%";
        hpDisplay.GetComponent<Text>().text = (int)hp + "/100";
        comboDisplay.GetComponent<Text>().text = combo + "x";
    }

    private IEnumerator createNote()
    {
        int prevLane = -1;
        float delay = 0.2f;

        while(true)
        {
            int   randomLane    = 0;
            float randomX       = 0.0f;
            float randomZ       = 0.0f;

            do
            {
                randomLane = Random.Range(0, 4);
            } while (prevLane == randomLane);
            randomX = -0.6f + (0.4f * randomLane);

            GameObject note = (Random.Range(0.0f, 1.0f) < 0.3) ? longNote : normalNote;

            GameObject g = GameObject.Instantiate(note, new Vector3(randomX, -0.999f, 7.3f),
                Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));

            createdNotes.Add(g);

            prevLane = randomLane;
            if (delay > 0.08f) delay -= 0.0002f;
            yield return new WaitForSeconds(delay);
        }
    }

    private bool AlmostEqual(float a, float b)
	{
        return Mathf.Abs(a - b) < 0.0001;
	}
}
