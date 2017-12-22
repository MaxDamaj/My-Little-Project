using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.System;

namespace MLA.UI.Windows {
    public class UISimRoadInfo : MonoBehaviour {

        [Header("UI")]
        public Text bonusesText;
        public Button beginButton;

        void Start() {
            Refresh();
            beginButton.onClick.AddListener(delegate {
                UnityEngine.SceneManagement.SceneManager.LoadScene("road_simulation");
            });
        }

        void Refresh() {
            bonusesText.text = DBSimulation.Instance.sectionBonusLow + "-" + DBSimulation.Instance.sectionBonusHigh;
        }

        public void IncreaseDiffLow(int value) {
            if (DBSimulation.Instance.sectionBonusLow + value >= 0 && DBSimulation.Instance.sectionBonusLow + value < DBSimulation.Instance.sectionBonusHigh) {
                DBSimulation.Instance.sectionBonusLow += value;
            }
            Refresh();
        }
        public void IncreaseDiffHigh(int value) {
            if (DBSimulation.Instance.sectionBonusHigh + value > DBSimulation.Instance.sectionBonusLow && DBSimulation.Instance.sectionBonusHigh + value < DBSimulation.Instance.section.obstacles.Count) {
                DBSimulation.Instance.sectionBonusHigh += value;
            }
            Refresh();
        }
        public void ResetDifficulty() {
            DBSimulation.Instance.sectionBonusLow = 0;
            DBSimulation.Instance.sectionBonusHigh = 5;
            Refresh();
        }
    }
}
