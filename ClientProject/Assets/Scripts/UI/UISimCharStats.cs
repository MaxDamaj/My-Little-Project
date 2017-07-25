using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISimCharStats : MonoBehaviour {

    [Header("Up Panel")]
    public Text charHP;
    public Text charMP;
    public Text charSPD;

    [Header("Stat Set Panel")]
    public Text statSetHP;
    public Text statSetMP;
    public Text statSetSPD;

    void Start() {
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
    }

    public void SetHP(int value) {
        DBSimulation.Instance.simCharacter.HP = value;
        Refresh();
    }
    public void SetMP(int value) {
        DBSimulation.Instance.simCharacter.MP = value;
        Refresh();
    }
    public void SetSPD(int value) {
        DBSimulation.Instance.simCharacter.SPD = value;
        Refresh();
    }

    public void IncreaseHP(int value) {
        if (DBSimulation.Instance.simCharacter.HP + value > 0 && DBSimulation.Instance.simCharacter.HP + value < 1000) {
            DBSimulation.Instance.simCharacter.HP += value;
        }
        Refresh();
    }
    public void IncreaseMP(int value) {
        if (DBSimulation.Instance.simCharacter.MP + value > 0 && DBSimulation.Instance.simCharacter.MP + value < 1000) {
            DBSimulation.Instance.simCharacter.MP += value;
        }
        Refresh();
    }
    public void IncreaseSPD(int value) {
        if (DBSimulation.Instance.simCharacter.SPD + value > 30 && DBSimulation.Instance.simCharacter.SPD + value < 100) {
            DBSimulation.Instance.simCharacter.SPD += value;
        }
        Refresh();
    }

}
