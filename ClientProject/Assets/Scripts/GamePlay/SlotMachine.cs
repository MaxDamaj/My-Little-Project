using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour {

    public UISlotSpinning[] slots;
    public Vector2 firstDelay;
    public Vector2 secondDelay;

    private List<SpinValue> results;

    void Start() {
        foreach (var slot in slots) {
            slot.StartSpinning();
        }
        IEnumerator action = Action();
        StartCoroutine(action);
    
    }

    void CheckStates() {
        //Icons
        if (results[0].spinIcon == results[1].spinIcon) {
            slots[0].animIcon.SetTrigger("flash");
            slots[1].animIcon.SetTrigger("flash");
        }
        if (results[1].spinIcon == results[2].spinIcon) {
            slots[1].animIcon.SetTrigger("flash");
            slots[2].animIcon.SetTrigger("flash");
        }
        if (results[0].spinIcon == results[2].spinIcon) {
            slots[0].animIcon.SetTrigger("flash");
            slots[2].animIcon.SetTrigger("flash");
        }
        //Digits
        if (results[0].spinNumber == results[1].spinNumber) {
            slots[0].animDigit.SetTrigger("flash");
            slots[1].animDigit.SetTrigger("flash");
        }
        if (results[1].spinNumber == results[2].spinNumber) {
            slots[1].animDigit.SetTrigger("flash");
            slots[2].animDigit.SetTrigger("flash");
        }
        if (results[0].spinNumber == results[2].spinNumber) {
            slots[0].animDigit.SetTrigger("flash");
            slots[2].animDigit.SetTrigger("flash");
        }
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
            foreach (var slot in slots) {
                slot.StartSpinning();
            }
        }
    }


}
