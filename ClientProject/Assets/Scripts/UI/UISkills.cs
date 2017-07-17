using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UISkills : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    [SerializeField]
    private Color _colorGrey = Color.grey;
    [SerializeField]
    private Color _colorWhite = Color.white;
    [SerializeField]
    private Color[] _skillsColors = null;

    public Image[] SkillIcons;
    public Image[] Frames;
    public Image[] Masks;
    public Text[] CoolTexts;

    private bool IsPressed;

    #region API

    void Start() {

    }

    void FixedUpdate() {
        if (IsPressed) {
            SkillController.Instance.SkillXHold();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!IsPressed) {
            if (eventData.pointerEnter == Frames[0].gameObject) { PonyController.Instance.Jump(); }
            if (eventData.pointerEnter == Frames[1].gameObject) { SkillController.Instance.SkillXDown(); }
            if (eventData.pointerEnter == Frames[2].gameObject) { SkillController.Instance.SkillYDown(); }
            if (eventData.pointerEnter == Frames[3].gameObject) { SkillController.Instance.SkillBDown(); }
        }
        IsPressed = true;
    }
    public void OnPointerExit(PointerEventData eventData) {
        IsPressed = false;
        if (eventData.pointerEnter == Frames[1].gameObject) {
            SkillController.Instance.SkillXUp();
        }
    }
    public void OnPointerUp(PointerEventData eventData) {
        IsPressed = false;
        if (eventData.pointerEnter == Frames[1].gameObject) {
            SkillController.Instance.SkillXUp();
        }
    }

    #endregion

    public void DeactivateSkill(int buttonNum) {
        Frames[buttonNum].color = _colorGrey;
        SkillIcons[buttonNum].color = _colorGrey;
    }
    public void ActivateSkill(int buttonNum) {
        Frames[buttonNum].color = _colorWhite;
        SkillIcons[buttonNum].color = _skillsColors[buttonNum];
    }

    public IEnumerator StartCooldown(int skillNum, int cooldownValue) {
        for (int i = cooldownValue; i > 0; i--) {
            CoolTexts[skillNum].text = "" + i;
            Masks[skillNum].fillAmount = i * 1f / cooldownValue;
            yield return new WaitForSeconds(1f);
        }
        CoolTexts[skillNum].text = "";
        Masks[skillNum].fillAmount = 0;
    }

}
