using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    List<AudioSource> sounds = null;

    // Use this for initialization
    void Start() {

    }

    public void PlaySound(string name) {
        AudioSource sound = sounds.Find(x => x.gameObject.name == name);
        if (sound != null) {
            sound.Play();
        }
    }

    public void SetMuteState(string name, bool value) {
        AudioSource sound = sounds.Find(x => x.gameObject.name == name);
        if (sound != null) {
            sound.mute = value;
        }
    }
}
