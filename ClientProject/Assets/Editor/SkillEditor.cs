using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Skill))]
public class SkillEditor : Editor {

    SerializedObject serObj;

    SerializedProperty title;
    SerializedProperty icon;
    SerializedProperty description;

    SerializedProperty skillType;
    SerializedProperty passiveType;
    SerializedProperty statType;
    SerializedProperty condition;

    SerializedProperty MP_cost;
    SerializedProperty duration;
    SerializedProperty cooldown;
    SerializedProperty chance;
    SerializedProperty multiplier;
    SerializedProperty SPDmlp;
    SerializedProperty DMGmlp;
    SerializedProperty item;
    SerializedProperty items;
    SerializedProperty obj;
    SerializedProperty fx;
    SerializedProperty sound;
    SerializedProperty projPosition;

    void OnEnable() {
        serObj = new SerializedObject(target);

        title = serObj.FindProperty("title");
        icon = serObj.FindProperty("icon");
        description = serObj.FindProperty("description");

        skillType = serObj.FindProperty("skillType");
        passiveType = serObj.FindProperty("passiveType");
        statType = serObj.FindProperty("statType");
        condition = serObj.FindProperty("condition");

        MP_cost = serObj.FindProperty("MP_cost");
        duration = serObj.FindProperty("duration");
        cooldown = serObj.FindProperty("cooldown");
        chance = serObj.FindProperty("chance");
        multiplier = serObj.FindProperty("multiplier");
        SPDmlp = serObj.FindProperty("SPDmlp");
        DMGmlp = serObj.FindProperty("DMGmlp");
        item = serObj.FindProperty("item");
        items = serObj.FindProperty("items");
        obj = serObj.FindProperty("obj");
        fx = serObj.FindProperty("fx");
        sound = serObj.FindProperty("sound");
        projPosition = serObj.FindProperty("projPosition");

    }

    public override void OnInspectorGUI() {
        serObj.Update();

        EditorGUILayout.LabelField("Skill used by character", EditorStyles.miniLabel);

        EditorGUILayout.PropertyField(title, new GUIContent("Skill name"));
        EditorGUILayout.PropertyField(icon, new GUIContent("Icon"));
        EditorGUILayout.PropertyField(description, new GUIContent("Description"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(skillType, new GUIContent("Skill Type"));

        //Fields for passive skills
        if (0 == skillType.intValue) {
            EditorGUILayout.PropertyField(passiveType, new GUIContent(" Type"));
            //Item Multiplier
            if (0 == passiveType.intValue) {
                EditorGUILayout.PropertyField(item, new GUIContent("  Item"));
                EditorGUILayout.PropertyField(multiplier, new GUIContent("  Multiplier"));
                chance.floatValue = EditorGUILayout.Slider("  Chance to success", chance.floatValue, 0.0f, 1.0f);
            }
            //Item Pickup
            if (1 == passiveType.intValue) {
                EditorGUILayout.PropertyField(item, new GUIContent("  Obstacle"));
                EditorGUILayout.PropertyField(items, new GUIContent("  Items"), true); //this is array
                EditorGUILayout.PropertyField(multiplier, new GUIContent("  Quantity"));
                chance.floatValue = EditorGUILayout.Slider("  Chance to success", chance.floatValue, 0.0f, 1.0f);
            }
            //Stat in conditions
            if (2 == passiveType.intValue) {
                EditorGUILayout.PropertyField(statType, new GUIContent("  Stat"));
                EditorGUILayout.PropertyField(multiplier, new GUIContent("  Multiplier"));
                EditorGUILayout.PropertyField(condition, new GUIContent("  In Condition"));
            }
        }

        //Fields for Dash skills.
        if (1 == skillType.intValue) {
            EditorGUILayout.PropertyField(duration, new GUIContent(" Duration"));
            EditorGUILayout.PropertyField(MP_cost, new GUIContent(" MP cost"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(SPDmlp, new GUIContent(" Speed multiplier"));
            EditorGUILayout.PropertyField(DMGmlp, new GUIContent(" Damage multiplier"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for Projectile skill
        if (3 == skillType.intValue) {
            EditorGUILayout.PropertyField(MP_cost, new GUIContent(" MP cost"));
            EditorGUILayout.PropertyField(duration, new GUIContent(" Delay"));
            EditorGUILayout.PropertyField(cooldown, new GUIContent(" Lifetime"));
            EditorGUILayout.PropertyField(obj, new GUIContent(" Projectile"));
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
            EditorGUILayout.PropertyField(projPosition, new GUIContent(" Position"));
        }
        //Fields for Changing Stats skill
        if (4 == skillType.intValue) {
            EditorGUILayout.PropertyField(duration, new GUIContent(" Duration"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(statType, new GUIContent(" Stat"));
            EditorGUILayout.PropertyField(multiplier, new GUIContent(" Value"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(MP_cost, new GUIContent(" MP cost"));
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for Restoration Stats skill
        if (5 == skillType.intValue) {
            EditorGUILayout.PropertyField(duration, new GUIContent(" Duration"));
            EditorGUILayout.PropertyField(cooldown, new GUIContent(" Cooldown"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(statType, new GUIContent(" Stat"));
            EditorGUILayout.PropertyField(multiplier, new GUIContent(" Value"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for Items Giving skill
        if (6 == skillType.intValue) {
            EditorGUILayout.PropertyField(cooldown, new GUIContent(" Cooldown"));
            EditorGUILayout.PropertyField(items, new GUIContent(" Items"), true); //this is array
            EditorGUILayout.PropertyField(multiplier, new GUIContent(" Quantity"));
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for Fly skill
        if (7 == skillType.intValue) {
            EditorGUILayout.PropertyField(MP_cost, new GUIContent(" MP cost"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(SPDmlp, new GUIContent(" Speed multiplier"));
            EditorGUILayout.PropertyField(DMGmlp, new GUIContent(" Damage multiplier"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for Slow Motion skill
        if (8 == skillType.intValue) {
            EditorGUILayout.PropertyField(duration, new GUIContent(" Duration"));
            EditorGUILayout.PropertyField(cooldown, new GUIContent(" Cooldown"));
            EditorGUILayout.PropertyField(multiplier, new GUIContent(" Value"));
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }
        //Fields for MP Protection skill
        if (9 == skillType.intValue) {
            EditorGUILayout.PropertyField(duration, new GUIContent(" Duration"));
            EditorGUILayout.PropertyField(cooldown, new GUIContent(" Cooldown"));
            EditorGUILayout.PropertyField(fx, new GUIContent(" FX object"));
            EditorGUILayout.PropertyField(sound, new GUIContent(" Sound"));
        }

        serObj.ApplyModifiedProperties();
    }
}
