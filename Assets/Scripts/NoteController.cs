using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    private float speed = 1.0f;
    List<GameObject> notes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject note in notes.ToList())
		{
            if (note.transform.position.z < -1)
			{
                notes.Remove(note);
                GameObject.Destroy(note);
			}
            note.transform.Translate(0, speed * -10.0f * Time.deltaTime, 0);
        }
    }

    public void SetSpeed(float speed)
	{
        this.speed = speed;
	}

    public void AddNote(GameObject note)
	{
        notes.Add(note);
	}
}
