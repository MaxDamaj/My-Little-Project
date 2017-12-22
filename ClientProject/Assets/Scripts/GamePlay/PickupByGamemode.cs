using MLA.Gameplay.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLA.Gameplay.Common {
    public class PickupByGamemode : MonoBehaviour {

        public GameObject pickupInFreemode;
        public GameObject pickupInSimulation;

        void Start() {
            Destroy(transform.GetChild(0).gameObject);

            GameObject tmp = null;
            if (SkillController.Instance == null) return;
            if (!SkillController.Instance.IsSimulation) {
                tmp = Instantiate(pickupInFreemode, transform);
            } else {
                tmp = Instantiate(pickupInSimulation, transform);
            }

            tmp.transform.localPosition = Vector3.zero;
        }

    }
}
