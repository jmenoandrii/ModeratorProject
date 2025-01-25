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
    [SerializeField] private GameObject _playButton;
    [SerializeField] private int index;
    [SerializeField] private List<Song> _songList = new List<Song>();
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        ValidateIndex();
        ConfigureSong();
    }

    public void Next()
    {
        index++;
        ValidateIndex();
        ConfigureSong();
    }

    public void Previous()
    {
        index--;
        ValidateIndex();
        ConfigureSong();
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
        if (!_playButton.activeSelf)
            _audioSource.Play();
    }

    [System.Serializable]
    public class Song
    {
        public string name;
        public AudioClip clip;
    }
}
