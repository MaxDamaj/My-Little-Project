using MLA.System;
using MLA.System.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MLA.UI.Windows {
    public class UIEndurance : MonoBehaviour {

        public Button endEasyButton;
        public Button endNormalButton;
        public Button endHardButton;
        public Text endEasyText;
        public Text endNormalText;
        public Text endHardText;

        public Color textDisableColor;

        public void Init() {
            endEasyButton.interactable = false;
            endEasyText.color = textDisableColor;
            endNormalButton.interactable = false;
            endNormalText.color = textDisableColor;
            endHardButton.interactable = false;
            endHardText.color = textDisableColor;

            if (Database.Instance.enduranceLevel >= 1) {
                endEasyButton.interactable = true;
                endEasyText.color = Color.white;
                endEasyButton.onClick.AddListener(delegate { GameController.Instance.StartEndurance(Difficulty.Easy); });
            }
            if (Database.Instance.enduranceLevel >= 2) {
                endNormalButton.interactable = true;
                endNormalText.color = Color.white;
                endNormalButton.onClick.AddListener(delegate { GameController.Instance.StartEndurance(Difficulty.Normal); });
            }
            if (Database.Instance.enduranceLevel >= 3) {
                endHardButton.interactable = true;
                endHardText.color = Color.white;
                //endHardButton.onClick.AddListener(delegate { GameController.Instance.StartEndurance(Difficulty.Hard); });
            }

        }

    }
}
