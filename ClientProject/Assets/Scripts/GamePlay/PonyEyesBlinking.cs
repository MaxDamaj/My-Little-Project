using UnityEngine;
using System.Collections;

public class PonyEyesBlinking : MonoBehaviour {

	public Material eyeMaterial;
	public float blinkDelay;
	public float blinkRandom;
	public float blinkSmooth;

	// Use this for initialization
	void Start () {
		IEnumerator blinking = Blink(blinkDelay, blinkRandom, blinkSmooth);
		StartCoroutine(blinking);
	}
	
	IEnumerator Blink(float bD, float bR, float bS) {
		while (true) {
			eyeMaterial.mainTextureOffset = new Vector2(0, 0);		//Full open
			yield return new WaitForSeconds(Random.Range(bD-bR, bD+bR));
			eyeMaterial.mainTextureOffset = new Vector2(0.25f, 0);	//Half-closed
			yield return new WaitForSeconds(bS);
			eyeMaterial.mainTextureOffset = new Vector2(0.5f, 0);	//Full-closed
			yield return new WaitForSeconds(bS);
			eyeMaterial.mainTextureOffset = new Vector2(0.25f, 0);	//Half-closed
			yield return new WaitForSeconds(bS);
		}
	}

	void OnDestroy() {
		StopAllCoroutines();
	}
}
