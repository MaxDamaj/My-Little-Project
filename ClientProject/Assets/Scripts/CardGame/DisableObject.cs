using UnityEngine;
using System.Collections;

public class DisableObject : MonoBehaviour {

	public float delay;

	// Use this for initialization
	void OnEnable() {
		IEnumerator disableDelay = DisableObj(delay);
		StartCoroutine(disableDelay);
	}	
	
	IEnumerator DisableObj(float time) {
		yield return new WaitForSeconds(time);
		gameObject.SetActive(false);
	}

	void OnDisable() {
		StopAllCoroutines();
	}
}
