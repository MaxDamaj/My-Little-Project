using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharFMInfoShort : MonoBehaviour {

    public int CharNum;
    public MenuNavigation dummy;
    public CharFMInfoFull window;
    public Animator win_anim;

    public Text CharName;
    public Image CharIcon;
    public Text HPText, MPText, SPDText;
    public Text STMText;
    public GameObject priceTag;
    public UIStatUpgrade cost;

    private CharsFMData Character;

    void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(Activate);
        priceTag.GetComponent<Button>().onClick.AddListener(ShowBuyWindow);
        Database.onRefresh += RefreshUI;
        RefreshUI();
    }

    //Refresh UI info
    void RefreshUI() {
        Character = Database.Instance.GetCharFMInfo(CharNum);
        CharName.text = Character.CharName;
        CharName.color = Character.CharColor;
        CharIcon.sprite = Character.CharIcon;
        HPText.text = "" + Character.HP;
        MPText.text = "" + Character.MP;
        SPDText.text = "" + Character.SPD;
        STMText.text = Mathf.RoundToInt(Character.STMCurrent) + "/" + Character.STMMax;
        if (Character.Rank >= 0) { priceTag.SetActive(false); } else { priceTag.SetActive(true); }
        if (cost != null) { cost.UpgradeCost(Character.costItems[0], Character.costItems[1], Character.costItems[2], Character.costPrises[0], Character.costPrises[1], Character.costPrises[2]); }
    }

    //Button OnClickEvent
    void Activate() {
        if (CharNum != Database.Instance.SelectedPony) {
            Database.Instance.SelectedPony = CharNum;
            Database.Instance.PartyPony[(int)Character.Type] = CharNum;
            UIPartySwitcher.Instance.ChangeCharacter((int)Character.Type);
        } else {
            window.RefreshUI();
            win_anim.SetBool("trigger", true);
        }

    }

    bool PriceCheck() {
        for (int i = 0; i < Character.costPrises.GetLength(0); i++) {
            if (Database.Instance.GetItemQuantity(Character.costItems[i]) < Character.costPrises[i]) { return false; }
        }
        return true;
    }

    void ShowBuyWindow() {
        if (PriceCheck()) {
            UIMessageWindow.Instance.ShowMessage("Are you really want to this pony join to your team?", CharNum, UIAction.buying);
        } else {
            UIMessageWindow.Instance.ShowMessage("You don't have enough materials", 0, UIAction.nothing, true, false);
        }
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }


}
