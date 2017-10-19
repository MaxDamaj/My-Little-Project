using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PonyType {
    EarthPony = 0,
    Unicorn = 1,
    Pegasus = 2,
    Alicorn = 3,
    Unaligned = 4
};

[Serializable]
public class StorageData {
    public string ItemName;
    public float ItemQuantity;
    public Sprite ItemIcon;
}

[Serializable]
public class UsableItem {
    public string ItemName;
    public string Description;
    public List<string> costItems;
    public float[] costPrices;
    public float exitQuantity = 1;
    public GameObject prefab;
    public bool IsItem;

    public float GetCostItemQuantity(string title) {
        float value = 0;
        for (int i = 0; i < costItems.Count; i++) {
            if (costItems[i] == title)
                value = costPrices[i];
        }
        return value;
    }
}

[Serializable]
public class CharsFMData {
    public string CharName;
    public Color CharColor;
    public Sprite CharIcon;
    public PonyType Type;
    public int Rank;
    public float HP;
    public float MP;
    public float SPD;
    public float STMCurrent;
    public float STMMax;
    public float MPRecovery;
    public GameObject PreviewPrefab;
    public GameObject GamePlayPrefab;
    public Skill[] CharSkills;
    public string[] costItems;
    public float[] costPrises;

    CharsFMData(CharsFMData character) {
        CharName = character.CharName;
        CharColor = character.CharColor;
        CharIcon = character.CharIcon;
        Type = character.Type;
        Rank = character.Rank;
        HP = character.HP;
        MP = character.MP;
        SPD = character.SPD;
        STMCurrent = character.STMCurrent;
        STMMax = character.STMMax;
        MPRecovery = character.MPRecovery;
        PreviewPrefab = character.PreviewPrefab;
        GamePlayPrefab = character.GamePlayPrefab;
        CharSkills = character.CharSkills;
        costItems = character.costItems;
        costPrises = character.costPrises;
    }
}

public class Database : MonoBehaviour {

    public static Color COLOR_GREEN = new Color(0.455f, 1.000f, 0.243f);
    public static Color COLOR_RED = new Color(1.000f, 0.243f, 0.243f);
    public static Color COLOR_YELLOW = Color.yellow;
    public static Color COLOR_GREY = new Color(0.588f, 0.588f, 0.588f);

    private static Database database;

    //Storage Items and icons
    [SerializeField]
    List<StorageData> _storageItems = null;
    //Usable craftable Items
    [SerializeField]
    List<UsableItem> _usableItems = null;
    //Free Mode Characters
    [SerializeField]
    List<CharsFMData> _freeModeChars = null;

    [Header("Common")]
    public int timeSpan;        //time span in seconds between last saving and 01.01.2016
    public int SelectedPony;    //Current selected pony in FreeMode
    public int[] PartyPony;     //Ponies in your party
    public bool IsLoaded;       //Is current Database loaded from disk
    public int tutorialState;
    public int[] nowPlaying;    //Current playing music in different modes
    public float cameraShift;   //Current camera shifting position in gamemode
    public int paramBloom;      //Camera using Antialising
    public int paramSSAO;       //Camera using Ambient Obscurance
    public int furnaceSlots;    //Number of furnace slots

    [Header("Statistic")]
    public int distTotal;       //Total passed distance in free mode
    public int distEnd;         //Passed distance in endurance mode
    public int distChall;       //Passed distance in challenge mode
    public int obstTotal;       //Total wrecked obstacles
    public int obstWithDamage;  //Wrecked obstacles with recieving damage
    public int obstNonDamage;   //Wrecked obstacles without recieving damage
    public int distEndEasy;

    [Header("Lists")]
    public int[] passedChallenges;
    public List<int> takenAchievements;
    public int[] readenCodex;
    public int[] endRewardsEasy;

    [Header("Items")]
    public List<string> itemsInventory;
    public List<string> itemsBelt;
    public List<string> itemsFurnace;

