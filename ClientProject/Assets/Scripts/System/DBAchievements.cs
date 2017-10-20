using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AchieveType {
    Nothing, TotalDist, EndDist, ChallDist, TotalObst, DamObst, NonDamObst, PassChallInTotal, TotalPS, CharsOwned, ComponentsCount, TargetCharOwned, CraftedComps
};

[Serializable]
public class Achievement {
    public string title;
    public string description;
    public AchieveType condition;
    public float value;
    public DBCharUpgrade.CharUpgradeLine reward;
}

public class DBAchievements : ScriptableObject {

    [SerializeField]
    List<Achievement> _achievementsList = null;

    private static DBAchievements dbAchievements;

#if UNITY_EDITOR
    [MenuItem("Assets/Create DBAchievements asset")]
    private static void CreateContainer() {
        if (Resources.Load<DBAchievements>("DBAchievements") == null) {
            var obj = ScriptableObject.CreateInstance<DBAchievements>();
            AssetDatabase.CreateAsset(obj, "Assets/Resources/DBAchievements.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif

    public static DBAchievements Instance {
        get {
            if (dbAchievements == null) {
                dbAchievements = Resources.Load<DBAchievements>("DBAchievements");
            }
            return dbAchievements;
        }
    }

    public Achievement GetAchievement(int index) {
        return _achievementsList[index];
    }
    public int GetAchievementsCount() {
        return _achievementsList.Count;
    }


}
