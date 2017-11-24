using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour {

    public UISlotSpinning[] slots;
    public Vector2 firstDelay;
    public Vector2 secondDelay;

    void Start() {

        foreach (var slot in slots) {
            slot.StartSpinning();
        }
        IEnumerator action = Action();
        StartCoroutine(action);

    }

    IEnumerator Action() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(firstDelay.x, firstDelay.y));
            slots[0].StopSpinning();
            yield return new WaitForSeconds(Random.Range(secondDelay.x, secondDelay.y));
            slots[2].StopSpinning();
            yield return new WaitForSeconds(1f);
            slots[1].StopSpinning();
            yield return new WaitForSeconds(2f);
            foreach (var slot in slots) {
                slot.StartSpinning();
            }
        }
    }


}
