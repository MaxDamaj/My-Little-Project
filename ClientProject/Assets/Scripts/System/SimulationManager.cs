using UnityEngine;
using System.Collections;
using MLA.UI.Controllers;

namespace MLA.System.Controllers {
    public class SimulationManager : MonoBehaviour {

        [Header("Common")]
        public GameObject mainCamera;

        void Start() {
            GlobalData.Instance.IsSimulation = true;
            MusicManager.Instance.SetFolder("Music/Simulation", 0);
            MenuNavigation.Instance.Init();
            Refresh();
        }

        public void Refresh() {
            if (Database.Instance == null) return;
            if (mainCamera != null) mainCamera.GetComponent<Bloom>().enabled = Database.Instance.paramBloom == 1;
            if (mainCamera != null) mainCamera.GetComponent<AmbientObscurance>().enabled = Database.Instance.paramSSAO == 1;
        }
    }
}
