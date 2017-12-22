using UnityEngine;
using System.Collections;
using MLA.System;
using MLA.System.Controllers;

namespace MLA.Gameplay.Common {
    public class PonyCollide : MonoBehaviour {

        [Header("Main")]
        public Vector3 hitForce = Vector3.zero;
        public string collisionType = "Hay";
        public string hitSound;

        private Rigidbody _rigidbody;

        void Start() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision coll) {
            if (coll.gameObject.tag == "Player") {

                if (GlobalData.Instance.gameState == GameModeState.Endurance) {
                    GetComponent<Rigidbody>().AddForce(hitForce);
                } else {
                    GetComponent<Rigidbody>().AddExplosionForce(100f * coll.rigidbody.velocity.sqrMagnitude * _rigidbody.mass, coll.transform.position, 2f, 1f);
                }

                Destroy(GetComponent<Collider>());
                Destroy(gameObject, 1f);
                SoundManager.Instance.PlaySound(hitSound);
            }
        }
    }
}
