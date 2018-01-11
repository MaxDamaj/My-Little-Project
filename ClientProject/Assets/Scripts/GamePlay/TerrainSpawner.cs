using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MLA.Gameplay.Controllers;

[Serializable]
public struct EnduranceSection {
    public string title;
    public float startDistance;
    public GameObject[] terrains;
    public GameObject border;
    public List<GameObject> obstacles;
    public GameObject[] bonuses;
    public Material skybox;
    public float sunIntencity;
    public int spawnDelay;
}

namespace MLA.Gameplay.Scenes {
    public class TerrainSpawner : MonoBehaviour {

        public List<EnduranceSection> sections;
        [Header("Common")]
        public float shift = 20f;
        public GameObject startSection;

        private Light sunlight;
        private Skybox sky_camera;
        private Collider pony;
        private int eoh_counter;
        private GameObject lastTerrain = null;
        private Transform anchor;

        #region API

        public void Init(Collider coll) {
            pony = coll;
            eoh_counter = 3;
            anchor = transform.GetChild(0);
            sky_camera = Camera.main.GetComponent<Skybox>();
            sunlight = GameObject.Find("SunLight").GetComponent<Light>();
            Destroy(Instantiate(startSection, transform.position, startSection.transform.rotation), 45);
            Destroy(Instantiate(startSection, new Vector3(transform.position.x - 20f, 0f, 0f), startSection.transform.rotation), 45);
            Destroy(Instantiate(startSection, anchor.position, startSection.transform.rotation), 45);

            sky_camera.material = sections[0].skybox;
        }

        #endregion

        #region Events

        void OnTriggerEnter(Collider coll) {
            if (coll == pony) { SpawnNewSection(); }
        }

        #endregion

        void SpawnNewSection() {
            //Move Trigger
            transform.position = new Vector3(transform.position.x + shift, 0, 0);
            //Find Section
            EnduranceSection section = sections.FindLast(x => x.startDistance <= anchor.position.x);

            //Spawn Terrain
            int sel = 0;
            if (section.terrains.GetLength(0) > 1) {
                sel = UnityEngine.Random.Range(1, section.terrains.GetLength(0));
                if (lastTerrain != section.terrains[0]) {
                    sel = 0;
                }
            }
            Destroy(Instantiate(section.terrains[sel], anchor.position, section.terrains[sel].transform.rotation), 45);
            lastTerrain = section.terrains[sel];
            //Spawn Bonuses
            sel = UnityEngine.Random.Range(0, section.obstacles.Count);
            Destroy(Instantiate(section.obstacles[sel], new Vector3(anchor.position.x - 7.5f, anchor.position.y, anchor.position.z), section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(0, section.obstacles.Count);
            Destroy(Instantiate(section.obstacles[sel], new Vector3(anchor.position.x - 2.5f, anchor.position.y, anchor.position.z), section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(0, section.obstacles.Count);
            Destroy(Instantiate(section.obstacles[sel], new Vector3(anchor.position.x + 2.5f, anchor.position.y, anchor.position.z), section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(0, section.obstacles.Count);
            Destroy(Instantiate(section.obstacles[sel], new Vector3(anchor.position.x + 7.5f, anchor.position.y, anchor.position.z), section.obstacles[sel].transform.rotation), 45);
            //Spawn Border
            Destroy(Instantiate(section.border, new Vector3(anchor.position.x + shift, -0.6f, 20f), section.border.transform.rotation), 45);
            //Skybox change
            sunlight.intensity = section.sunIntencity;
            sky_camera.material = section.skybox;
            //Spawn Packs
            if (eoh_counter <= 0 && section.bonuses.GetLength(0) != 0) {
                sel = UnityEngine.Random.Range(0, section.bonuses.GetLength(0));
                Destroy(Instantiate(section.bonuses[sel], new Vector3(anchor.position.x - 10f, 0.55f, 0f), section.bonuses[sel].transform.rotation), 30);
                eoh_counter = section.spawnDelay;
            }
            eoh_counter--;
        }
    }
}
