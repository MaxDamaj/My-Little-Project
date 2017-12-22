using MLA.System;
using MLA.UI.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MLA.UI.Windows {
    public class UIChallengeFull : MonoBehaviour {

        [SerializeField]
        DBChallenges DBC = null;

        public Text titleText;
        public Image mapImage;
        public Text timeText;
        public Image[] feeIcons;
        public Text[] feeTexts;
        public Image[] rewardIcons;
        public Text[] rewardTexts;
        public Button beginButton;

        private Challenge challenge;
        private int challID;

        void Start() {
            beginButton.onClick.AddListener(StartChallenge);
        }

        public void ShowWindow(int ChallID) {
            MenuNavigation.Instance.HideLeftPanels();
            GetComponent<Animator>().SetBool("trigger", true);
            challenge = DBC.GetChallenge(ChallID);
            challID = ChallID;
            //UI
            titleText.text = challenge.title;
            mapImage.sprite = challenge.map;
            TimeSpan timeRestr = new TimeSpan(0, (int)challenge.timeRestr.x, (int)challenge.timeRestr.y);
            timeText.text = string.Format("Time Restriction: {0:m}", timeRestr);
            //Fee
            for (int i = 0; i < challenge.startFee.GetLength(0); i++) {
                feeIcons[i].gameObject.SetActive(true);
                feeIcons[i].sprite = challenge.startFee[i].ItemIcon;
                feeTexts[i].gameObject.SetActive(true);
                feeTexts[i].text = challenge.startFee[i].ItemQuantity.ToString();
            }
            //Reward
            for (int i = 0; i < challenge.reward.GetLength(0); i++) {
                rewardIcons[i].gameObject.SetActive(true);
                rewardIcons[i].sprite = challenge.reward[i].ItemIcon;
                rewardTexts[i].gameObject.SetActive(true);
                rewardTexts[i].text = challenge.reward[i].ItemQuantity.ToString();
            }
        }

        void StartChallenge() {
            UIMessageWindow.Instance.ShowMessage("Are you really want to begin challenge " + challenge.title + "?", challID, UIAction.startChallenge);
        }

    }
}
