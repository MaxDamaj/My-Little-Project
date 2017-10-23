using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIOptionWindow : MonoBehaviour {

    [Header("UI")]
    public Slider camPosition;
    public Slider musicVolume;
    public Toggle bloomCheck;
    public Toggle SSAOCheck;
    public Button clearButton;

    [Header("Common")]
    public GameObject mainCamera;

    private bool IsChangesAllow = false;

    // Use this for initialization
    void Start() {
        clearButton.onClick.AddListener(ShowBuyWindow);
        Invoke("SetValues", 0.35f);
    }

    void SetValues() {
        camPosition.value = Database.Instance.cameraShift;
        musicVolume.value = Database.Instance.musicVolume;
        bloomCheck.isOn = Database.Instance.paramBloom == 1;
        IsChangesAllow = true;
        SSAOCheck.isOn = Database.Instance.paramSSAO == 1;
        RefreshUI();
    }

    public void RefreshUI() {
        if (!IsChangesAllow) return;
        if (Database.Instance == null) return;
        Database.Instance.cameraShift = camPosition.value;
        Database.Instance.musicVolume = musicVolume.value;
        Database.Instance.paramBloom = bloomCheck.isOn ? 1 : 0;
        Database.Instance.paramSSAO = SSAOCheck.isOn ? 1 : 0;
        if (mainCamera != null) mainCamera.GetComponent<Bloom>().enabled = Database.Instance.paramBloom == 1;
        if (mainCamera != null) mainCamera.GetComponent<AmbientObscurance>().enabled = Database.Instance.paramSSAO == 1;
        if (MusicManager.Instance != null) MusicManager.Instance.SetMusicVolume(Database.Instance.musicVolume);
    }

    void ShowBuyWindow() {
        UIMessageWindow.Instance.ShowMessage("Are you really want to clear game state?", 0, UIAction.clear);
    }

    void OnDestroy() {
        RefreshUI();
    }
}
