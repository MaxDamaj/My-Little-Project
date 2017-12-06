using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIItemsCraft : MonoBehaviour {

    [SerializeField]
    private UICraftTable craftTable = null;

    public Button meltButton;
    public Button upgradeButton;
    public List<Transform> furnace;
    public Text[] itemsCount;
    public Transform furnaseResult;
    public Text warningText;
    public UIStatUpgrade upgradeStat;
    public Button recipeButton;
    public Image[] recipeItemSamples;

    private bool IsEnoughItem = false;
    private static UIItemsCraft _uiItemCraft;

    #region API

    public static UIItemsCraft Instance {
        get {
            if (_uiItemCraft == null) {
                _uiItemCraft = FindObjectOfType<UIItemsCraft>();
            }
            return _uiItemCraft;
        }
    }

    void Start() {
        meltButton.onClick.AddListener(Melting);
        upgradeButton.onClick.AddListener(Upgrade);
        warningText.gameObject.SetActive(false);
        recipeButton.onClick.AddListener(delegate { craftTable.gameObject.SetActive(true); });
        Database.onRefresh += RefreshUI;
        RefreshUI();
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }

    #endregion

    public void RefreshUI() {
        //Set active furnace slots
        for (int i = 0; i < furnace.Count; i++) {
            furnace[i].parent.gameObject.SetActive(Database.Instance.furnaceSlots > i);
        }
        SetFurnaceUpgradeStats();
        HideWarningText();

        List<CraftComponent> item = new List<CraftComponent>();
        foreach (var res in furnace) {
            if (res.GetComponentInChildren<CraftComponent>() != null)
                item.Add(res.GetComponentInChildren<CraftComponent>());
        }
        if (recipeItemSamples[0].gameObject.activeSelf) return;
        recipeItemSamples[4].gameObject.SetActive(false);
        for (int i = 0; i < itemsCount.GetLength(0); i++) {
            itemsCount[i].text = "";
        }

        if (item.Count < 2) return;

        UsableItem result = null;
        if (item.Count == 2) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title); }
        if (item.Count == 3) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title, item[2].title); }
        if (item.Count == 4) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title, item[2].title, item[3].title); }
        if (result == null) return;
        //------
        foreach (var res in item) {
            res.transform.parent.parent.GetComponentInChildren<Text>().text = "" + result.GetCostItemQuantity(res.title);
            if (result.GetCostItemQuantity(res.title) > Database.Instance.GetItemQuantity(res.title) && !res.IsItem) {
                res.transform.parent.parent.GetComponentInChildren<Text>().color = Database.COLOR_RED;
            } else {
                res.transform.parent.parent.GetComponentInChildren<Text>().color = Database.COLOR_GREEN;
            }
        }
        itemsCount[4].text = "" + result.exitQuantity;
        recipeItemSamples[4].gameObject.SetActive(true);
        if (Database.Instance.GetItemExist(result.ItemName)) {
            recipeItemSamples[4].sprite = Database.Instance.GetItemIcon(result.ItemName);
        }
        ShowNotification("You can create " + result.ItemName, Database.COLOR_YELLOW, 0);
    }

    void Melting() {
        List<CraftComponent> item = new List<CraftComponent>();
        foreach (var res in furnace) {
            if (res.GetComponentInChildren<CraftComponent>() != null)
                item.Add(res.GetComponentInChildren<CraftComponent>());
        }

        if (item.Count < 2) {
            ShowNotification("Place two or more different materials in furnace!", Database.COLOR_RED);
            return;
        }

        UsableItem result = null;
        if (item.Count == 2) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title); }
        if (item.Count == 3) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title, item[2].title); }
        if (item.Count == 4) { result = Database.Instance.GetUsableItem(item[0].title, item[1].title, item[2].title, item[3].title); }

        if (result == null) {
            ShowNotification("There are no recipe with current components!", Database.COLOR_RED);
            return;
        }
        if (!CheckEnoughItems(result)) {
            ShowNotification("You don't have enough materials!", Database.COLOR_RED);
            return;
        }
        if (furnaseResult.childCount != 0) {
            ShowNotification("Take your created item from furnace first!", Database.COLOR_RED);
            return;
        }

        //Create item
        foreach (var res in item) {
            if (!res.IsItem) {
                Database.Instance.IncreaseItemQuantity(res.title, -result.GetCostItemQuantity(res.title));
            } else {
                Destroy(res.gameObject);
            }
        }
        Database.Instance.IncreaseItemQuantity(result.ItemName, result.exitQuantity);
        Database.Instance.craftedComps++;
        AchievementsController.Instance.CheckStates();
        ShowNotification(result.ItemName + " crafted!", Database.COLOR_GREEN);


    }

    void Upgrade() {
        if (IsEnoughItem) {
            Database.Instance.furnaceSlots++;
            var upgrade = DBCharUpgrade.Instance.FurnaceUpgrade.Find(x => x.toValue == Database.Instance.furnaceSlots);
            Database.Instance.IncreaseItemQuantity(upgrade.res1, -upgrade.quan1);
            Database.Instance.IncreaseItemQuantity(upgrade.res2, -upgrade.quan2);
            RefreshUI();
        } else {
            ShowNotification("You don't have enough materials!", Database.COLOR_RED);
        }
    }

    void SetFurnaceUpgradeStats() {
        switch (Database.Instance.furnaceSlots) {
            case 2:
                IsEnoughItem = upgradeStat.UpgradeCost(DBCharUpgrade.Instance.FurnaceUpgrade[0].res1, DBCharUpgrade.Instance.FurnaceUpgrade[0].res2,
                    DBCharUpgrade.Instance.FurnaceUpgrade[0].quan1, DBCharUpgrade.Instance.FurnaceUpgrade[0].quan2);
                break;
            case 3:
                IsEnoughItem = upgradeStat.UpgradeCost(DBCharUpgrade.Instance.FurnaceUpgrade[1].res1, DBCharUpgrade.Instance.FurnaceUpgrade[1].res2,
                    DBCharUpgrade.Instance.FurnaceUpgrade[1].quan1, DBCharUpgrade.Instance.FurnaceUpgrade[1].quan2);
                break;
            case 4:
                upgradeStat.itemIcon[0].gameObject.SetActive(false);
                upgradeStat.itemText[0].gameObject.SetActive(false);
                upgradeStat.itemIcon[1].gameObject.SetActive(false);
                upgradeStat.itemText[1].gameObject.SetActive(false);
                upgradeButton.interactable = false;
                break;
        }
    }

    void ShowNotification(string message, Color textColor, float hideDelay = 3.0f) {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        warningText.color = textColor;
        if (hideDelay > 0) {
            Invoke("HideWarningText", hideDelay);
        }
    }

    void HideWarningText() {
        warningText.gameObject.SetActive(false);
    }

    bool CheckEnoughItems(UsableItem recipe) {
        for (int i = 0; i < recipe.costItems.Count; i++) {
            if (recipe.costPrices[i] > Database.Instance.GetItemQuantity(recipe.costItems[i]) && Database.Instance.GetItemExist(recipe.costItems[i]))
                return false;
        }
        return true;
    }

    public void SetRecipeItemsSalples(string[] materials, float[] values, string result, float exitCount) {
        if (materials.GetLength(0) > 4) return;
        if (values.GetLength(0) > 4) return;
        for (int i = 0; i < itemsCount.GetLength(0); i++) {
            itemsCount[i].text = "";
        }
        //Set components
        foreach (var item in recipeItemSamples) {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < materials.GetLength(0); i++) {
            recipeItemSamples[i].gameObject.SetActive(true);
            recipeItemSamples[i].sprite = Database.Instance.GetItemIcon(materials[i]);
            itemsCount[i].text = "" + values[i];
            itemsCount[i].color = Color.white;
        }
        //Set result
        recipeItemSamples[4].gameObject.SetActive(true);
        recipeItemSamples[4].sprite = Database.Instance.GetItemIcon(result);
        itemsCount[4].text = "" + exitCount;
    }

    public void ClearRecipeSamples() {
        foreach (var item in recipeItemSamples) {
            item.gameObject.SetActive(false);
        }
    }

}
