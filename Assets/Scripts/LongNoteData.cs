using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteData : NoteData
{
	float endBeat, endSecond;

	public LongNoteData(float beat, float endBeat, int key) : base(beat, key)
	{
		this.endBeat = endBeat;
	}
}
