using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteController : MonoBehaviour
{
	private NoteData note;
	private NoteManager noteManager;

	// Update is called once per frame
	void Update()
	{
		transform.Translate(0, noteManager.GetSpeed() * -10.0f * Time.deltaTime, 0);
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
