using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Navigation : MonoBehaviour {

    public UIWindow SendPanel;      //Destination panel
    public UIWindow CurrPanel;      //Current panel
    public UIAction specialAction;  //Is button execute special action

    private Button NowButton;       //Current button

    void Start() {
        NowButton = gameObject.GetComponent<Button>();
        NowButton.onClick.AddListener(Activation);
    }

    void Activation() {
        //Buttons special actions
        switch (specialAction) {
            case UIAction.startEndurance:
                UnityEngine.SceneManagement.SceneManager.LoadScene("road_endurance");
                break;
            case UIAction.hideLeftPanels:
                MenuNavigation.Instance.HideLeftPanels();
                SendPanel.anim.SetBool("trigger", true);
                return;
            case UIAction.loadCodex:
                UnityEngine.SceneManagement.SceneManager.LoadScene("codex");
                break;
            case UIAction.exit:
                UIMessageWindow.Instance.ShowMessage("Are you really want to quit game?", 0, UIAction.exit);
                return;
            case UIAction.returnToEquestria:
                UIMessageWindow.Instance.ShowMessage("Exit the simulation?", 0, UIAction.returnToEquestria);
                return;
            case UIAction.runSimulation:
                UIMessageWindow.Instance.ShowMessage("Enter into simulation?", 0, UIAction.runSimulation);
                return;
            case UIAction.startRaceCource:
                UIMessageWindow.Instance.ShowMessage("Start test Race cource?", 0, UIAction.startRaceCource);
                return;
        }
        //Show dest. panel, hide current
        if (CurrPanel != null) {
            CurrPanel.anim.SetBool("trigger", false);
        } else {
            MenuNavigation.Instance.HideRightPanels();
        }
        if (SendPanel != null) {
            SendPanel.anim.SetBool("trigger", true);
            MenuNavigation.Instance.panel = SendPanel.returnPanel;
        }
        MenuNavigation.Instance.backButton.gameObject.SetActive(true);
    }
}
