using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.System;
using MLA.UI.Controllers;

namespace MLA.UI.Windows {
    public class UIPartySwitcher : MonoBehaviour {

        [Header("UI")]
        public Image[] partyIcons;
        public Text[] partyNames;
        public Button[] partyButtons;
        public Animator win_anim;

        [Header("Common")]
        public MenuNavigation navi;
        public Sprite emptySprite;

        void Start() {
            Refresh();
            Database.onRefresh += Refresh;
        }

        void Refresh() {
            for (int i = 0; i < partyIcons.GetLength(0); i++) {
                if (Database.Instance.PartyPony[i] >= 0) {
                    CharsFMData pony = Database.Instance.GetCharFMInfo(Database.Instance.PartyPony[i]);
                    partyIcons[i].sprite = pony.CharIcon;
                    partyNames[i].text = pony.CharName;
                    partyNames[i].color = pony.CharColor;
                    partyButtons[i].interactable = true;
                } else {
                    partyIcons[i].sprite = emptySprite;
                    partyNames[i].text = "-empty-";
                    partyNames[i].color = Color.white;
                    partyButtons[i].interactable = false;
                }
            }
        }

        public void ChangeCharacter(int type) {
            CharsFMData pony = Database.Instance.GetCharFMInfo(Database.Instance.PartyPony[type]);
            if (Database.Instance.SelectedPony == Database.Instance.PartyPony[type]) {
                if (win_anim.GetBool("trigger")) {
                    MenuNavigation.Instance.HideLeftPanels();
                } else {
                    MenuNavigation.Instance.HideLeftPanels();
                    win_anim.SetBool("trigger", true);
                }
            }
            navi.RefreshPreviewMesh(pony);
            Database.Instance.SelectedPony = Database.Instance.PartyPony[type];
            win_anim.GetComponent<CharFMInfoFull>().RefreshUI();
            Refresh();
        }

        public static UIPartySwitcher Instance {
            get {
                return FindObjectOfType<UIPartySwitcher>();
            }
        }

        void OnDestroy() {
            Database.onRefresh -= Refresh;
        }
    }
}
