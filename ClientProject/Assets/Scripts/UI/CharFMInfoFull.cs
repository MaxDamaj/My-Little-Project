using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharFMInfoFull : MonoBehaviour {

    [SerializeField]
    private UICharFMSkills _uiSkills = null;

    public UIStatUpgrade[] UIStat;

    public Text CharName;
    public Image CharIcon;
    public Text HPText, MPText, SPDText, LUCKText;
    public Text STMText;
    public Button[] upgButton;
    public Text[] upgText;

    private CharsFMData Character;
    private bool IsEnoughToHP, IsEnoughToMP, IsEnoughToLuck;
    private string i1, i2, i3;
    private int quan1, quan2, quan3;

    #region API

    void Start() {
        IsEnoughToHP = false;
        IsEnoughToMP = false;
        upgButton[0].onClick.AddListener(UpgradeHealth);
        upgButton[1].onClick.AddListener(UpgradeMana);
        upgButton[2].onClick.AddListener(UpgradeLuck);
        Database.onRefresh += RefreshUI;
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }

    #endregion

    public void RefreshUI() {
        Character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        CharName.text = Character.CharName;
        CharIcon.sprite = Character.CharIcon;
        CharName.color = Character.CharColor;
        HPText.text = "" + Character.HP;
        MPText.text = "" + Character.MP;
        SPDText.text = "" + Character.SPD;
        LUCKText.text = "" + Character.LUCK;
        STMText.text = Mathf.RoundToInt(Character.STMCurrent) + "/" + Character.STMMax;
        //Show Upgrade values
        upgText[0].text = "+0.5";
        upgText[1].text = "+0.5";
        if (Character.LUCK > 90) {
            upgButton[2].transform.parent.gameObject.SetActive(false);
        } else {
            upgButton[2].transform.parent.gameObject.SetActive(true);
            upgText[2].text = "+10";
        }
        //------------
        _uiSkills.RefreshUI();
        UpgradeHealthCost();
        UpgradeManaCost();
        UpgradeLuckCost();
    }

    //-----------Health-Upgrade----------------
    void UpgradeHealthCost() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Health, Character.HP);
        //Set Upgrade Cost
        i1 = line.res1;
        i2 = line.res2;
        i3 = line.res3;
        quan1 = Mathf.FloorToInt(line.quan1 + (line.quan1) * 0.1f * (Character.HP - line.toValue + 5));
        quan2 = Mathf.FloorToInt(line.quan2 + (line.quan2) * 0.1f * (Character.HP - line.toValue + 5));
        quan3 = Mathf.FloorToInt(line.quan3 + (line.quan3) * 0.1f * (Character.HP - line.toValue + 5));
        //Set items values
        IsEnoughToHP = UIStat[0].UpgradeCost(i1, i2, i3, quan1, quan2, quan3);
    }
    void UpgradeHealth() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Health, Character.HP);
        if (IsEnoughToHP) {
            Database.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (line.quan1) * 0.1f * (Character.HP - line.toValue + 5)));
            Database.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (line.quan2) * 0.1f * (Character.HP - line.toValue + 5)));
            Database.Instance.IncreaseItemQuantity(line.res3, 0 - (line.quan3 + (line.quan3) * 0.1f * (Character.HP - line.toValue + 5)));
            Character.HP += 0.5f;
            Database.Instance.SetCharFM_HP(Database.Instance.SelectedPony, Character.HP);
        } else {
            UIMessageWindow.Instance.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
        }
    }
    //-----------Mana-Upgrade----------------
    void UpgradeManaCost() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Mana, Character.MP);
        //Set Upgrade Cost
        i1 = line.res1;
        i2 = line.res2;
        i3 = line.res3;
        quan1 = Mathf.FloorToInt(line.quan1 + (line.quan1) * 0.1f * (Character.MP - line.toValue + 5));
        quan2 = Mathf.FloorToInt(line.quan2 + (line.quan2) * 0.1f * (Character.MP - line.toValue + 5));
        quan3 = Mathf.FloorToInt(line.quan3 + (line.quan3) * 0.1f * (Character.MP - line.toValue + 5));
        //Set items values
        IsEnoughToMP = UIStat[1].UpgradeCost(i1, i2, i3, quan1, quan2, quan3);
    }
    void UpgradeMana() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Mana, Character.MP);
        if (IsEnoughToMP) {
            Database.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (line.quan1) * 0.1f * (Character.MP - line.toValue + 5)));
            Database.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (line.quan2) * 0.1f * (Character.MP - line.toValue + 5)));
            Database.Instance.IncreaseItemQuantity(line.res3, 0 - (line.quan3 + (line.quan3) * 0.1f * (Character.MP - line.toValue + 5)));
            Character.MP += 0.5f;
            Database.Instance.SetCharFM_MP(Database.Instance.SelectedPony, Character.MP);
        } else {
            UIMessageWindow.Instance.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
        }
    }
    //-----------Luck-Upgrade----------------
    void UpgradeLuckCost() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Luck, Character.LUCK);
        //Set Upgrade Cost
        i1 = line.res1;
        i2 = line.res2;
        i3 = line.res3;
        quan1 = Mathf.FloorToInt(line.quan1);
        quan2 = Mathf.FloorToInt(line.quan2);
        quan3 = Mathf.FloorToInt(line.quan3);
        //Set items values
        IsEnoughToLuck = UIStat[2].UpgradeCost(i1, i2, i3, quan1, quan2, quan3);
    }
    void UpgradeLuck() {
        var line = DBCharUpgrade.Instance.GetStatLine(Skill.StatType.Luck, Character.LUCK);
        if (IsEnoughToLuck) {
            Database.Instance.IncreaseItemQuantity(line.res1, -line.quan1);
            Database.Instance.IncreaseItemQuantity(line.res2, -line.quan2);
            Database.Instance.IncreaseItemQuantity(line.res3, -line.quan3);
            Character.LUCK += 10f;
            Database.Instance.SetCharFMLuck(Database.Instance.SelectedPony, Character.LUCK);
        } else {
            UIMessageWindow.Instance.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
        }
    }


}
