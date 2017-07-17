using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public struct EnduranceSection {
    public string title;
    public float startDistance;
    public GameObject[] terrains;
    public GameObject border;
    public GameObject[] obstacles;
    public GameObject[] bonuses;
    public Material skybox;
    public float sunIntencity;
    public int spawnDelay;
}

public class TerrainEDR : MonoBehaviour {

    public List<EnduranceSection> sections;

    public Light sunlight;
    public Skybox sky_camera;

    private Collider pony;
    private int eoh_counter;
    private GameObject lastTerrain = null;

    // Use this for initialization
    void Start() {
        Invoke("FindPony", 0.4f);
        eoh_counter = 3;

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
        //Find Section
        EnduranceSection section =  sections.FindLast(x => x.startDistance <= transform.position.x);

        //Spawn Terrain
        int sel = 0;
        if (section.terrains.GetLength(0) > 1) {
            sel = UnityEngine.Random.Range(1, section.terrains.GetLength(0));
            if (lastTerrain != section.terrains[0]) {
                sel = 0;
            }
        }    
        Destroy(Instantiate(section.terrains[sel], transform.position, section.terrains[sel].transform.rotation), 45);
        lastTerrain = section.terrains[sel];
        //Spawn Bonuses
        sel = UnityEngine.Random.Range(0, section.obstacles.GetLength(0));
        Destroy(Instantiate(section.obstacles[sel], transform.position, section.obstacles[sel].transform.rotation), 45);
        //Spawn Border
        Destroy(Instantiate(section.border, new Vector3(transform.position.x + 20, -0.6f, 20f), section.border.transform.rotation), 45);
        //Skybox change
        sunlight.intensity = section.sunIntencity;
        sky_camera.material = section.skybox;
        //Spawn Packs
        if (eoh_counter <= 0) {
            sel = UnityEngine.Random.Range(0, section.bonuses.GetLength(0));
            Destroy(Instantiate(section.bonuses[sel], new Vector3(transform.position.x - 10f, 0.55f, 0f), section.bonuses[sel].transform.rotation), 30);
            eoh_counter = section.spawnDelay;
        }
        eoh_counter--;
    }
}
