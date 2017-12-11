using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RaceCourceController : MonoBehaviour {

    [SerializeField]
    DBChallenges DBC = null;

    [Header("UI")]
    public Image ponyIcon;
    public Image ponyHP;
    public Image ponyMP;
    public Button pauseButton;
    public Button retireButton;
    public Text pauseText;
    public Text retireText;
    public Text distanceText;
    public GameObject InfoScreen;
    public Text InfoText;
    public GameObject SkillsPanel;
    public Image[] SkillIcons;
    public Text stamina;
    public Text charName;
    public Text timerText;
    [Header("Common")]
    public Animator[] flashingUI;

    [Header("Events")]
    public UnityAction<int, Skill.Condition> CharacterState;

    private CharsFMData _pony;
    private Transform player;
    private float _distance;
    private Camera mainCamera;
    private TimeSpan _timer;
    private Challenge challenge;
    //Statement check
    bool IsMPLow, IsHPLow;

    #region API

    void Start() {
        MusicManager.Instance.SetFolder("Music/Endurance", 1);
        GlobalData.Instance.gameState = GameModeState.Challenge;
        challenge = DBC.GetChallenge(GlobalData.Instance.nowChallenge);

        Instantiate(challenge.terrain);
        Invoke("FindPony", 0.3f);
        _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        pauseButton.onClick.AddListener(GamePause);
        retireButton.onClick.AddListener(GameRetire);
        ponyIcon.sprite = _pony.CharIcon;
        //Character Spawn
        Instantiate(_pony.FreePlayPrefab, _pony.FreePlayPrefab.transform.position, _pony.FreePlayPrefab.transform.rotation);
        //Character Set HP and MP
        GlobalData.Instance.currentHP = _pony.HP;
        GlobalData.Instance.currentMP = 0;
        GlobalData.Instance.currentMP_rec = _pony.MPRecovery;
        GlobalData.Instance.isMPProtection = false;
        GlobalData.Instance.PickupMlp = 1f;
        //Set STM value
        stamina.text = "" + Mathf.RoundToInt(Database.Instance.GetCurrSTM(Database.Instance.SelectedPony));
        //Set pony name
        charName.text = _pony.CharName;
        charName.color = _pony.CharColor;
        stamina.color = _pony.CharColor;
        mainCamera = Camera.main;

        //Set Skill icons
        SkillIcons[1].sprite = _pony.CharSkills[1].icon;
        SkillIcons[2].sprite = _pony.CharSkills[2].icon;
        SkillIcons[3].sprite = _pony.CharSkills[3].icon;

        distanceText.text = "0";
        SoundManager.Instance.SetMuteState("a_run", true);
        Time.timeScale = 0;
        GlobalData.Instance.timeSpeed = 1;
        GlobalData.Instance.DMGmlp = 1;
        GlobalData.Instance.SPDmlp = 1;
        _distance = 0;
        _timer = new TimeSpan(0, (int)challenge.timeRestr.x, (int)challenge.timeRestr.y);

        IEnumerator challengeTime = ChallengeTime();
        StartCoroutine(challengeTime);
    }

    void FindPony() {
        player = PonyFreeMoveController.Instance.transform;
    }

    void Update() {
        //Retire
        if (Input.GetButtonDown("Back") && Time.timeScale == 0) { GameRetire(); }
        //Pause switch
        if (Input.GetButtonDown("Start") && pauseButton.gameObject.activeSelf) { GamePause(); }
        //Statements checking
        if (GlobalData.Instance.currentHP < _pony.HP / 2 && !IsHPLow) {
            if (CharacterState != null) CharacterState(0, Skill.Condition.HPLow);
            IsHPLow = true;
        }
        if (GlobalData.Instance.currentHP >= _pony.HP / 2 && IsHPLow) {
            if (CharacterState != null) CharacterState(0, Skill.Condition.HPHigh);
            IsHPLow = false;
        }
        if (GlobalData.Instance.currentMP < _pony.MP / 2 && !IsMPLow) {
            if (CharacterState != null) CharacterState(0, Skill.Condition.MPLow);
            IsMPLow = true;
        }
        if (GlobalData.Instance.currentMP >= _pony.MP / 2 && IsMPLow) {
            if (CharacterState != null) CharacterState(0, Skill.Condition.MPHigh);
            IsMPLow = false;
        }
    }

    void FixedUpdate() {
        if (player == null) return;
        //Get tick pony flat velocity magnitude
        Vector3 ponyVelocity = player.GetComponent<Rigidbody>().velocity;
        float flatMagnitude = Mathf.Sqrt(ponyVelocity.x * ponyVelocity.x + ponyVelocity.z * ponyVelocity.z);
        //Recalculate STM Value
        Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 0 - flatMagnitude / 20f);
        stamina.text = "" + Mathf.RoundToInt(Database.Instance.GetCurrSTM(Database.Instance.SelectedPony));
        //Passed Distance
        _distance += flatMagnitude / 20f;
        distanceText.text = "" + Mathf.RoundToInt(_distance);
        //Refresh animators values
        flashingUI[0].SetFloat("value", ponyHP.fillAmount);
        flashingUI[1].SetFloat("value", ponyHP.fillAmount);
        flashingUI[2].SetFloat("value", ponyMP.fillAmount);
        flashingUI[3].SetFloat("value", ponyMP.fillAmount);
        //MP recovery
        if (GlobalData.Instance.currentMP < _pony.MP) GlobalData.Instance.currentMP += GlobalData.Instance.currentMP_rec;
        if (GlobalData.Instance.currentMP > _pony.MP) GlobalData.Instance.currentMP = _pony.MP;
        //HP checking
        if (GlobalData.Instance.currentHP > _pony.HP) GlobalData.Instance.currentHP = _pony.HP;
        //Set HP and MP lines
        ponyHP.fillAmount = GlobalData.Instance.currentHP / _pony.HP;
        ponyMP.fillAmount = GlobalData.Instance.currentMP / _pony.MP;
        //Low HP screen effects
        mainCamera.GetComponent<ColorCorrectionCurves>().saturation = GlobalData.Instance.currentHP > _pony.HP / 2 ? 1 : GlobalData.Instance.currentHP / (_pony.HP / 2);
        mainCamera.GetComponent<ScreenOverlay>().intensity = GlobalData.Instance.currentHP > _pony.HP / 2 ? 1 : (10 - 10 * (GlobalData.Instance.currentHP / (_pony.HP / 2)));
        //KO screen draw
        if (GlobalData.Instance.currentHP <= 0) { ShowKOWindow(); }
        if (Database.Instance.GetCurrSTM(Database.Instance.SelectedPony) <= 0) { ShowSTMOutWindow(); }
        //Timeout Message
        if (_timer.TotalSeconds <= 0) ShowTimeoutWindow();

    }

    #endregion

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
        SetInfoScreenState("Challenge Completed!", false);
        retireText.text = "Ok";
        SoundManager.Instance.SetMuteState("a_run", true);
        foreach (var item in challenge.reward) {
            Database.Instance.IncreaseItemQuantity(item.ItemName, item.ItemQuantity);
        }
        Database.Instance.passedChallenges[GlobalData.Instance.nowChallenge]++;
        Time.timeScale = 0f;
    }
    public void ShowTimeoutWindow() {
        retireButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        SetInfoScreenState("You run out of time!", false);
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
            Time.timeScale = GlobalData.Instance.timeSpeed;
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
        Database.Instance.distChall += (int)_distance;
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

    void SetInfoScreenState(string text, bool skills) {
        InfoScreen.SetActive(true);
        SkillsPanel.SetActive(skills);
        InfoText.gameObject.SetActive(!skills);
        if (text != "") { InfoText.text = text; }
    }

    IEnumerator ChallengeTime() {
        TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        while (_timer.TotalSeconds > 0) {
            yield return new WaitForSeconds(1);
            _timer = _timer.Subtract(oneSecond);
            timerText.text = string.Format("Time: {0:m}", _timer);
        }
    }
}
