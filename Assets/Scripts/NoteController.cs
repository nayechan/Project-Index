using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteController : MonoBehaviour
{
	private NoteData note;
	private NoteManager noteManager;
	private float minDistance = float.MaxValue;
	private float minSecond = float.MaxValue;
	private bool b = true;

	// Update is called once per frame
	void Update()
	{
		transform.Translate(0, noteManager.GetSpeed() * -10.0f * Time.deltaTime, 0);

		// 검산
		float pos = transform.position.z;
		if (b && Mathf.Abs(pos) < Mathf.Abs(minDistance))
		{
			minDistance = pos;
			minSecond = (Time.time - noteManager.startTime) + minDistance / (noteManager.GetSpeed() * 10.0f);
		}
		if (b && pos < 0)
		{
			Debug.Log(minSecond);
			b = false;
		}
	}

	public void SetNoteData(NoteData note)
	{
		this.note = note;
	}

	public void SetNoteManager(NoteManager noteManager)
	{
		this.noteManager = noteManager;
	}
}
