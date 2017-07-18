using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class EndModeController : MonoBehaviour {

    //UI
    public Image ponyIcon;
    public Image ponyHP;
    public Image ponyMP;
    public Button pauseButton;
    public Button retireButton;
    public Text pauseText;
    public Text retireText;
    public Text distance;
    public GameObject InfoScreen;
    public Text InfoText;
    public GameObject SkillsPanel;
    public Image[] SkillIcons;
    public Text stamina;
    public Text charName;
    //Popup
    public Image popupIcon;
    public Text popupText;
    //SplashSideImage
    public Image splashGrad;
    public Animator splashText;

    public Transform mainCamera;
    public Animator[] flashingUI;

    public float currentHP;
    public float currentMP;
    public float currentMP_rec;
    public int popupTimer;
    [Header("Common")]
    public float SPDmlp;    //Speed multiplier. Used in PonyController
    public float DMGmlp;    //Damage multiplier. Used in PonyController
    public float timeSpeed; //Current time length. Used in slowing motions skills
    [Header("Events")]
    public UnityAction<int, Skill.Condition> CharacterState;

    private CharsFMData _pony;
    private Transform player;
    private float deltaCam; //Main Camera last x position
    //Statement check
    bool IsMPLow, IsHPLow;

    #region API

    void Start() {
        Invoke("FindPony", 0.3f);
        _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        pauseButton.onClick.AddListener(GamePause);
        retireButton.onClick.AddListener(GameRetire);
        ponyIcon.sprite = _pony.CharIcon;
        //Character Spawn
        Vector3 v3 = new Vector3(-3f, 0f, 0f);
        Instantiate(_pony.GamePlayPrefab, v3, _pony.GamePlayPrefab.transform.rotation);
        //Character Set HP and MP
        currentHP = _pony.HP;
        currentMP = 0;
        currentMP_rec = _pony.MPRecovery;
        //Set STM value
        stamina.text = "" + Mathf.RoundToInt(Database.Instance.GetCurrSTM(Database.Instance.SelectedPony));
        deltaCam = mainCamera.position.x;
        //Set pony name
        charName.text = _pony.CharName;
        charName.color = _pony.CharColor;
        stamina.color = _pony.CharColor;

        //Set Skill icons
        SkillIcons[1].sprite = _pony.CharSkills[1].icon;
        SkillIcons[2].sprite = _pony.CharSkills[2].icon;
        SkillIcons[3].sprite = _pony.CharSkills[3].icon;

        distance.text = "0";
        SoundManager.Instance.SetMuteState("a_run", true);
        popupTimer = -1;
        Time.timeScale = 0;
        timeSpeed = 1;
        DMGmlp = 1; SPDmlp = 1;
    }

    void FindPony() {
        player = PonyController.Instance.transform;
    }

    void Update() {
        //Passed Distance
        if (player != null) distance.text = "" + (player.position.x > 0 ? Mathf.RoundToInt(player.position.x * 2) : 0);
        //Retire
        if (Input.GetButtonDown("Back") && Time.timeScale == 0) {
            GameRetire();
        }
        //Pause switch
        if (Input.GetButtonDown("Start") && pauseButton.gameObject.activeSelf) {
            GamePause();
        }
        //Statements checking
        if (currentHP < _pony.HP / 2 && !IsHPLow) {
            CharacterState(0, Skill.Condition.HPLow);
            IsHPLow = true;
        }
        if (currentHP >= _pony.HP / 2 && IsHPLow) {
            CharacterState(0, Skill.Condition.HPHigh);
            IsHPLow = false;
        }
        if (currentMP < _pony.MP / 2 && !IsMPLow) {
            CharacterState(0, Skill.Condition.MPLow);
            IsMPLow = true;
        }
        if (currentMP >= _pony.MP / 2 && IsMPLow) {
            CharacterState(0, Skill.Condition.MPHigh);
            IsMPLow = false;
        }
    }

    void FixedUpdate() {
        //Recalculate STM Value
        Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 0 - (mainCamera.position.x - deltaCam) * 2);
        stamina.text = "" + Mathf.RoundToInt(Database.Instance.GetCurrSTM(Database.Instance.SelectedPony));
        //PopupTimer Counter
        if (popupTimer >= 0) { popupTimer--; }
        if (popupTimer == 0) { popupIcon.gameObject.SetActive(false); }
        //Refresh animators values
        flashingUI[0].SetFloat("value", ponyHP.fillAmount);
        flashingUI[1].SetFloat("value", ponyHP.fillAmount);
        flashingUI[2].SetFloat("value", ponyMP.fillAmount);
        flashingUI[3].SetFloat("value", ponyMP.fillAmount);
        //Splash Image controller
        if (ponyHP.fillAmount < 0.4f && !splashGrad.gameObject.activeSelf) { splashGrad.gameObject.SetActive(true); }
        if (ponyHP.fillAmount > 0.4f && splashGrad.gameObject.activeSelf) { splashGrad.gameObject.SetActive(false); }
        splashText.SetFloat("value", ponyHP.fillAmount);
        //MP recovery
        if (currentMP < _pony.MP) currentMP += currentMP_rec;
        if (currentMP > _pony.MP) currentMP = _pony.MP;
        //HP checking
        if (currentHP > _pony.HP) currentHP = _pony.HP;
        //Set HP and MP lines
        ponyHP.fillAmount = currentHP / _pony.HP;
        ponyMP.fillAmount = currentMP / _pony.MP;
        //Set new maincamera x pos
        deltaCam = mainCamera.position.x;
        //KO screen draw
        if (currentHP <= 0) { ShowKOWindow(); }
        if (Database.Instance.GetCurrSTM(Database.Instance.SelectedPony) <= 0) { ShowSTMOutWindow(); }

    }

    #endregion

    public void ShowPopupInfo(string item) {
        popupTimer = 60;
        popupIcon.sprite = Database.Instance.GetItemIcon(item);
        popupText.text = "" + Mathf.FloorToInt(Database.Instance.GetItemQuantity(item));
        popupIcon.gameObject.SetActive(true);
    }

    void ShowSTMOutWindow() {
        retireButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        SetInfoScreenState("You are out of stamina!", false);
        SoundManager.Instance.SetMuteState("a_run", true);
        Time.timeScale = 0f;
    }
    void ShowKOWindow() {
        retireButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        SetInfoScreenState("You are Knocked Out!", false);
        SoundManager.Instance.SetMuteState("a_run", true);
        Time.timeScale = 0f;
    }
    public void ShowPassWindow() {
        retireButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        SetInfoScreenState("Challenge Complete!", false);
        retireText.text = "Ok";
        SoundManager.Instance.SetMuteState("a_run", true);
        Time.timeScale = 0f;
    }

    //Pause Game
    void GamePause() {
        if (Time.timeScale == 0) {
            pauseText.text = "Pause";
            retireButton.gameObject.SetActive(false);
            InfoScreen.SetActive(false);
            SoundManager.Instance.SetMuteState("a_run", false);
            Time.timeScale = timeSpeed;
        } else {
            pauseText.text = "Resume";
            retireButton.gameObject.SetActive(true);
            SetInfoScreenState("", true);
            Time.timeScale = 0f;
        }
    }

    void GameRetire() {
        Time.timeScale = 1;
        if (player != null) {
            Database.Instance.distTotal += player.position.x > 0 ? Mathf.RoundToInt(player.position.x * 2) : 0;
            if (FindObjectOfType<ChallModeController>() != null) {
                Database.Instance.distChall += player.position.x > 0 ? Mathf.RoundToInt(player.position.x * 2) : 0;
            } else {
                Database.Instance.distEnd += player.position.x > 0 ? Mathf.RoundToInt(player.position.x * 2) : 0;
                if (player.position.x * 2 > Database.Instance.distEndEasy) {
                    Database.Instance.distEndEasy = Mathf.RoundToInt(player.position.x * 2);
                }
            }
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

    void SetInfoScreenState(string text, bool skills) {
        InfoScreen.SetActive(true);
        SkillsPanel.SetActive(skills);
        InfoText.gameObject.SetActive(!skills);
        if (text != "") { InfoText.text = text; }
    }
}
