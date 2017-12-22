using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLA.Gameplay.Common {
    public class Projectile : MonoBehaviour {

        public Vector3 force;

        public GameObject fx = null;
        public float _objectDestroyDelay = 0;
        public float _fxDestroyDelay = 3;


        void Start() {
            GetComponent<Rigidbody>().AddRelativeForce(force);
        }



        public void DestroyObject(float delay) {
            IEnumerator spawnFX = SpawnFX(delay);
            StartCoroutine(spawnFX);
        }

        IEnumerator SpawnFX(float delay) {
            yield return new WaitForSeconds(delay);
            if (fx != null) {
                Destroy(Instantiate(fx, transform.position, fx.transform.rotation), _fxDestroyDelay);
            }
            Destroy(gameObject, _objectDestroyDelay);
        }

    }
}
