﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum FallBackPanel { 
    None = 0,
    FreeMode = 1,
    MLC = 2,
    PlayMode = 3
};
public enum PanelType {
    Undefined = 0,
    LeftPanel = 1,
    RightPanel = 2
};

namespace MLA.UI.Common {
    public class UIWindow : MonoBehaviour {

        public Animator anim;
        public FallBackPanel returnPanel;
        public PanelType panelType;
        public FallBackPanel fallback;
    }
}
