using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MLA.Gameplay.Controllers;
using MLA.System;

namespace MLA.Gameplay.Common {
    public class CameraPonyFollow : MonoBehaviour {

        public Slider posCorrection;
        public float shake_intensity;
        [SerializeField]
        private float shake_decay = 0.001f;

        // Use this for initialization
        void Start() {
            posCorrection.value = Database.Instance.cameraShift;
            GetComponent<Bloom>().enabled = Database.Instance.paramBloom == 1;
            GetComponent<AmbientObscurance>().enabled = Database.Instance.paramSSAO == 1;
        }

        // Update is called once per frame
        void Update() {
            if (PonyController.Instance == null) { return; }
            //Camera follow to pony
            if (GlobalData.Instance.IsYMovementAllowed) {
                transform.position = new Vector3(PonyController.Instance.transform.position.x + posCorrection.value, PonyController.Instance.transform.position.y + 1.05f, -2.2f);
            } else {
                transform.position = new Vector3(PonyController.Instance.transform.position.x + posCorrection.value, 1.05f, -2.2f);
            }

            //Shaking
            if (shake_intensity > 0) {
                transform.position += Random.insideUnitSphere * shake_intensity;
                shake_intensity -= shake_decay;
            }
        }

        void OnDestroy() {
            if (Database.Instance == null) return;
            Database.Instance.cameraShift = posCorrection.value;
        }
    }
}
