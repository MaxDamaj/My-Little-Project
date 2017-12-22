using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MLA.System;
using MLA.UI.Common;

namespace MLA.UI.Windows {
    public class UIChallenge : MonoBehaviour {

        [SerializeField]
        DBChallenges DBC = null;

        public UIChallengeFull fullWindow;

        [Header("UI")]
        public Button challButton;
        public Text challTitle;
        public Image challBG;
        public GameObject lockSprite;
        public GameObject passSprite;
        public Text psText;
        public Image[] feeTypes;
        [Header("Fee")]
        public GameObject feeGameobject;
        public Image[] feeIcons;
        public Text[] feeTexts;
        public UIPriceCheck[] feePriceChecks;
        [Header("Reward")]
        public GameObject rewardGameobject;
        public Image[] rewardIcons;
        public Text[] rewardTexts;

        [Header("Common")]
        public int ChallID;
        public Color colorInactive;

        private float timer;
        private float feeScale;
        private float rewardScale;
        private Challenge challenge;

        void Start() {
            challenge = DBC.GetChallenge(ChallID);
            //UI
            challButton.onClick.AddListener(ShowStartMessage);
            challTitle.text = challenge.title;
            challBG.sprite = challenge.bg;
            psText.text = "You need " + challenge.PSRestr + " PS";
            switch (challenge.charRestr) {
                case PonyType.EarthPony:
                    feeTypes[1].color = colorInactive;
                    feeTypes[2].color = colorInactive;
                    break;
                case PonyType.Unicorn:
                    feeTypes[0].color = colorInactive;
                    feeTypes[2].color = colorInactive;
                    break;
                case PonyType.Pegasus:
                    feeTypes[0].color = colorInactive;
                    feeTypes[1].color = colorInactive;
                    break;
            }
            //Fee
            feeGameobject.SetActive(true);
            for (int i = 0; i < challenge.startFee.GetLength(0); i++) {
                feeIcons[i].gameObject.SetActive(true);
                feeIcons[i].sprite = challenge.startFee[i].ItemIcon;
                feeTexts[i].gameObject.SetActive(true);
                feeTexts[i].text = challenge.startFee[i].ItemQuantity.ToString();
                feePriceChecks[i].item = challenge.startFee[i].ItemName;
                feePriceChecks[i].value = challenge.startFee[i].ItemQuantity;
            }
            //Reward
            rewardGameobject.SetActive(true);
            for (int i = 0; i < challenge.reward.GetLength(0); i++) {
                rewardIcons[i].gameObject.SetActive(true);
                rewardIcons[i].sprite = challenge.reward[i].ItemIcon;
                rewardTexts[i].gameObject.SetActive(true);
                rewardTexts[i].text = challenge.reward[i].ItemQuantity.ToString();
            }
            rewardGameobject.transform.localScale = new Vector3(1, 0, 1);
            Database.onRefresh += RefreshUI;
            RefreshUI();
            timer = 0;
        }


        void FixedUpdate() {
            timer += Time.deltaTime;
            //Show Fee
            if (timer >= 0 && timer < 0.5f) {
                feeScale += Time.deltaTime * 2;
                rewardScale = 0;
            }
            //Fee Showed
            if (timer >= 0.5f && timer < 5f) feeScale = 1;
            //Hiding Fee
            if (timer >= 5 && timer < 5.5f) feeScale -= Time.deltaTime * 2;

            //Show Reward
            if (timer >= 5.5f && timer < 6) {
                rewardScale += Time.deltaTime * 2;
                feeScale = 0;
            }
            //Reward Showed
            if (timer >= 6 && timer < 11) rewardScale = 1;
            //Hiding Reward
            if (timer >= 11 && timer < 11.5f) rewardScale -= Time.deltaTime * 2;

            //Go to firts step
            if (timer >= 11.5f) timer = 0;
            //Set Scales
            feeGameobject.transform.localScale = new Vector3(1, feeScale, 1);
            rewardGameobject.transform.localScale = new Vector3(1, rewardScale, 1);
        }

        void RefreshUI() {
            if (lockSprite != null)
                lockSprite.SetActive(UIPSCalculation.PartyStrength < challenge.PSRestr);
            if (passSprite != null)
                passSprite.SetActive(Database.Instance.passedChallenges[ChallID] > 0);
        }

        void ShowStartMessage() {
            //Resources Check
            foreach (var item in challenge.startFee) {
                if (Database.Instance.GetItemQuantity(item.ItemName) < item.ItemQuantity) {
                    UIMessageWindow.Instance.ShowMessage("You don't have enough items to start this challenge!", 0, UIAction.nothing, true, false);
                    return;
                }
            }
            //Pony type check
            if (challenge.charRestr != PonyType.Unaligned) {
                if (challenge.charRestr != Database.Instance.GetCharType(Database.Instance.SelectedPony)) {
                    UIMessageWindow.Instance.ShowMessage("You need a " + challenge.charRestr.ToString() + " to begin this challenge!", 0, UIAction.nothing, true, false);
                    return;
                }
            }
            fullWindow.ShowWindow(ChallID);
        }

        void OnDestroy() {
            Database.onRefresh -= RefreshUI;
        }
    }
}
