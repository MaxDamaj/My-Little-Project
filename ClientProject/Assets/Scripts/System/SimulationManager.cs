using UnityEngine;
using System.Collections;

public class SimulationManager : MonoBehaviour {

    [Header("Common")]
    public GameObject mainCamera;

    void Start() {
        MusicManager.Instance.SetFolder("Music/Simulation", 0);
        Refresh();
    }

    public void Refresh() {
        if (Database.Instance == null) return;
        if (mainCamera != null) mainCamera.GetComponent<Bloom>().enabled = Database.Instance.paramBloom == 1;
        if (mainCamera != null) mainCamera.GetComponent<AmbientObscurance>().enabled = Database.Instance.paramSSAO == 1;
    }

}
