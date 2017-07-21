using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum FallBackPanel { 
    None = 0,
    FreeMode = 1,
    MLC = 2
};
public enum PanelType {
    Undefined = 0,
    LeftPanel = 1,
    RightPanel = 2
};

public class UIWindow : MonoBehaviour {

    public Animator anim;
    public FallBackPanel returnPanel;
    public PanelType panelType;
    public FallBackPanel fallback;

}
