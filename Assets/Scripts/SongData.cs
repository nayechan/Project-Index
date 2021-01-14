using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SongData : MonoBehaviour
{
	public string version, title, artist;
	public int bpm, offset;
	private string songData;

	// Start is called before the first frame update
	void Start()
	{
		songData = ((TextAsset)Resources.Load("Charts/SampleSongData")).text;
		RegexOptions options = RegexOptions.Multiline;

		version = Regex.Match(songData, @"(?<=^#version\s*)(\d+[\d\.]*\d+|\d)", options).Value;
		title = Regex.Match(songData, @"(?<=^title\s*:\s*)(\S.*\S|\S)", options).Value;
		artist = Regex.Match(songData, @"(?<=^artist\s*:\s*)(\S.*\S|\S)", options).Value;
		bpm = int.Parse(Regex.Match(songData, @"(?<=^bpm\s*:\s*)\d+", options).Value);
		offset = int.Parse(Regex.Match(songData, @"(?<=^offset\s*:\s*)\d+", options).Value);

		Debug.Log("version: " + version);
		Debug.Log("title: " + title);
		Debug.Log("artist: " + artist);
		Debug.Log("bpm: " + bpm);
		Debug.Log("offset: " + offset);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
