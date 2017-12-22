using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MLA.System {
    public class DBSimulation : MonoBehaviour {

        [Header("Common")]
        [SerializeField]
        List<StorageData> _simulationItems = null;
        public CharsFMData simCharacter;
        public int sectionBonusLow = 0;
        public int sectionBonusHigh = 5;

        public EnduranceSection section;

        private static DBSimulation database;



        public static DBSimulation Instance {
            get {
                if (database == null) {
                    database = FindObjectOfType<DBSimulation>();
                }
                return database;
            }
        }

        #region Materials

        public void SetItemQuantity(int index, float quantity) {
            _simulationItems[index].ItemQuantity = quantity;
        }

        public void IncreaseItemQuantity(string itemName, float quantity) {
            for (int i = 0; i < _simulationItems.Count; i++) {
                if (_simulationItems[i].ItemName == itemName) {
                    _simulationItems[i].ItemQuantity += quantity;
                }
            }
        }
        public void IncreaseItemQuantity(int index, float quantity) {
            _simulationItems[index].ItemQuantity += quantity;
        }

        public float GetItemQuantity(string itemName) {
            for (int i = 0; i < _simulationItems.Count; i++) {
                if (_simulationItems[i].ItemName == itemName) {
                    return _simulationItems[i].ItemQuantity;
                }
            }
            return 0;
        }
        public float GetItemQuantity(int index) {
            return _simulationItems[index].ItemQuantity;
        }

        public Sprite GetItemIcon(string itemName) {
            for (int i = 0; i < _simulationItems.Count; i++) {
                if (_simulationItems[i].ItemName == itemName) {
                    return _simulationItems[i].ItemIcon;
                }
            }
            return _simulationItems[0].ItemIcon;
        }
        public string GetItemTitle(int index) {
            return _simulationItems[index].ItemName;
        }

        #endregion

        public int ArrayItemsGetLenght() {
            return _simulationItems.Count;
        }
    }
}
