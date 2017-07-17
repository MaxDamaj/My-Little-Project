using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPSCalculation : MonoBehaviour {

    public static int PartyStrength = 0;

    public Text psText;
	public UIPartySwitcher PartySwitcher;

    //Party Strength - total strenght your current selected ponies
	//PS = earth(HP+MP) + unicorn(HP+MP) + pegasus(HP+MP)

    void Start() {
        Database.onRefresh += RefreshUI;
        RefreshUI();
    }

    void RefreshUI() {
        float ponySTR;
        CharsFMData pony;
        PartyStrength = 0;
		for (int i = 0; i < PartySwitcher.partyNames.GetLength(0); i++) {
			if (PartySwitcher.partyNames[i].text != "-empty-") {
				pony = Database.Instance.GetCharFMInfo(PartySwitcher.partyNames[i].text);
				ponySTR = (pony.HP + pony.MP);
				PartyStrength += Mathf.FloorToInt(ponySTR);
			}
        }
        psText.text = "PS: " + PartyStrength;
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }
}
