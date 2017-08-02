using UnityEngine;
using System.Collections;

public class PonyCollide : MonoBehaviour {

    [Header("Main")]
    public Vector3 hitForce = Vector3.zero;
    public string collisionType = "Hay";
    public string hitSound;

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Player") {
            GetComponent<Rigidbody>().AddForce(new Vector3(100f, 90f, -50f));
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, 1f);
            SoundManager.Instance.PlaySound(hitSound);
        }
    }
}
