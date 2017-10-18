using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class RunRewards {
    public int distance;
    public string[] rewardItems;
    public float[] rewardPrices;
    public string stage;
}

public class DBEndRewards : ScriptableObject {

    public RunRewards[] EasyRewards;

    private static DBEndRewards dbEndRewards;

#if UNITY_EDITOR
    [MenuItem("Assets/Create DBEndRewards asset")]
    private static void CreateContainer() {
        if (Resources.Load<DBEndRewards>("DBEndRewards") == null) {
            var obj = ScriptableObject.CreateInstance<DBEndRewards>();
            AssetDatabase.CreateAsset(obj, "Assets/Resources/DBEndRewards.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif

    public static DBEndRewards Instance {
        get {
            if (dbEndRewards == null) {
                dbEndRewards = Resources.Load<DBEndRewards>("DBEndRewards");
            }
            return dbEndRewards;
        }
    }

}
