using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.System;
using MLA.UI.Controllers;

namespace MLA.UI.Windows {
    public class CodexTextWriter : MonoBehaviour {

        public Text header;
        public Text text;
        public Button moveToButton;
        public Transform container;

        private CodexActionType action;
        private CodexList codexList;
        private UICodexList nextCodexList;

        void Start() {
            header.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            moveToButton.gameObject.SetActive(false);
        }

        void OnEnable() {
            if (codexList != null) {
                WriteText();
            }
        }
        void OnDisable() {
            for (int i = 0; i < container.childCount; i++) {
                Destroy(container.GetChild(i).gameObject);
            }
            StopAllCoroutines();
        }

        void ListAction() {
            switch (action) {
                case CodexActionType.nextList:
                    nextCodexList.ShowList();
                    break;
                case CodexActionType.showInfoTab:
                    UICodexController.Instance.SwitchField(3);
                    break;
            }
        }

        public void ShowText(CodexList list, CodexActionType act, UICodexList nextList, int index) {
            OnDisable();
            codexList = list;
            action = act;
            nextCodexList = nextList;
            Database.Instance.readenCodex[index] = 1;
            if (gameObject.activeSelf) {
                OnEnable();
            } else {
                gameObject.SetActive(true);
            }
        }

        void WriteText() {
            for (int i = 0; i < codexList.codexRows.Count; i++) {
                //Print new row
                GameObject tmp = Instantiate(header.gameObject); //header
                tmp.GetComponent<Text>().text = codexList.codexRows[i].header;
                tmp.GetComponent<Text>().color = codexList.codexRows[i].headerColor;
                tmp.transform.SetParent(container);
                tmp.transform.localScale = Vector3.one;
                tmp.SetActive(true);
                tmp = Instantiate(text.gameObject); //rows
                tmp.transform.SetParent(container);
                tmp.transform.localScale = Vector3.one;
                tmp.SetActive(true);
                Text newText = tmp.GetComponent<Text>();
                newText.text = "";
                for (int j = 0; j < codexList.codexRows[i].text.Length; j++) {
                    newText.text = string.Concat(newText.text, codexList.codexRows[i].text.Substring(j, 1));
                }
            }
            if (action != CodexActionType.none) {
                GameObject tmp = Instantiate(moveToButton.gameObject); //button
                tmp.transform.SetParent(container);
                tmp.transform.localScale = Vector3.one;
                tmp.SetActive(true);
                tmp.GetComponent<Button>().onClick.AddListener(ListAction);
            }
        }
    }
}
