using UnityEngine;
using System.Collections;

public class OnDestroyFX : MonoBehaviour {

	[SerializeField]
	private GameObject fx = null;
	[SerializeField]
	private float _objectDestroyDelay = 0;
	[SerializeField]
	private float _fxDestroyDelay = 3;

	void Start() {
		PonyController.onPlayerPickup += ExecuteFX;
	}

	void ExecuteFX(string tag, GameObject target) {
		if (tag == "Player" && gameObject==target) {
			Destroy(gameObject, _objectDestroyDelay);
		}
	}

	void OnDestroy() {
		PonyController.onPlayerPickup -= ExecuteFX;
		Destroy(Instantiate(fx, transform.position, fx.transform.rotation), _fxDestroyDelay);
	}
}
