using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData
{
	public float beat;
	public int key;

	public NoteData(float beat, int key)
	{
		this.beat = beat;
		this.key = key;
	}
}
