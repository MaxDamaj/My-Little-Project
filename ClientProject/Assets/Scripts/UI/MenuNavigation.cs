using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuNavigation : MonoBehaviour {

    public Button backButton;
    public Animator[] UIRightPanels;
    public Animator[] UILeftPanels;
    public FallBackPanel panel;
    public Text ScenePonyName;

    void Start() {
        //First Panel Active
        backButton.onClick.AddListener(GetBack);
        for (int i = 0; i < UIRightPanels.GetLength(0); i++) {
            UIRightPanels[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < UILeftPanels.GetLength(0); i++) {
            UILeftPanels[i].gameObject.SetActive(true);
        }
        UIRightPanels[0].SetBool("trigger", true); //FreeMode panel
                                                   //Spawn Selected Pony first time
        CharsFMData pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        RefreshPreviewMesh(pony);
    }

    void GetBack() {
        //Hide all panels
        HideRightPanels();
        HideLeftPanels();
        //Return to main Screen
        if (panel == FallBackPanel.FreeMode) {
            UIRightPanels[0].SetBool("trigger", true);
            backButton.gameObject.SetActive(false);
            return;
        }
        //Return to MLC Screen
        if (panel == FallBackPanel.MLC) {
            UIRightPanels[2].SetBool("trigger", true); //MLC window
            panel = FallBackPanel.FreeMode;
            return;
        }
    }

    public void HideRightPanels() {
        for (int i = 0; i < UIRightPanels.GetLength(0); i++) {
            UIRightPanels[i].SetBool("trigger", false);
        }
    }
    public void HideLeftPanels() {
        for (int i = 0; i < UILeftPanels.GetLength(0); i++) {
            UILeftPanels[i].SetBool("trigger", false);
        }
    }

    public void RefreshPreviewMesh(CharsFMData _pony) {
        GameObject _previewPony = GameObject.FindGameObjectWithTag("Player");
        if (_previewPony != null) { Destroy(_previewPony); }
        Instantiate(_pony.PreviewPrefab, _pony.PreviewPrefab.transform.position, _pony.PreviewPrefab.transform.rotation);
        ScenePonyName.text = _pony.CharName;
        ScenePonyName.color = _pony.CharColor;
    }
}
