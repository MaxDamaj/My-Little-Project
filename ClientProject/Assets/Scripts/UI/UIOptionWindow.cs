using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIOptionWindow : MonoBehaviour {

    [Header("UI")]
    public Slider camPosition;
    public Toggle bloomCheck;
    public Toggle SSAOCheck;
    public Button clearButton;

    [Header("Common")]
    public GameObject mainCamera;

    // Use this for initialization
    void Start() {
        clearButton.onClick.AddListener(ShowBuyWindow);
        camPosition.value = Database.Instance.cameraShift;
        bloomCheck.isOn = Database.Instance.paramBloom == 1;
        SSAOCheck.isOn = Database.Instance.paramSSAO == 1;
        Database.onRefresh += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI() {
        if (Database.Instance == null) return;
        Database.Instance.cameraShift = camPosition.value;
        Database.Instance.paramBloom = bloomCheck.isOn ? 1 : 0;
        Database.Instance.paramSSAO = SSAOCheck.isOn ? 1 : 0;
        if (mainCamera != null) mainCamera.GetComponent<Bloom>().enabled = Database.Instance.paramBloom == 1;
        if (mainCamera != null) mainCamera.GetComponent<AmbientObscurance>().enabled = Database.Instance.paramSSAO == 1;
    }

    void ShowBuyWindow() {
        UIMessageWindow.Instance.ShowMessage("Are you really want to clear game state? You cannot undo this action!", 0, UIAction.clear);
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
        RefreshUI();
    }
}
