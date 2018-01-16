using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MLA.System;
using MLA.UI.Common;

namespace MLA.UI.Windows {
    public class UISimCharStats : MonoBehaviour {

        [Header("Up Panel")]
        public Text charHP;
        public Text charMP;
        public Text charSPD;

        [Header("Stat Set Panel")]
        public Text statSetHP;
        public Text statSetMP;
        public Text statSetSPD;

        public UIStatUpgrade[] UIStat;
        public Button[] upgButton;
        public Text[] upgText;

        private bool IsEnoughToHP, IsEnoughToMP;
        private string i1, i2;
        private int quan1, quan2;

        void Start() {
            IsEnoughToHP = false;
            IsEnoughToMP = false;
            upgButton[0].onClick.AddListener(UpgradeHealth);
            upgButton[1].onClick.AddListener(UpgradeMana);
            Refresh();
        }

        void Refresh() {
            CharsFMData character = DBSimulation.Instance.simCharacter;
            charHP.text = "" + character.HP;
            charMP.text = "" + character.MP;
            charSPD.text = "" + character.SPD;
            statSetHP.text = "" + character.HP;
            statSetMP.text = "" + character.MP;
            statSetSPD.text = "" + character.SPD;
            //Show Upgrade values
            upgText[0].text = "+1";
            upgText[1].text = "+1";
            UpgradeHealthCost();
            UpgradeManaCost();
        }


        #region Common

        //-----------Health-Upgrade----------------
        void UpgradeHealthCost() {
            var line = DBCharUpgrade.Instance.SimHPUpgrade;
            //Set Upgrade Cost
            i1 = line.res1;
            i2 = line.res2;
            quan1 = Mathf.FloorToInt(line.quan1 + (DBSimulation.Instance.simCharacter.HP - line.toValue));
            quan2 = Mathf.FloorToInt(line.quan2 + (DBSimulation.Instance.simCharacter.HP - line.toValue));
            //Set items values
            IsEnoughToHP = UIStat[0].UpgradeSimCost(i1, i2, quan1, quan2);
        }
        void UpgradeHealth() {
            var line = DBCharUpgrade.Instance.SimHPUpgrade;
            if (IsEnoughToHP) {
                DBSimulation.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (DBSimulation.Instance.simCharacter.HP - line.toValue)));
                DBSimulation.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (DBSimulation.Instance.simCharacter.HP - line.toValue)));
                DBSimulation.Instance.simCharacter.HP += 1f;
            } else {
                UIMessageWindow.Instance.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
            }
            Refresh();
        }
        //-----------Mana-Upgrade----------------
        void UpgradeManaCost() {
            var line = DBCharUpgrade.Instance.SimMPUpgrade;
            //Set Upgrade Cost
            i1 = line.res1;
            i2 = line.res2;
            quan1 = Mathf.FloorToInt(line.quan1 + (DBSimulation.Instance.simCharacter.MP - line.toValue));
            quan2 = Mathf.FloorToInt(line.quan2 + (DBSimulation.Instance.simCharacter.MP - line.toValue));
            //Set items values
            IsEnoughToMP = UIStat[1].UpgradeSimCost(i1, i2, quan1, quan2);
        }
        void UpgradeMana() {
            var line = DBCharUpgrade.Instance.SimMPUpgrade;
            if (IsEnoughToMP) {
                DBSimulation.Instance.IncreaseItemQuantity(line.res1, 0 - (line.quan1 + (DBSimulation.Instance.simCharacter.MP - line.toValue)));
                DBSimulation.Instance.IncreaseItemQuantity(line.res2, 0 - (line.quan2 + (DBSimulation.Instance.simCharacter.MP - line.toValue)));
                DBSimulation.Instance.simCharacter.MP += 1f;
            } else {
                UIMessageWindow.Instance.ShowMessage("You don't have enough resources", 0, UIAction.nothing, true, false);
            }
            Refresh();
        }

        #endregion
    }
}
