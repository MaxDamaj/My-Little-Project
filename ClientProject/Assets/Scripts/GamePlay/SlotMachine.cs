using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour {

    public UISlotSpinning[] slots;
    public Vector2 firstDelay;
    public Vector2 secondDelay;
    public Text rewardText;

    private List<SpinValue> results;

    void Start() {
        rewardText.text = "";
        foreach (var slot in slots) {
            slot.StartSpinning();
        }
        IEnumerator action = Action();
        StartCoroutine(action);

    }

    void CheckStates() {
        string iconValue = "";
        int digitValue = -1;
        //Icons
        if (results[0].spinIcon == results[1].spinIcon) {
            slots[0].animIcon.SetTrigger("flash");
            slots[1].animIcon.SetTrigger("flash");
            iconValue = results[0].spinIcon;
        }
        if (results[1].spinIcon == results[2].spinIcon) {
            slots[1].animIcon.SetTrigger("flash");
            slots[2].animIcon.SetTrigger("flash");
            iconValue = results[1].spinIcon;
        }
        if (results[0].spinIcon == results[2].spinIcon) {
            slots[0].animIcon.SetTrigger("flash");
            slots[2].animIcon.SetTrigger("flash");
            iconValue = results[0].spinIcon;
        }
        //Digits
        if (results[0].spinNumber == results[1].spinNumber) {
            slots[0].animDigit.SetTrigger("flash");
            slots[1].animDigit.SetTrigger("flash");
            digitValue = results[0].spinNumber;
        }
        if (results[1].spinNumber == results[2].spinNumber) {
            slots[1].animDigit.SetTrigger("flash");
            slots[2].animDigit.SetTrigger("flash");
            digitValue = results[1].spinNumber;
        }
        if (results[0].spinNumber == results[2].spinNumber) {
            slots[0].animDigit.SetTrigger("flash");
            slots[2].animDigit.SetTrigger("flash");
            digitValue = results[0].spinNumber;
        }
        GainReward(iconValue, digitValue);
    }


    IEnumerator Action() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(firstDelay.x, firstDelay.y));
            slots[0].StopSpinning();
            yield return new WaitForSeconds(Random.Range(secondDelay.x, secondDelay.y));
            slots[2].StopSpinning();
            yield return new WaitForSeconds(1f);
            //GetResults
            slots[1].StopSpinning();
            yield return new WaitForSeconds(0.5f);
            results = new List<SpinValue>();
            results.Add(slots[0].GetSpinValue());
            results.Add(slots[1].GetSpinValue());
            results.Add(slots[2].GetSpinValue());
            CheckStates();

            yield return new WaitForSeconds(2f);
            rewardText.text = "";
            foreach (var slot in slots) {
                slot.StartSpinning();
            }
        }
    }

    void GainReward(string iconType, int digitValue) {
        int iconCount = results.FindAll(x => x.spinIcon == iconType).Count;
        int digitCount = results.FindAll(x => x.spinNumber == digitValue).Count;
        IEnumerator paramReturn;
        float value;
        switch (iconType) {
            case "Laughter":
                value = iconCount * digitCount;
                rewardText.text = "Restore " + value + " mana";
                GlobalData.Instance.currentMP += value;
                break;
            case "Generosity":
                value = (iconCount * digitCount) * 0.5f;
                rewardText.text = "Gain on " + value + " more items";
                GlobalData.Instance.PickupMlp *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Honesty":
                value = 1 - ((iconCount * digitCount) * 0.1f);
                rewardText.text = "Decrease damage on " + value + "%";
                GlobalData.Instance.DMGmlp *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Kindness":
                value = 1 + ((iconCount * digitCount) * 0.1f);
                rewardText.text = "Increase mana restoration at " + (iconCount * digitCount) * 0.5f + "%";
                GlobalData.Instance.currentMP_rec *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Loyalty":
                value = (iconCount * digitCount) * 20f;
                rewardText.text = "Restore " + value + " stamina";
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, value);
                break;
            case "Magic":
                value = (iconCount * digitCount) * 0.5f;
                rewardText.text = "Restore " + value + " health";
                GlobalData.Instance.currentHP += value;
                break;
        }
    }

    IEnumerator SwitchParameter(string type, float value) {
        yield return new WaitForSeconds(6f);
        switch(type) {
            case "Generosity":
                GlobalData.Instance.PickupMlp /= value;
                break;
            case "Honesty":
                GlobalData.Instance.DMGmlp /= value;
                break;
            case "Kindness":
                GlobalData.Instance.currentMP_rec /= value;
                break;
        }
    }


}
