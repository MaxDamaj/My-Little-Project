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
    private CharsFMData character;

    void Start() {
        character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        rewardText.text = "";
        if (character.LUCK > 0) {
            foreach (var slot in slots) {
                slot.StartSpinning();
            }
            IEnumerator action = Action();
            StartCoroutine(action);
        }
    }

    IEnumerator Action() {
        while (true) {
            string expectedIcon = "";
            int expectedDigit = -1;

            //First wheel
            yield return new WaitForSeconds(Random.Range(firstDelay.x, firstDelay.y));
            slots[0].StopSpinning();

            //Second wheel
            yield return new WaitForSeconds(Random.Range(secondDelay.x, secondDelay.y));
            expectedIcon = slots[0].GetSpinValue().spinIcon;
            expectedDigit = slots[0].GetSpinValue().spinNumber;
            if (character.LUCK > 50) slots[2].StopSpinning(expectedIcon, expectedDigit);
            if (character.LUCK > 30) slots[2].StopSpinning(expectedIcon);
            slots[2].StopSpinning();

            //Third wheel
            yield return new WaitUntil(() => !slots[2].IsSlotSpinning());
            yield return new WaitForSeconds(1f);
            if (character.LUCK > 90) slots[1].StopSpinning(expectedIcon, expectedDigit);
            if (character.LUCK > 75) slots[1].StopSpinning(expectedIcon);
            slots[1].StopSpinning();

            //GetResults
            yield return new WaitUntil(() => !slots[1].IsSlotSpinning());
            results = new List<SpinValue>();
            results.Add(slots[0].GetSpinValue());
            results.Add(slots[1].GetSpinValue());
            results.Add(slots[2].GetSpinValue());
            CheckStates();

            yield return new WaitForSeconds(2f);
            rewardText.text = "";
            Database.Instance.SetCharFMLuck(Database.Instance.SelectedPony, character.LUCK - 1);
            if (character.LUCK <= 0) break;
            foreach (var slot in slots) {
                slot.StartSpinning();
            }
        }
    }

    IEnumerator SwitchParameter(string type, float value) {
        yield return new WaitForSeconds(6f);
        switch (type) {
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

    #region States

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

    void GainReward(string iconType, int digitValue) {
        int iconCount = results.FindAll(x => x.spinIcon == iconType).Count;
        int digitCount = results.FindAll(x => x.spinNumber == digitValue).Count;
        IEnumerator paramReturn;
        float value;
        switch (iconType) {
            case "Laughter":
                value = iconCount * (digitCount + 1);
                rewardText.text = "Restore " + value + " mana";
                GlobalData.Instance.currentMP += value;
                break;
            case "Generosity":
                value = 1 + (iconCount * (digitCount + 1)) * 0.5f;
                rewardText.text = "Gain on " + value + " more items";
                GlobalData.Instance.PickupMlp *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Honesty":
                value = 1 - ((iconCount * (digitCount + 1)) * 0.1f);
                rewardText.text = "Decrease damage on " + (100f - value * 100f).ToString() + "%";
                GlobalData.Instance.DMGmlp *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Kindness":
                value = 1 + ((iconCount * (digitCount + 1)) * 0.1f);
                rewardText.text = "Increase mana restoration at " + (value * 100f - 100f).ToString() + "%";
                GlobalData.Instance.currentMP_rec *= value;
                paramReturn = SwitchParameter(iconType, value);
                StartCoroutine(paramReturn);
                break;
            case "Loyalty":
                value = (iconCount * (digitCount + 1)) * 20f;
                rewardText.text = "Restore " + value + " stamina";
                Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, value);
                break;
            case "Magic":
                value = (iconCount * (digitCount + 1)) * 0.5f;
                rewardText.text = "Restore " + value + " health";
                GlobalData.Instance.currentHP += value;
                break;
            case "":
                if (digitCount > 0) {
                    switch (digitValue) {
                        case 0:
                            value = (digitCount + 1) * 10f;
                            rewardText.text = "Gain " + value + " bits";
                            Database.Instance.IncreaseItemQuantity("Bits", value);
                            break;
                        case 1:
                        case 2:
                        case 3:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Laughter";
                            Database.Instance.IncreaseItemQuantity("Laughter", value);
                            break;
                        case 4:
                        case 5:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Generosity";
                            Database.Instance.IncreaseItemQuantity("Generosity", value);
                            break;
                        case 6:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Honesty";
                            Database.Instance.IncreaseItemQuantity("Honesty", value);
                            break;
                        case 7:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Kindness";
                            Database.Instance.IncreaseItemQuantity("Kindness", value);
                            break;
                        case 8:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Loyalty";
                            Database.Instance.IncreaseItemQuantity("Loyalty", value);
                            break;
                        case 9:
                            value = (digitCount + 1);
                            rewardText.text = "Gain " + value + " Magic";
                            Database.Instance.IncreaseItemQuantity("Magic", value);
                            break;
                    }
                }
                break;
        }
    }

    #endregion

}
