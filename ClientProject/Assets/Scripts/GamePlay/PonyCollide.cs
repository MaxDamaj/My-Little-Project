using UnityEngine;
using System.Collections;

public class PonyCollide : MonoBehaviour {

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Player") {
            switch (gameObject.tag) {
                //Obstacles
                case "Hay":
                case "WoodCrate":
                case "Rail":
                case "Firebarrel":
                    GetComponent<Rigidbody>().AddForce(new Vector3(100f, 90f, -50f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                //Pickups
                case "Bit":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 120f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Laughter":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Generosity":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Honesty":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Kindness":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Loyalty":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
                case "EoH_Magic":
                    GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f));
                    Destroy(GetComponent<Collider>());
                    Destroy(gameObject, 1f);
                    break;
            }

        }
    }
}
