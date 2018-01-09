using UnityEngine;
using System.Collections;
using MLA.Gameplay.Controllers;
using MLA.System.Controllers;
using MLA.System;
using MLA.UI.Windows;

namespace MLA.Gameplay.Common {
    public class OnDestroyFX : MonoBehaviour {

        [SerializeField]
        private GameObject fx = null;
        [SerializeField]
        private float _objectDestroyDelay = 0;
        [SerializeField]
        private float _fxDestroyDelay = 3;
        [SerializeField]
        private float _addValue = 1;

        public string objectType = "Bits";
        public string sound = "a_beeps";
        public bool isShowPopup = false;
        public bool isSimulationPickup = false;

        void Start() {
            PonyController.onPlayerPickup += ExecuteFX;
            PonyFreeMoveController.onPlayerPickup += ExecuteFX;
        }

        void ExecuteFX(string tag, GameObject target) {
            if (tag == "Player" && gameObject == target) {
                SoundManager.Instance.PlaySound(sound);
                if (!isSimulationPickup) {
                    Database.Instance.IncreaseItemQuantity(objectType, _addValue * GlobalData.Instance.PickupMlp);
                } else {
                    DBSimulation.Instance.IncreaseItemQuantity(objectType, _addValue * GlobalData.Instance.PickupMlp);
                }
                if (isShowPopup) { PickupPopup.Instance.ShowPopupInfo(objectType, isSimulationPickup); }
                Destroy(Instantiate(fx, transform.position, fx.transform.rotation), _fxDestroyDelay);
                Destroy(gameObject, _objectDestroyDelay);
            }
        }

        void OnDestroy() {
            PonyController.onPlayerPickup -= ExecuteFX;
            PonyFreeMoveController.onPlayerPickup -= ExecuteFX;
        }
    }
}
