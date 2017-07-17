using UnityEngine;
using System.Collections;

public class TerrainCHL : MonoBehaviour {

    [SerializeField]
    DBChallenges DBC = null;

    private Challenge challenge;
    private Collider pony;
    private int eoh_counter;

    void Start() {
        Invoke("FindPony", 0.4f);
        challenge = DBC.GetChallenge(GlobalData.Instance.nowChallenge);
        //Create base terrain
        Destroy(Instantiate(challenge.roads[0], new Vector3(0, 0, 0), challenge.roads[0].transform.rotation), 45);
        Destroy(Instantiate(challenge.border, new Vector3(0, -0.6f, 20f), challenge.border.transform.rotation), 45);
        Destroy(Instantiate(challenge.border, new Vector3(-20f, -0.6f, 20f), challenge.border.transform.rotation), 45);
        Destroy(Instantiate(challenge.border, new Vector3(20f, -0.6f, 20f), challenge.border.transform.rotation), 45);
        eoh_counter = challenge.packsSpawnDelay;
    }

    void OnTriggerEnter(Collider coll) {
        if (coll == pony) {
            SpawnNewSection();
        }
    }

    void FindPony() {
        pony = PonyController.Instance.GetComponent<Collider>();
    }


    void SpawnNewSection() {
        //Move Trigger
        transform.position = new Vector3(transform.position.x + 20, 0, 0);

        //Spawn Terrain
        int sel = Random.Range(0, challenge.roads.GetLength(0));
        Destroy(Instantiate(challenge.roads[sel], transform.position, challenge.roads[sel].transform.rotation), 45);
        //Spawn Bonuses
        sel = Random.Range(0, challenge.bonuses.GetLength(0));
        Destroy(Instantiate(challenge.bonuses[sel], transform.position, challenge.bonuses[sel].transform.rotation), 45);
        //Spawn Border
        Destroy(Instantiate(challenge.border, new Vector3(transform.position.x + 20, -0.6f, 20f), challenge.border.transform.rotation), 45);
        //Spawn Packs
        if (eoh_counter <= 0) {
            sel = Random.Range(0, challenge.packs.GetLength(0));
            Destroy(Instantiate(challenge.packs[sel], new Vector3(transform.position.x - 10f, 0.55f, 0f), challenge.packs[sel].transform.rotation), 30);
            eoh_counter = challenge.packsSpawnDelay;
        }
        eoh_counter--;
    }
}
