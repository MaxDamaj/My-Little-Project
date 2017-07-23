using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharFMInfoFull : MonoBehaviour {

    [SerializeField]
    private UICharFMSkills _uiSkills = null;
    public UIMessageWindow mWindow;

    public UIStatUpgrade[] UIStat;

    public Text CharName;
    public Image CharIcon;
    public Text HPText, MPText, SPDText;
    public Text STMText;
    public Button[] upgButton;
    public Text[] upgText;

    private CharsFMData Character;
    private bool IsEnoughItems;
    private string i1, i2, i3;
    private float quan1, quan2, quan3;

    void Start() {
        IsEnoughItems = false;
        upgButton[0].onClick.AddListener(UpgradeHealth);
        upgButton[1].onClick.AddListener(UpgradeMana);
        Database.onRefresh += RefreshUI;
    }

    public void RefreshUI() {
        Character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        CharName.text = Character.CharName;
        CharIcon.sprite = Character.CharIcon;
        CharName.color = Character.CharColor;
        HPText.text = "" + Character.HP;
        MPText.text = "" + Character.MP;
        SPDText.text = "" + Character.SPD;
        STMText.text = Mathf.RoundToInt(Character.STMCurrent) + "/" + Character.STMMax;
        //Show Upgrade values
        upgText[0].text = "+" + 0.5f + 0.5f * Character.Rank / 10;
        upgText[1].text = "+" + 0.5f + 0.5f * Character.Rank / 10;
        //------------
        _uiSkills.RefreshUI();
        UpgradeHealthCost();
        UpgradeManaCost();
    }

    //-----------Health-Upgrade----------------
    void UpgradeHealthCost() {
		var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Health, Character.HP);
        //Set Upgrade Cost
		i1 = line.res1;
		i2 = line.res2;
		i3 = line.res3;
		quan1 = line.quan1 + (line.quan1) * 0.2f * (Character.HP - line.toValue + 5);
		quan2 = line.quan2 + (line.quan2) * 0.2f * (Character.HP - line.toValue + 5);
		quan3 = line.quan3 + (line.quan3) * 0.2f * (Character.HP - line.toValue + 5);
        //Set items values
        IsEnoughItems = UIStat[0].UpgradeCost(i1, i2, i3, quan1, quan2, quan3);
    }
    void UpgradeHealth() {
		var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Health, Character.HP);
        if (IsEnoughItems) {
			Database.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (line.quan1) * 0.2f * (Character.HP - line.toValue + 5)));
			Database.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (line.quan2) * 0.2f * (Character.HP - line.toValue + 5)));
			Database.Instance.IncreaseItemQuantity(line.res3, 0 - (line.quan3 + (line.quan3) * 0.2f * (Character.HP - line.toValue + 5)));
            Character.HP += 0.5f;
            Database.Instance.SetCharFM_HP(Database.Instance.SelectedPony, Character.HP);
        } else {
            mWindow.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
        }
    }

    //-----------Mana-Upgrade----------------
	void UpgradeManaCost() {
		var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Mana, Character.MP);
		//Set Upgrade Cost
		i1 = line.res1;
		i2 = line.res2;
		i3 = line.res3;
		quan1 = line.quan1 + (line.quan1) * 0.2f * (Character.MP - line.toValue + 5);
		quan2 = line.quan2 + (line.quan2) * 0.2f * (Character.MP - line.toValue + 5);
		quan3 = line.quan3 + (line.quan3) * 0.2f * (Character.MP - line.toValue + 5);
        //Set items values
        IsEnoughItems = UIStat[1].UpgradeCost(i1, i2, i3, quan1, quan2, quan3);
    }
    void UpgradeMana() {
		var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Mana, Character.MP);
        if (IsEnoughItems) {
			Database.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (line.quan1) * 0.2f * (Character.MP - line.toValue + 5)));
			Database.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (line.quan2) * 0.2f * (Character.MP - line.toValue + 5)));
			Database.Instance.IncreaseItemQuantity(line.res3, 0 - (line.quan3 + (line.quan3) * 0.2f * (Character.MP - line.toValue + 5)));
            Character.MP += 0.5f;
            Database.Instance.SetCharFM_MP(Database.Instance.SelectedPony, Character.MP);
        } else {
            mWindow.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
        }
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }

}
