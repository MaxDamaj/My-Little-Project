using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CodexRow {
    public string header;
    public Color headerColor;
    [Multiline]
    public string text;
}

namespace MLA.System {
    public class CodexList : ScriptableObject {

        public List<CodexRow> codexRows;
        [Header("Shown Condition")]
        public AchieveType condition;
        public float condValue;
        public string condLine;

#if UNITY_EDITOR
        [MenuItem("Assets/Create new Codex list")]
        private static void CreateGDContainer() {
            var obj = ScriptableObject.CreateInstance<CodexList>();
            AssetDatabase.CreateAsset(obj, "Assets/Resources/Codex/_NewCodexList.asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
