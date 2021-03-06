﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.System;
using MLA.UI.Controllers;

public enum CodexActionType {
    none, nextList, showInfoTab, returnInGame
};

namespace MLA.UI.Windows {
    public class UICodexList : MonoBehaviour {

        public CodexTextWriter writer;
        public CodexList codexList;
        public CodexActionType action;
        public UICodexList nextCodexList;
        public Image unreadIcon;
        public int codexID;

        void Start() {
            gameObject.GetComponent<Button>().onClick.AddListener(ShowList);
            if (unreadIcon != null) {
                unreadIcon.gameObject.SetActive(Database.Instance.readenCodex[codexID] < 1);
            }
        }

        public void ShowList() {
            UICodexController.Instance.HideAllFields();
            if (unreadIcon != null) {
                unreadIcon.gameObject.SetActive(false);
            }
            writer.ShowText(codexList, action, nextCodexList, codexID);
        }
    }
}
