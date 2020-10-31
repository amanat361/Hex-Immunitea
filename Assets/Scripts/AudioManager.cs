using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] playlist;
    public int currentSongIndex = 0;

    void Start()
    {
        NextTrack();
    }

    private void OnDestroy()
    {
        source.Stop();
    }

    void NextTrack()
    {
        if (currentSongIndex == playlist.Length)
        {
            currentSongIndex = 0;
        }
        source.Stop();
        source.clip = playlist[currentSongIndex++];
        source.Play();
        //TO-DO add continuous playlist
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextTrack();
        }
    }
}