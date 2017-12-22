using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using MLA.System;
using MLA.System.Controllers;

public enum UIAction {
    nothing = 0,
    buying = 1,
    clear = 2,
    exit = 3,
    startChallenge = 4,
    restart = 5,
    returnToEquestria = 6,
    startEndurance = 7,
    hideLeftPanels = 8,
    loadCodex = 9,
    runSimulation = 10,
    startRaceCource = 11
};

namespace MLA.UI.Windows {
    public class UIMessageWindow : MonoBehaviour {

        [SerializeField]
        DatabaseManager DBM = null;
        [SerializeField]
        DBChallenges DBC = null;

        public GameObject window;
        public Button ButtonYes;
        public Button ButtonNo;
        public Text questionText;

        private int ID;
        private UIAction action;

        private static UIMessageWindow component;

        #region API

        public static UIMessageWindow Instance {
            get {
                if (component == null) {
                    component = FindObjectOfType<UIMessageWindow>();
                }
                return component;
            }
        }

        void Start() {
            ButtonYes.onClick.AddListener(Confirm);
            ButtonNo.onClick.AddListener(Decline);
        }

        #endregion

        public void ShowMessage(string text, int id, UIAction mAction) {
            ID = id;
            action = mAction;
            ButtonYes.gameObject.SetActive(true);
            ButtonNo.gameObject.SetActive(true);
            questionText.text = text;
            window.SetActive(true);
        }
        public void ShowMessage(string text, int id, UIAction mAction, bool bYes, bool bNo) {
            ID = id;
            action = mAction;
            ButtonYes.gameObject.SetActive(bYes);
            ButtonNo.gameObject.SetActive(bNo);
            questionText.text = text;
            window.SetActive(true);
        }

        private void Confirm() {
            switch (action) {
                case UIAction.buying:
                    Database.Instance.SetCharFMRank(ID, 0);
                    CharsFMData pony = Database.Instance.GetCharFMInfo(ID);
                    for (int i = 0; i < pony.costPrises.GetLength(0); i++) {
                        Database.Instance.IncreaseItemQuantity(pony.costItems[i], 0 - pony.costPrises[i]);
                    }
                    AchievementsController.Instance.CheckStates();
                    break;
                case UIAction.clear:
                    DBM.ClearState();
                    UnityEngine.SceneManagement.SceneManager.LoadScene("splashScreen");
                    break;
                case UIAction.exit:
                    Application.Quit();
                    break;
                case UIAction.startChallenge:
                    var challenge = DBC.GetChallenge(ID);
                    foreach (var item in challenge.startFee) {
                        Database.Instance.IncreaseItemQuantity(item.ItemName, -item.ItemQuantity);
                    }
                    GlobalData.Instance.nowChallenge = ID;
                    UnityEngine.SceneManagement.SceneManager.LoadScene("freemode");
                    break;
                case UIAction.returnToEquestria:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
                    break;
                case UIAction.runSimulation:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("simulation");
                    break;
                case UIAction.startRaceCource:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("freemode");
                    break;
            }
            window.SetActive(false);
        }

        private void Decline() {
            window.SetActive(false);
        }
    }
}
