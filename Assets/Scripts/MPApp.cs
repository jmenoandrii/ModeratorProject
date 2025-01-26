using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MPApp : App
{
    [Header("Song Settings")]
    [SerializeField] private TMP_Text _songName;
    [SerializeField] private TMP_Text _songTime;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _stopButton;
    [SerializeField] private int index;
    [SerializeField] private List<Song> _songList = new List<Song>();
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        ValidateIndex();
        ConfigureSong();
    }

    private void Update()
    {
        UpdateSongTime();
    }

    public void Next()
    {
        index++;
        ValidateIndex();
        ConfigureSong();
        PlaySong();
    }

    public void Previous()
    {
        index--;
        ValidateIndex();
        ConfigureSong();
        PlaySong();
    }

    private void ValidateIndex()
    {
        if (index < 0)
            index = _songList.Count - 1;
        else if (index >= _songList.Count)
            index = 0;
    }

    private void ConfigureSong()
    {
        _audioSource.clip = _songList[index].clip;
        _songName.SetText(_songList[index].name);
    }

    private void PlaySong()
    {
        _playButton.SetActive(false);
        _stopButton.SetActive(true);
        _audioSource.Play();
    }

    private void UpdateSongTime()
    {
        if (_audioSource.clip != null)
        {
            float currentTime = _audioSource.time;
            float totalTime = _audioSource.clip.length;

            string formattedTime = FormatTime(currentTime) + " / " + FormatTime(totalTime);
            _songTime.SetText(formattedTime);
        }
    }

    private string FormatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    [System.Serializable]
    public class Song
    {
        public string name;
        public AudioClip clip;
    }
}
