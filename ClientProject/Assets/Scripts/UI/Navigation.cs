using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Navigation : MonoBehaviour {

    public UIWindow SendPanel;      //Destination panel
    public UIWindow CurrPanel;      //Current panel
    public MenuNavigation dummy;
    public int specialAction;       //Is button execute special action
    public UIMessageWindow mWindow;

    private Button NowButton;       //Current button

    // Use this for initialization
    void Start() {
        NowButton = gameObject.GetComponent<Button>();
        NowButton.onClick.AddListener(Activation);
    }

    void Activation() {
        //Buttons special actions
        switch (specialAction) {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("road_endurance");
                break;
            case 2:
                dummy.HideLeftPanels();
                SendPanel.anim.SetBool("trigger", true);
                return;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("codex");
                break;
            case 99:
                mWindow.ShowMessage("Are you really want to quit game?", 0, UIMessageWindow.Action.exit);
                return;
        }
        //Show dest. panel, hide current
        if (CurrPanel != null) {
            CurrPanel.anim.SetBool("trigger", false);
        } else {
            dummy.HideRightPanels();
        }
        if (SendPanel != null) {
            SendPanel.anim.SetBool("trigger", true);
            dummy.panel = SendPanel.returnPanel;
        }
        dummy.backButton.gameObject.SetActive(true);
    }
}
