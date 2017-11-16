using UnityEngine;
using System.Collections;

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

            //GetComponent<Rigidbody>().AddForce(new Vector3(hitForce));
            GetComponent<Rigidbody>().AddExplosionForce(100f*coll.rigidbody.velocity.sqrMagnitude*_rigidbody.mass, coll.transform.position, 2f, 1f);

            Destroy(GetComponent<Collider>());
            Destroy(gameObject, 1f);
            SoundManager.Instance.PlaySound(hitSound);
        }
    }
}
