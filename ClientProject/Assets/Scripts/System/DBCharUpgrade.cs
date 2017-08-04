using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DBCharUpgrade : ScriptableObject {

    public List<CharUpgradeLine> HPUpgrade;
    public List<CharUpgradeLine> MPUpgrade;
    public List<CharUpgradeLine> FurnaceUpgrade;

    [Serializable]
    public struct CharUpgradeLine {
        public float toValue;
        public string res1;
        public float quan1;
        public string res2;
        public float quan2;
        public string res3;
        public float quan3;
    }

    private static DBCharUpgrade dbCharUpgrade;

    public CharUpgradeLine GetStatLine(Skill.StatType stat, float value) {
        int index = 0;
        switch (stat) {
            case Skill.StatType.Health:
                index = HPUpgrade.FindIndex(x => x.toValue > value);
                return HPUpgrade[index];
            case Skill.StatType.Mana:
                index = MPUpgrade.FindIndex(x => x.toValue > value);
                return MPUpgrade[index];
        }
        return HPUpgrade[0];
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create DBCharUpgrade asset")]
    private static void CreateContainer() {
        if (Resources.Load<DBCharUpgrade>("DBCharUpgrade") == null) {
            var obj = ScriptableObject.CreateInstance<DBCharUpgrade>();
            AssetDatabase.CreateAsset(obj, "Assets/Resources/DBCharUpgrade.asset");
            AssetDatabase.SaveAssets();
        }
    }

    [CustomPropertyDrawer(typeof(CharUpgradeLine))]
    public class CharUpgradeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect rectValue = new Rect(position.x, position.y, position.width * 0.1f, position.height);
            Rect rectRes1 = new Rect(rectValue.x + rectValue.width, position.y, position.width * 0.2f, position.height);
            Rect rectQuan1 = new Rect(rectRes1.x + rectRes1.width, position.y, position.width * 0.1f, position.height);
            Rect rectRes2 = new Rect(rectQuan1.x + rectQuan1.width, position.y, position.width * 0.2f, position.height);
            Rect rectQuan2 = new Rect(rectRes2.x + rectRes2.width, position.y, position.width * 0.1f, position.height);
            Rect rectRes3 = new Rect(rectQuan2.x + rectQuan2.width, position.y, position.width * 0.2f, position.height);
            Rect rectQuan3 = new Rect(rectRes3.x + rectRes3.width, position.y, position.width * 0.1f, position.height);

            EditorGUI.PropertyField(rectValue, property.FindPropertyRelative("toValue"), GUIContent.none);
            EditorGUI.PropertyField(rectRes1, property.FindPropertyRelative("res1"), GUIContent.none);
            EditorGUI.PropertyField(rectQuan1, property.FindPropertyRelative("quan1"), GUIContent.none);
            EditorGUI.PropertyField(rectRes2, property.FindPropertyRelative("res2"), GUIContent.none);
            EditorGUI.PropertyField(rectQuan2, property.FindPropertyRelative("quan2"), GUIContent.none);
            EditorGUI.PropertyField(rectRes3, property.FindPropertyRelative("res3"), GUIContent.none);
            EditorGUI.PropertyField(rectQuan3, property.FindPropertyRelative("quan3"), GUIContent.none);
        }
    }

#endif

    public static DBCharUpgrade Instance {
        get {
            if (dbCharUpgrade == null) {
                dbCharUpgrade = Resources.Load<DBCharUpgrade>("DBCharUpgrade");
            }
            return dbCharUpgrade;
        }
    }



}
