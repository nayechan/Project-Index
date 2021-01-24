using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class ChartManager : MonoBehaviour
{
	private string version, title, artist;
	private int bpm, offset;
	public List<NoteData> notes;

	// Start is called before the first frame update
	void Start()
	{
		string songData = ((TextAsset)Resources.Load("Charts/SampleSongData")).text;
		RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;

		version = Regex.Match(songData, @"(?<=^#version\s*)(\d+[\d\.]*\d+|\d)", options).Value;
		title = Regex.Match(songData, @"(?<=^title\s*:\s*)(\S.*\S|\S)", options).Value;
		artist = Regex.Match(songData, @"(?<=^artist\s*:\s*)(\S.*\S|\S)", options).Value;
		bpm = int.Parse(Regex.Match(songData, @"(?<=^bpm\s*:\s*)\d+", options).Value);
		offset = int.Parse(Regex.Match(songData, @"(?<=^offset\s*:\s*)\d+", options).Value);
		string noteData = songData.Split(new string[] { "#notedata" }, StringSplitOptions.None)[1].Trim() ;

		Debug.Log("version: " + version);
		Debug.Log("title: " + title);
		Debug.Log("artist: " + artist);
		Debug.Log("bpm: " + bpm);
		Debug.Log("offset: " + offset);


		notes = new List<NoteData>();
		foreach (string str in noteData.Split('\n'))
		{
			foreach (Match match in Regex.Matches(str, @"(?<=^normalnote)\s*{\s*beat\s*:\s*(\d+\.?\d+)\s*,\s*key\s*:\s*(\d+)\s*}(?=\s*$)", options))
			{
				float beat = float.Parse(match.Groups[1].Value);
				int key = int.Parse(match.Groups[2].Value);
				notes.Add(new NoteData(beat, key));
				Debug.Log("beat: " + beat + ", key: " + key);
			}
		}

		NoteManager noteManager = GameObject.Find("NoteManager").GetComponent<NoteManager>();
		noteManager.SetVersion(version);
		noteManager.SetTitle(title);
		noteManager.SetArtist(artist);
		noteManager.SetBpm(bpm);
		noteManager.SetOffset(offset);
		noteManager.SetNotes(notes);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
