using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
