using UnityEngine;
using System.Collections;

public class SimulationManager : MonoBehaviour {

    public DBSimulation database;

    void Start() {
        /*if (FindObjectOfType<DBSimulation>() == null) {
            GameObject tmp = Instantiate(database.gameObject);
            tmp.name = "Database_Simulation";
        }*/
    }

}