    public delegate void RefreshArgs(); //variable for event values
    public static event RefreshArgs onRefresh; //Refresh event

    //Instance
    public static Database Instance {
        get {
            if (database == null) {
                database = FindObjectOfType<Database>();
            }
            return database;
        }
    }

    #region Materials

    public void IncreaseItemQuantity(string itemName, float quantity) {
        for (int i = 0; i < _storageItems.Count; i++) {
            if (_storageItems[i].ItemName == itemName) {
                _storageItems[i].ItemQuantity += quantity;
            }
        }
        if (onRefresh != null) { onRefresh(); }
    }
    public void IncreaseItemQuantity(int index, float quantity) {
        _storageItems[index].ItemQuantity += quantity;
        if (onRefresh != null) { onRefresh(); }
    }
    public void SetItemQuantity(int index, float quantity) {
        _storageItems[index].ItemQuantity = quantity;
        if (onRefresh != null) { onRefresh(); }
    }
    public float GetItemQuantity(string itemName) {
        for (int i = 0; i < _storageItems.Count; i++) {
            if (_storageItems[i].ItemName == itemName) {
                return _storageItems[i].ItemQuantity;
            }
        }
        return 0;
    }
    public float GetItemQuantity(int index) {
        return _storageItems[index].ItemQuantity;
    }
    public Sprite GetItemIcon(string itemName) {
        for (int i = 0; i < _storageItems.Count; i++) {
            if (_storageItems[i].ItemName == itemName) {
                return _storageItems[i].ItemIcon;
            }
        }
        return _storageItems[0].ItemIcon;
    }
    public Sprite GetItemIcon(int index) {
        return _storageItems[index].ItemIcon;
    }
    public string GetItemTitle(int index) {
        return _storageItems[index].ItemName;
    }
    public bool GetItemExist(string title) {
        if (_storageItems.Find(x => x.ItemName == title) != null) {
            return true;
        }
        return false;
    }

    #endregion

    #region Recipes

    public UsableItem GetUsableItem(string name) {
        return _usableItems.Find(x => x.ItemName == name);
    }
    public UsableItem GetUsableItem(int id) {
        return _usableItems[id];
    }
    public UsableItem GetUsableItem(string material1, string material2) {
        List<string> materials = new List<string>();
        materials.Add(material1);
        materials.Add(material2);

        var items = _usableItems.FindAll(x => x.costItems.Count == 2);
        items = items.FindAll(x => (x.costItems.Find(y => y == material1) != null && x.costItems.Find(z => z == material2) != null));
        if (materials.FindAll(w => w == material1).Count > 1) { return null; }
        if (materials.FindAll(w => w == material2).Count > 1) { return null; }
        return items.Count > 0 ? items[0] : null;
    }
    public UsableItem GetUsableItem(string material1, string material2, string material3) {
        List<string> materials = new List<string>();
        materials.Add(material1);
        materials.Add(material2);
        materials.Add(material3);

        var items = _usableItems.FindAll(x => x.costItems.Count == 3);
        items = items.FindAll(x => (x.costItems.Find(a => a == material1) != null && x.costItems.Find(b => b == material2) != null && x.costItems.Find(c => c == material3) != null));
        if (materials.FindAll(w => w == material1).Count > 1) { return null; }
        if (materials.FindAll(w => w == material2).Count > 1) { return null; }
        if (materials.FindAll(w => w == material3).Count > 1) { return null; }
        return items.Count > 0 ? items[0] : null;
    }
    public UsableItem GetUsableItem(string material1, string material2, string material3, string material4) {
        List<string> materials = new List<string>();
        materials.Add(material1);
        materials.Add(material2);
        materials.Add(material3);
        materials.Add(material4);

        var items = _usableItems.FindAll(x => x.costItems.Count == 4);
        items = items.FindAll(x => (x.costItems.Find(a => a == material1) != null && x.costItems.Find(b => b == material2) != null && x.costItems.Find(c => c == material3) != null && x.costItems.Find(d => d == material4) != null));
        if (materials.FindAll(w => w == material1).Count > 1) { return null; }
        if (materials.FindAll(w => w == material2).Count > 1) { return null; }
        if (materials.FindAll(w => w == material3).Count > 1) { return null; }
        if (materials.FindAll(w => w == material4).Count > 1) { return null; }
        return items.Count > 0 ? items[0] : null;
    }

