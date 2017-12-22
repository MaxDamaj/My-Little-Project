using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MLA.System.Controllers {
    public class SoundManager : MonoBehaviour {

        private List<AudioSource> sounds = null;
        private static SoundManager manager;

        public static SoundManager Instance {
            get {
                if (manager == null) {
                    manager = FindObjectOfType<SoundManager>();
                }
                return manager;
            }
        }

        public void UpdateSoundList() {
            sounds = new List<AudioSource>();
            for (int i = 0; i < transform.childCount; i++) {
                sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
            }
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
}
