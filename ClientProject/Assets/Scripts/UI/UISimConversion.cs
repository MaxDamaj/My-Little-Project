using MLA.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MLA.UI.Windows {
    public class UISimConversion : MonoBehaviour {

        public Text pixelText;
        public Text fluxText;
        public GameObject lineSample;
        public RectTransform container;

        private List<ConvertLine> lines;

        void Start() {
            lines = new List<ConvertLine>();
            for (int i=0; i<DBSimulation.Instance.materialConversion.Count; i++) {
                lines.Add(new ConvertLine(lineSample, container, DBSimulation.Instance.materialConversion[i]));
            }
            lineSample.SetActive(false);
            Database.onRefresh += Refresh;
            Refresh();
        }

        public void Refresh() {
            pixelText.text = DBSimulation.Instance.GetItemQuantity(0).ToString();
            fluxText.text = DBSimulation.Instance.GetItemQuantity(1).ToString();
            foreach (var line in lines) {
                line.Refresh();
            }
        }

        void OnDestroy() {
            Database.onRefresh -= Refresh;
        }


        #region SubLine
        class ConvertLine {
            public Image iconFrom1 { get; private set; }
            public Image iconFrom2 { get; private set; }
            public Image iconTo { get; private set; }
            public Text textFrom1 { get; private set; }
            public Text textFrom2 { get; private set; }
            public Text textTo { get; private set; }
            public Button convertButton { get; private set; }

            private DBCharUpgrade.CharUpgradeLine materialsLine;

            public ConvertLine(GameObject sample, Transform parent, DBCharUpgrade.CharUpgradeLine statLine) {
                GameObject instance = Instantiate(sample, parent);
                instance.transform.localScale = Vector3.one;
                materialsLine = statLine;

                //Images
                List<Image> images = new List<Image>(); images.AddRange(instance.GetComponentsInChildren<Image>());
                iconFrom1 = images.Find(x => x.name == "IconFrom1");
                iconFrom1.sprite = DBSimulation.Instance.GetItemIcon(statLine.res1);
                iconFrom2 = images.Find(x => x.name == "IconFrom2");
                if (statLine.res2 == "") {
                    iconFrom2.gameObject.SetActive(false);
                } else {
                    iconFrom2.sprite = DBSimulation.Instance.GetItemIcon(statLine.res2);
                }
                iconTo = images.Find(x => x.name == "IconTo");
                iconTo.sprite = Database.Instance.GetItemIcon(statLine.res3);

                //Texts
                List<Text> texts = new List<Text>(); texts.AddRange(instance.GetComponentsInChildren<Text>());
                textFrom1 = texts.Find(x => x.name == "TextFrom1");
                textFrom1.text = statLine.quan1.ToString();
                textFrom2 = texts.Find(x => x.name == "TextFrom2");
                if (statLine.res2 == "") {
                    textFrom2.gameObject.SetActive(false);
                } else {
                    textFrom2.text = statLine.quan2.ToString();
                }
                textTo = texts.Find(x => x.name == "TextTo");
                textTo.text = statLine.quan3.ToString();

                //Button
                convertButton = instance.GetComponentInChildren<Button>();
                convertButton.onClick.AddListener(ConvertMaterials);
            }

            public void Refresh() {
                convertButton.interactable = true;
                textFrom1.color = Database.COLOR_GREEN;
                textFrom2.color = Database.COLOR_GREEN;
                if (DBSimulation.Instance.GetItemQuantity(materialsLine.res1) < materialsLine.quan1) {
                    convertButton.interactable = false;
                    textFrom1.color = Database.COLOR_RED;
                }
                if (materialsLine.res2 != "" && DBSimulation.Instance.GetItemQuantity(materialsLine.res2) < materialsLine.quan2) {
                    convertButton.interactable = false;
                    textFrom2.color = Database.COLOR_RED;
                }
            }

            void ConvertMaterials() {
                DBSimulation.Instance.IncreaseItemQuantity(materialsLine.res1, -materialsLine.quan1);
                if (materialsLine.res2 != "") { DBSimulation.Instance.IncreaseItemQuantity(materialsLine.res2, -materialsLine.quan2); }
                Database.Instance.IncreaseItemQuantity(materialsLine.res3, materialsLine.quan3);
            }

        }
        #endregion

    }
}
