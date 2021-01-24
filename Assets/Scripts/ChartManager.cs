﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class ChartManager : MonoBehaviour
{
	private string version, title, artist;
	private float bpm, offset;
	private List<TimingData> timings = new List<TimingData>();
	private List<NoteData> notes = new List<NoteData>();

	// Start is called before the first frame update
	void Start()
	{
		string songData = ((TextAsset)Resources.Load("Charts/SampleSongData")).text;
		RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;

		version = Regex.Match(songData, @"(?<=^#version\s*)(\d+[\d\.]*\d+|\d)(?=\s*$)", options).Value;
		title = Regex.Match(songData, @"(?<=^title\s*:\s*)(\S.*\S|\S)(?=\s*$)", options).Value;
		artist = Regex.Match(songData, @"(?<=^artist\s*:\s*)(\S.*\S|\S)(?=\s*$)", options).Value;
		bpm = float.Parse(Regex.Match(songData, @"(?<=^bpm\s*:\s*)(\d+\.?\d+|\d)(?=\s*$)", options).Value);
		offset = float.Parse(Regex.Match(songData, @"(?<=^offset\s*:\s*)(\d+\.?\d+|\d)(?=\s*$)", options).Value);
		string timingData = songData.Split(new string[] { "#timing" }, StringSplitOptions.None)[1]
			.Split(new string[] { "#notedata" }, StringSplitOptions.None)[0].Trim();
		string noteData = songData.Split(new string[] { "#notedata" }, StringSplitOptions.None)[1].Trim() ;

		Debug.Log("version: " + version);
		Debug.Log("title: " + title);
		Debug.Log("artist: " + artist);
		Debug.Log("bpm: " + bpm);
		Debug.Log("offset: " + offset);
		Debug.Log(timingData);
		Debug.Log(noteData);

		foreach (string str in timingData.Split('\n'))
		{
			if (str.Contains("bpm"))
			{
				Match match = Regex.Match(str, @"(?<=^bpm)\s*:\s*(\d+\.?\d+|\d)\s*{\s*beat\s*:\s*(\d+\.?\d+|\d)\s*}(?=\s*$)", options);
				float bpm = float.Parse(match.Groups[1].Value);
				float beat = float.Parse(match.Groups[2].Value);
				timings.Add(new BpmData(beat, bpm));
			}
			else if (str.Contains("speed"))
			{
				Match match = Regex.Match(str, @"(?<=^speed)\s*:\s*(\d+\.?\d+|\d)\s*{\s*beat\s*:\s*(\d+\.?\d+|\d)\s*}(?=\s*$)", options);
				float speed = float.Parse(match.Groups[1].Value);
				float beat = float.Parse(match.Groups[2].Value);
				timings.Add(new BpmData(beat, speed));
			}
			else
			{
				Debug.Log("TimingData Load Error");
			}
		}

		foreach (string str in noteData.Split('\n'))
		{
			Match match = Regex.Match(str, @"(?<=^normalnote)\s*{\s*beat\s*:\s*(\d+\.?\d+|\d)\s*,\s*key\s*:\s*(\d+)\s*}(?=\s*$)", options);
			float beat = float.Parse(match.Groups[1].Value);
			int key = int.Parse(match.Groups[2].Value);
			notes.Add(new NoteData(beat, key));
		}

		NoteManager noteManager = GameObject.Find("Note Manager").GetComponent<NoteManager>();
		noteManager.SetVersion(version);
		noteManager.SetTitle(title);
		noteManager.SetArtist(artist);
		noteManager.SetBpm(bpm);
		noteManager.SetOffset(offset);
		noteManager.SetTimings(timings);
		noteManager.SetNotes(notes);
	}

	// Update is called once per frame
	void Update()
	{

	}
}