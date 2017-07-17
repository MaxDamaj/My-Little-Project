using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Collections.Generic;

public enum ButtonState {
    Active, Locked, Fixed
};

[Serializable]
public class ButtonComponent {
    public Text text;
    public Image image;
	public GameObject fx;

    public Color outColor;
    public Color overColor;
    public Color downColor;
    public Color disableColor;
    public Color fixedColor;

    public Sprite outSprite;
    public Sprite overSprite;
    public Sprite downSprite;
    public Sprite disableSprite;
    public Sprite fixedSprite;
}
//---------------------------------------------
public class ButtonUniversal : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler {

    [Header("Statements")]
    public ButtonState state;

    [Header("UI Components")]
    public ButtonComponent[] _components;

    [Space()]
	public UnityEvent onClick;


    void Start () {
		if (onClick == null) onClick = new UnityEvent();
		ChangeButtonStates();
	}


    public void SetState(ButtonState value) {
		state = value;
        ChangeButtonStates();
    }

    void ChangeButtonStates() {
		if (state == ButtonState.Active) {
			foreach (ButtonComponent comp in _components) {
				if (comp.text != null) {
					comp.text.color = comp.outColor;
				}
				if (comp.image != null) {
					comp.image.color = comp.outColor;
					if (comp.outSprite != null) comp.image.sprite = comp.outSprite;
				}
				if (comp.fx != null) {
					comp.fx.SetActive(true);
				}
			}
		}
		if (state == ButtonState.Fixed) {
			foreach (ButtonComponent comp in _components) {
				if (comp.text != null) {
					comp.text.color = comp.fixedColor;
				}
				if (comp.image != null) {
					comp.image.color = comp.fixedColor;
					if (comp.fixedSprite != null) comp.image.sprite = comp.fixedSprite;
				}
				if (comp.fx != null) {
					comp.fx.SetActive(false);
				}
			}
		}
		if (state == ButtonState.Locked) {
			foreach (ButtonComponent comp in _components) {
				if (comp.text != null) {
					comp.text.color = comp.disableColor;
				}
				if (comp.image != null) {
					comp.image.color = comp.disableColor;
					if (comp.disableSprite != null) comp.image.sprite = comp.disableSprite;
				}
				if (comp.fx != null) {
					comp.fx.SetActive(false);
				}
			}
		}
    }

    //-----------------------------------
    public void OnPointerEnter(PointerEventData eventData) {
        if (state == ButtonState.Active) {
            foreach (ButtonComponent comp in _components) {
                if (comp.text != null) {
                    comp.text.color = comp.overColor;
                }
                if (comp.image != null) {
                    comp.image.color = comp.overColor;
                    if (comp.overSprite != null) comp.image.sprite = comp.overSprite;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left && state == ButtonState.Active) {
            foreach (ButtonComponent comp in _components) {
                if (comp.text != null) {
                    comp.text.color = comp.downColor;
                }
                if (comp.image != null) {
                    comp.image.color = comp.downColor;
                    if (comp.downSprite != null) comp.image.sprite = comp.downSprite;
                }
            }
			onClick.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (state == ButtonState.Active) {
            foreach (ButtonComponent comp in _components) {
                if (comp.text != null) {
                    comp.text.color = comp.outColor;
                }
                if (comp.image != null) {
                    comp.image.color = comp.outColor;
                    if (comp.outSprite != null) comp.image.sprite = comp.outSprite;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (state == ButtonState.Active) {
            foreach (ButtonComponent comp in _components) {
                if (comp.text != null) {
                    comp.text.color = comp.overColor;
                }
                if (comp.image != null) {
                    comp.image.color = comp.overColor;
                    if (comp.overSprite != null) comp.image.sprite = comp.overSprite;
                }
            }
        }
    }
	//---------------------------------------------
}
