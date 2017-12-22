using MLA.Gameplay.Controllers;
using MLA.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLA.Gameplay.Scenes {
    public class TerrainSIM : MonoBehaviour {

        private Collider pony;
        private int eoh_counter;
        private GameObject lastTerrain = null;

        #region API

        void Start() {
            Invoke("FindPony", 0.4f);
            eoh_counter = 3;

        }

        void FindPony() {
            pony = PonyController.Instance.GetComponent<Collider>();
        }

        #endregion

        #region Events

        void OnTriggerEnter(Collider coll) {
            if (coll == pony) { SpawnNewSection(); }
        }

        #endregion

        void SpawnNewSection() {
            //Move Trigger
            transform.position = new Vector3(transform.position.x + 20, 0, 0);

            //Spawn Terrain
            int sel = 0;
            if (DBSimulation.Instance.section.terrains.GetLength(0) > 1) {
                sel = UnityEngine.Random.Range(1, DBSimulation.Instance.section.terrains.GetLength(0));
                if (lastTerrain != DBSimulation.Instance.section.terrains[0]) {
                    sel = 0;
                }
            }
            Destroy(Instantiate(DBSimulation.Instance.section.terrains[sel], transform.position, DBSimulation.Instance.section.terrains[sel].transform.rotation), 45);
            lastTerrain = DBSimulation.Instance.section.terrains[sel];
            //Spawn Bonuses
            sel = UnityEngine.Random.Range(DBSimulation.Instance.sectionBonusLow, DBSimulation.Instance.sectionBonusHigh);
            Destroy(Instantiate(DBSimulation.Instance.section.obstacles[sel], new Vector3(transform.position.x - 7.5f, transform.position.y, transform.position.z), DBSimulation.Instance.section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(DBSimulation.Instance.sectionBonusLow, DBSimulation.Instance.sectionBonusHigh);
            Destroy(Instantiate(DBSimulation.Instance.section.obstacles[sel], new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z), DBSimulation.Instance.section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(DBSimulation.Instance.sectionBonusLow, DBSimulation.Instance.sectionBonusHigh);
            Destroy(Instantiate(DBSimulation.Instance.section.obstacles[sel], new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z), DBSimulation.Instance.section.obstacles[sel].transform.rotation), 45);
            sel = UnityEngine.Random.Range(DBSimulation.Instance.sectionBonusLow, DBSimulation.Instance.sectionBonusHigh);
            Destroy(Instantiate(DBSimulation.Instance.section.obstacles[sel], new Vector3(transform.position.x + 7.5f, transform.position.y, transform.position.z), DBSimulation.Instance.section.obstacles[sel].transform.rotation), 45);
            //Spawn Border
            Destroy(Instantiate(DBSimulation.Instance.section.border, new Vector3(transform.position.x + 20, -0.6f, 20f), DBSimulation.Instance.section.border.transform.rotation), 45);
            //Spawn Packs
            if (eoh_counter <= 0 && DBSimulation.Instance.section.bonuses.GetLength(0) != 0) {
                sel = UnityEngine.Random.Range(0, DBSimulation.Instance.section.bonuses.GetLength(0));
                Destroy(Instantiate(DBSimulation.Instance.section.bonuses[sel], new Vector3(transform.position.x - 10f, 0.55f, 0f), DBSimulation.Instance.section.bonuses[sel].transform.rotation), 30);
                eoh_counter = DBSimulation.Instance.section.spawnDelay;
            }
            eoh_counter--;
        }
    }
}