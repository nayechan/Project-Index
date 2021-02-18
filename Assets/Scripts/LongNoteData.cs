using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteData : NoteData
{
	public float endBeat, endSecond, length;
	public LongNoteData(float beat, float endBeat, int key) : base(beat, key)
	{
		this.endBeat = endBeat;
	}
}
