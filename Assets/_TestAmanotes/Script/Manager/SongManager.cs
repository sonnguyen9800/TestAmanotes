using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using TestAmanotes;
using UnityCommunity.UnitySingleton;

public class SongManager : MonoSingleton<SongManager>
{
    public AudioSource audioSource;
   // public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;
    

    public string fileLocation;
    public float noteTime = 1;
    // public float noteSpawnY;
    // public float noteTapY;
    // public float noteDespawnY
    // {
    //     get
    //     {
    //         noteTapY = NoteSpawnerManager.Instance.TapLineTransform.position.y;
    //         return  noteTapY - (noteSpawnY - noteTapY);
    //     }
    // }

    public static MidiFile midiFile;
    // Start is called before the first frame update

    public void Setup()
    {
        ReadFromFile();
    }
    

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

       // foreach (var lane in lanes) lane.SetTimeStamps(array);
       NoteSpawnerManager.Instance.SetTimeStamps(array);
        
    }

    public void PlayGame()
    {
        Invoke(nameof(StartSong), songDelayInSeconds);

    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

}
