using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MLA.System;

namespace MLA.UI.Common {
    public class UIStatUpgrade : MonoBehaviour {

        public Image[] itemIcon;
        public Text[] itemText;

        void Start() {

        }

        public bool UpgradeCost(string i1, string i2, string i3, float quan1, float quan2, float quan3) {
            bool IsEnoughItems = true;
            //Show Upgrade Cost on screen
            itemIcon[0].sprite = Database.Instance.GetItemIcon(i1); itemIcon[1].sprite = Database.Instance.GetItemIcon(i2); itemIcon[2].sprite = Database.Instance.GetItemIcon(i3);
            itemText[0].text = "" + quan1; itemText[1].text = "" + quan2; itemText[2].text = "" + quan3;
            //Check items in storage
            if (quan1 <= Database.Instance.GetItemQuantity(i1)) {
                itemText[0].color = Database.COLOR_GREEN;
            } else {
                itemText[0].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            //---
            if (quan2 <= Database.Instance.GetItemQuantity(i2)) {
                itemText[1].color = Database.COLOR_GREEN;
            } else {
                itemText[1].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            //---
            if (quan3 <= Database.Instance.GetItemQuantity(i3)) {
                itemText[2].color = Database.COLOR_GREEN;
            } else {
                itemText[2].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            return IsEnoughItems;
        }

        public bool UpgradeCost(string i1, string i2, float quan1, float quan2) {
            bool IsEnoughItems = true;
            //Show Upgrade Cost on screen
            itemIcon[0].sprite = Database.Instance.GetItemIcon(i1); itemIcon[1].sprite = Database.Instance.GetItemIcon(i2);
            itemText[0].text = "" + quan1; itemText[1].text = "" + quan2;
            //Check items in storage
            if (quan1 <= Database.Instance.GetItemQuantity(i1)) {
                itemText[0].color = Database.COLOR_GREEN;
            } else {
                itemText[0].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            //---
            if (quan2 <= Database.Instance.GetItemQuantity(i2)) {
                itemText[1].color = Database.COLOR_GREEN;
            } else {
                itemText[1].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            return IsEnoughItems;
        }

        public bool UpgradeSimCost(string i1, string i2, float quan1, float quan2) {
            bool IsEnoughItems = true;
            //Show Upgrade Cost on screen
            itemIcon[0].sprite = DBSimulation.Instance.GetItemIcon(i1); itemIcon[1].sprite = DBSimulation.Instance.GetItemIcon(i2);
            itemText[0].text = "" + quan1; itemText[1].text = "" + quan2;
            //Check items in storage
            if (quan1 <= DBSimulation.Instance.GetItemQuantity(i1)) {
                itemText[0].color = Database.COLOR_GREEN;
            } else {
                itemText[0].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            //---
            if (quan2 <= DBSimulation.Instance.GetItemQuantity(i2)) {
                itemText[1].color = Database.COLOR_GREEN;
            } else {
                itemText[1].color = Database.COLOR_RED; IsEnoughItems = false;
            }
            return IsEnoughItems;
        }
    }
}
