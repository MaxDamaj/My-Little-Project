using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapObjectType {
    Unknown = 0,
    City = 1,
    Town = 2,
    Mine = 3,
    Hideout = 4,
    Shush = 5,
    Fortress = 6,
    Meadow = 7,
    Ruine = 8
}

namespace MLA.UI.Windows {
    public class UIMapObject : MonoBehaviour {

        public MapObjectType objectType;

        private Button button;
        private RectTransform _playerMarker;
        private float visibleDistance = 3000f;

        void Start() {
            _playerMarker = GameObject.Find("player_marker").GetComponent<RectTransform>();
            button = GetComponent<Button>();

            Vector2 position = GetComponent<RectTransform>().anchoredPosition;
            float sqrLen = (position - _playerMarker.anchoredPosition).sqrMagnitude;
            if (sqrLen > visibleDistance) {
                button.interactable = false;
            }
        }
    }
}
