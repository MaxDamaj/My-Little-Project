using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharFMSkills : MonoBehaviour {

    public Text[] SkillTitle;
    public Text[] SkillDesc;

    private CharsFMData Character;

    // Use this for initialization
    void Start () {
        RefreshUI();
    }

    // Update is called once per frame
    public void RefreshUI() {
        Character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        for (int i=0; i<SkillTitle.GetLength(0); i++) {
            SkillTitle[i].text = Character.CharSkills[i].title;
            SkillDesc[i].text = Character.CharSkills[i].description;
        }
    }
}
