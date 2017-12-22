using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MLA.UI.Windows {
    public class UIEndReward : MonoBehaviour {

        [Header("UI")]
        public Text distance;
        public GameObject taken;
        public Image[] rewIcons;
        public Text[] rewTexts;
        public Text rewardContainer;

        [Header("Common")]
        public string EndDifficulty;

        void Start() {

        }
    }
}
