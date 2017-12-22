using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.UI.Windows;
using MLA.System.Controllers;
using MLA.System;

namespace MLA.UI.Controllers {
    public class UICodexController : MonoBehaviour {

        [Header("UI")]
        public Button[] buttons;
        public Text[] buttonTexts;
        public GameObject[] codexFields;
        public Button buttonBack;
        public CodexTextWriter writer;
        public GameObject helpTip;

        [Header("Common")]
        public UICodexList firstText;
        public Color selectColor;

        private UICodexList[] codexLists;

        void Start() {
            MusicManager.Instance.SetFolder("Music/Codex", 0);
            buttonBack.onClick.AddListener(delegate {
                UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
            });
            helpTip.SetActive(false);
            HideAllFields();
            //Show in first time
            if (Database.Instance.tutorialState <= 1) {
                writer.ShowText(firstText.codexList, firstText.action, firstText.nextCodexList, firstText.codexID);
                helpTip.SetActive(true);
                Database.Instance.tutorialState++;
            }
        }

        public void SwitchField(int index) {
            foreach (var text in buttonTexts) {
                text.color = Color.white;
            }
            foreach (var field in codexFields) {
                field.SetActive(false);
            }
            writer.gameObject.SetActive(false);
            buttonTexts[index].color = selectColor;
            codexFields[index].SetActive(true);
        }

        public void HideAllFields() {
            foreach (var field in codexFields) {
                field.SetActive(false);
            }
        }

        void CheckCodexStates() {
            codexLists = FindObjectsOfType<UICodexList>();
            //Check active
            foreach (var list in codexLists) {
                int value = Database.Instance.readenCodex[list.codexID];
                switch (list.codexList.condition) {
                    case AchieveType.TotalDist:
                        if (Database.Instance.distTotal < list.codexList.condValue)
                            value = -1;
                        break;
                    case AchieveType.CharsOwned:
                        if (Database.Instance.GetUnlockedCharsCount() < list.codexList.condValue)
                            value = -1;
                        break;
                    case AchieveType.ComponentsCount:
                        if (Database.Instance.GetItemQuantity(list.codexList.condLine) < list.codexList.condValue)
                            value = -1;
                        break;
                    case AchieveType.TargetCharOwned:
                        if (Database.Instance.GetCharFMRank(list.codexList.condLine) == -1)
                            value = -1;
                        break;
                    case AchieveType.CraftedComps:
                        if (Database.Instance.craftedComps < list.codexList.condValue)
                            value = -1;
                        break;
                }
                //Set value
                Database.Instance.readenCodex[list.codexID] = value;
                list.gameObject.SetActive(value != -1);
            }
        }

        //---------------------
        public static UICodexController Instance {
            get {
                return FindObjectOfType<UICodexController>();
            }
        }
    }
}
