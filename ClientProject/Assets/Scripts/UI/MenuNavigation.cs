using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuNavigation : MonoBehaviour {

    public Button backButton;
    public RectTransform panelsContainer;
    public List<UIWindow> UIRightPanels;
    public List<UIWindow> UILeftPanels;
    public Text ScenePonyName;
    [HideInInspector]
    public FallBackPanel panel; 

    void Start() {
        //Find all panels
        List<UIWindow> panels = new List<UIWindow>();
        panels.AddRange(panelsContainer.GetComponentsInChildren<UIWindow>(true));
        UIRightPanels.AddRange(panels.FindAll(x => x.panelType == PanelType.RightPanel));
        UILeftPanels.AddRange(panels.FindAll(x => x.panelType == PanelType.LeftPanel));

        //First Panel Active
        backButton.onClick.AddListener(GetBack);
        for (int i = 0; i < UIRightPanels.Count; i++) {
            UIRightPanels[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < UILeftPanels.Count; i++) {
            UILeftPanels[i].gameObject.SetActive(true);
        }
        UIRightPanels.Find(x => x.fallback == FallBackPanel.FreeMode).anim.SetBool("trigger", true); //FreeMode panel
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
            UIRightPanels.Find(x => x.fallback == FallBackPanel.FreeMode).anim.SetBool("trigger", true);
            backButton.gameObject.SetActive(false);
            return;
        }
        //Return to MLC Screen
        if (panel == FallBackPanel.MLC) {
            UIRightPanels.Find(x => x.fallback == FallBackPanel.MLC).anim.SetBool("trigger", true); //MLC window
            panel = FallBackPanel.FreeMode;
            return;
        }
    }

    public void HideRightPanels() {
        for (int i = 0; i < UIRightPanels.Count; i++) {
            UIRightPanels[i].anim.SetBool("trigger", false);
        }
    }
    public void HideLeftPanels() {
        for (int i = 0; i < UILeftPanels.Count; i++) {
            UILeftPanels[i].anim.SetBool("trigger", false);
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
