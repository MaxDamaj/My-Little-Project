using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIChallenge : MonoBehaviour {

    [SerializeField]
    DBChallenges DBC = null;

    [Header("UI")]
    public Button challButton;
    public Text challTitle;
    public Image challBG;
    public GameObject distance;
    public Text distanceText;
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
    public UIMessageWindow mWindow;
	public UICardGame cgWindow;

    private float timer;
    private float feeScale;
    private float rewardScale;
    private float distanceScale;
    private Challenge challenge;

    void Start() {
        challenge = DBC.GetChallenge(ChallID);
        //UI
        challButton.onClick.AddListener(ShowStartMessage);
        challTitle.text = challenge.title;
        challBG.sprite = challenge.bg;
        distance.SetActive(true);
        distanceText.text = "" + challenge.distance;
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
            distanceScale = 0;
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

        //Show Distance
        if (timer >= 11.5f && timer < 12) {
            distanceScale += Time.deltaTime * 2;
            rewardScale = 0;
        }
        //Distance Showed
        if (timer >= 12 && timer < 17) distanceScale = 1;
        //Hiding Distance
        if (timer >= 17 && timer < 17.5f) distanceScale -= Time.deltaTime * 2;

        //Go to firts step
        if (timer >= 17.5f) timer = 0;
        //Set Scales
        feeGameobject.transform.localScale = new Vector3(1, feeScale, 1);
        rewardGameobject.transform.localScale = new Vector3(1, rewardScale, 1);
        distance.transform.localScale = new Vector3(1, distanceScale, 1);
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
                mWindow.ShowMessage("You don't have enough items to start this challenge!", 0, UIMessageWindow.Action.nothing, true, false);
                return;
            }
        }
        //Pony type check
        if (challenge.charRestr != PonyType.Unaligned) {
            if (challenge.charRestr != Database.Instance.GetCharType(Database.Instance.SelectedPony)) {
                mWindow.ShowMessage("You need a " + challenge.charRestr.ToString() + " to begin this challenge!", 0, UIMessageWindow.Action.nothing, true, false);
                return;
            }
        }
        //If enough resources show message
		if (challenge.challengeType != ChallType.CardGame) {
			mWindow.ShowMessage("Are you really want to begin challenge " + challenge.title + "?", ChallID, UIMessageWindow.Action.startChallenge);
		} else {
			GlobalData.Instance.nowChallenge = ChallID;
			cgWindow.gameObject.SetActive(true);
		}
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }
}
