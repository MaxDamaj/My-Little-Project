using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GlobalData : ScriptableObject {

    [Header("Non saved params")]
    public int nowChallenge;
    public int itemsInventoryCount;
    public int itemstBeltCount;

    private static GlobalData globalData;

    void Start() {

    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create GlobalData asset")]
    private static void CreateGDContainer() {
        var obj = ScriptableObject.CreateInstance<GlobalData>();
        AssetDatabase.CreateAsset(obj, "Assets/Resources/GlobalData.asset");
        AssetDatabase.SaveAssets();
    }
#endif

    public static GlobalData Instance {
        get {
            if (globalData == null) {
                globalData = Resources.Load<GlobalData>("GlobalData");
            }
            return globalData;
        }
    }

}