    public int GetMaterialIndexInRecipe(UsableItem item, string material) {
        for (int i = 0; i < item.costItems.Count; i++) {
            if (item.costItems[i] == material) {
                return i;
            }
        }
        return -1;
    }

    #endregion  

    #region Characters

    //Get states
    public CharsFMData GetCharFMInfo(int id) {
        return _freeModeChars[id];
    }
    public CharsFMData GetCharFMInfo(string name) {
        return _freeModeChars.Find(x => x.CharName == name);
    }
    public int GetCharFMRank(int id) {
        return _freeModeChars[id].Rank;
    }
    public PonyType GetCharType(int id) {
        return _freeModeChars[id].Type;
    }
    public int GetUnlockedCharsCount() {
        int count = 0;
        foreach (var pony in _freeModeChars) {
            if (pony.Rank >= 0) count++;
        }
        return count;
    }
    //Set states
    public void SetCharFM_HP(int id, float HP) {
        _freeModeChars[id].HP = HP;
        if (onRefresh != null) { onRefresh(); }
    }
    public void SetCharFM_MP(int id, float MP) {
        _freeModeChars[id].MP = MP;
        if (onRefresh != null) { onRefresh(); }
    }
    public void SetCharFM_SPD(int id, float SPD) {
        _freeModeChars[id].SPD = SPD;
        if (onRefresh != null) { onRefresh(); }
    }
    public void SetCharFMRank(int id, int rank) {
        _freeModeChars[id].Rank = rank;
        if (onRefresh != null) { onRefresh(); }
    }
    //Stamina
    public void IncreaseCurrSTM(int id, float value) {
        _freeModeChars[id].STMCurrent += value;
        if (_freeModeChars[id].STMCurrent > _freeModeChars[id].STMMax) { _freeModeChars[id].STMCurrent = _freeModeChars[id].STMMax; }
        if (onRefresh != null) { onRefresh(); }
    }
    public void SetCurrSTM(int id, float value) {
        _freeModeChars[id].STMCurrent = value;
        if (onRefresh != null) { onRefresh(); }
    }
    public float GetCurrSTM(int id) {
        return _freeModeChars[id].STMCurrent;
    }
    public float GetMaxSTM(int id) {
        return _freeModeChars[id].STMMax;
    }

    #endregion

    //Lists Lenght
    public int ArrayItemsGetLenght() {
        return _storageItems.Count;
    }
    public int ArrayCharFMGetLenght() {
        return _freeModeChars.Count;
    }
    public int ArrayUsableItemsGetLenght() {
        return _usableItems.Count;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StorageData))]
    public class StorageDataDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect rectName = new Rect(position.x, position.y, position.width * 0.4f, position.height);
            Rect rectQuan = new Rect(rectName.x + rectName.width, position.y, position.width * 0.2f, position.height);
            Rect rectIcon = new Rect(rectQuan.x + rectQuan.width, position.y, position.width * 0.4f, position.height);

            EditorGUI.PropertyField(rectName, property.FindPropertyRelative("ItemName"), GUIContent.none);
            EditorGUI.PropertyField(rectQuan, property.FindPropertyRelative("ItemQuantity"), GUIContent.none);
            EditorGUI.PropertyField(rectIcon, property.FindPropertyRelative("ItemIcon"), GUIContent.none);

        }
    }

#endif

}
