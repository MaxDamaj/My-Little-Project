using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    private AudioSource music = null;
    private int listID = -1;
    private List<AudioClip> clips = new List<AudioClip>();

    private static MusicManager manager;

    #region API

    public static MusicManager Instance {
        get {
            if (manager == null) {
                manager = FindObjectOfType<MusicManager>();
            }
            return manager;
        }
    }

    void Start() {
        music = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        if (!music.isPlaying && listID >= 0) {
            PlayNextSong();
        }
    }

    #endregion


    public void SetFolder(string pass, int dbIndex) {
        music.Stop();
        listID = dbIndex;
        clips = new List<AudioClip>();
        clips.AddRange(Resources.LoadAll<AudioClip>(pass));
        PlayNextSong();
    }

    public void StopAllMusic() {
        music.Stop();
    }

    public void SetMusicVolume(float value) {
        music.volume = value;
    }

    void PlayNextSong() {
        Database.Instance.nowPlaying[listID]++;
        if (Database.Instance.nowPlaying[listID] >= clips.Count) {
            Database.Instance.nowPlaying[listID] = 0;
        }
        music.clip = clips[Database.Instance.nowPlaying[listID]];
        music.Play();
    }
}
