using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GlobalData : ScriptableObject {

    [Header("Non saved params")]
    public int nowChallenge;
    public int itemsInventoryCount;
    public int itemsBeltCount;

    [Header("Ingame params")]
    public float SPDmlp;        //Speed multiplier. Used in PonyController
    public float DMGmlp;        //Damage multiplier. Used in PonyController
    public float timeSpeed;     //Current time length. Used in slowing motions skills
    public float currentHP;
    public float currentMP;
    public float currentMP_rec; //MP recovery value

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
