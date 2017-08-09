using UnityEngine;
using System.Collections;

public class MusicControl : MonoBehaviour {

    public AudioSource[] music; //Music list
    public int id;              //id in Database for checking next song

    private int now_playing;    //id in list current playing song

    void Start() {
        PlayNextSong(id);
    }

    void FixedUpdate() {
        //Play next song if nothing are played
        if (!music[now_playing].isPlaying) {
            PlayNextSong(id);
        }
    }

    //Play next song in list
    void PlayNextSong(int listID) {
        now_playing = Database.Instance.nowPlaying[listID];
        Database.Instance.nowPlaying[listID]++;
        if (Database.Instance.nowPlaying[listID] >= music.GetLength(0)) {
            Database.Instance.nowPlaying[listID] = 0;
        }
        music[now_playing].Play();
    }
}
