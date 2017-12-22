using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct SpinValue {
    public string spinIcon;
    public int spinNumber;
}

namespace MLA.UI.Windows {
    public class UISlotSpinning : MonoBehaviour {

        public List<Image> icons;
        public Text digit;
        public float speed = 2;
        public Animator animIcon;
        public Animator animDigit;

        private bool isSpinning;
        private bool trigger;

        private string forceStopIcon = "";
        private int forceStopDigit = -1;

        private SpinValue spinValue;

        #region API

        void Start() {
            digit.text = "" + Random.Range(0, 10);
        }

        void FixedUpdate() {
            if (isSpinning) {
                //digit spinning
                digit.text = "" + (int.Parse(digit.text) + 1);
                if (digit.text == "10") { digit.text = "0"; }
                //Icon spinning
                foreach (var icon in icons) {
                    icon.rectTransform.anchoredPosition = new Vector2(0, icon.rectTransform.anchoredPosition.y - speed);
                    if (icon.rectTransform.anchoredPosition.y <= -50) {
                        icon.rectTransform.anchoredPosition = new Vector2(0, icon.rectTransform.anchoredPosition.y + 300);
                        CheckStopState();
                    }
                }
            }
        }

        #endregion

        void CheckStopState() {
            if (trigger) {
                if (forceStopIcon != "") {
                    if (forceStopIcon == GetSpinValue().spinIcon) {
                        trigger = false; isSpinning = false;
                    }
                } else {
                    trigger = false; isSpinning = false;
                }

                if (forceStopDigit != -1) {
                    digit.text = forceStopDigit.ToString();
                    forceStopDigit = -1;
                }
            }
        }

        public void StopSpinning() {
            trigger = true;
        }
        public void StopSpinning(string Icon) {
            trigger = true;
            forceStopIcon = Icon;
        }
        public void StopSpinning(int Digit) {
            trigger = true;
            forceStopDigit = Digit;
        }
        public void StopSpinning(string Icon, int Digit) {
            trigger = true;
            forceStopIcon = Icon;
            forceStopDigit = Digit;
        }

        public void StartSpinning() {
            isSpinning = true;
            trigger = false;
            speed = Random.Range(2f, 5f);
        }

        public SpinValue GetSpinValue() {
            spinValue.spinIcon = icons.Find(x => x.rectTransform.localPosition.y < 10 && x.rectTransform.localPosition.y > -10).name;
            spinValue.spinNumber = int.Parse(digit.text);
            return spinValue;
        }
        public bool IsSlotSpinning() {
            return isSpinning;
        }
    }
}
