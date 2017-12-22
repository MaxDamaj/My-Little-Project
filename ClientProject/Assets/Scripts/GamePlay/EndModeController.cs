using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using MLA.Gameplay.Common;
using MLA.System;
using MLA.System.Controllers;

namespace MLA.Gameplay.Controllers {
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
        public Image charLuck;

        public Transform mainCamera;
        public Animator[] flashingUI;

        [Header("Events")]
        public UnityAction<int, Skill.Condition> CharacterState;

        private CharsFMData _pony;
        private Transform player;
        private float deltaCam; //Main Camera last x position
                                //Statement check
        bool IsMPLow, IsHPLow;

        #region API

        void Start() {
            MusicManager.Instance.SetFolder("Music/Endurance", 1);
            GlobalData.Instance.gameState = GameModeState.Endurance;

            Invoke("FindPony", 0.3f);
            _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
            pauseButton.onClick.AddListener(GamePause);
            retireButton.onClick.AddListener(GameRetire);
            ponyIcon.sprite = _pony.CharIcon;
            //Character Spawn
            Vector3 v3 = new Vector3(-3f, 0f, 0f);
            Instantiate(_pony.GamePlayPrefab, v3, _pony.GamePlayPrefab.transform.rotation);
            //Character Set HP and MP
            GlobalData.Instance.currentHP = _pony.HP;
            GlobalData.Instance.currentMP = 0;
            GlobalData.Instance.currentMP_rec = _pony.MPRecovery;
            GlobalData.Instance.isMPProtection = false;
            GlobalData.Instance.PickupMlp = 1f;
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
            Time.timeScale = 0;
            GlobalData.Instance.timeSpeed = 1;
            GlobalData.Instance.DMGmlp = 1;
            GlobalData.Instance.SPDmlp = 1;
        }

        void FindPony() {
            player = PonyController.Instance.transform;
        }

        void Update() {
            //Passed Distance
            if (player != null) distance.text = "" + (player.position.x > 0 ? Mathf.RoundToInt(player.position.x * 2) : 0);
            //Retire
            if (Input.GetButtonDown("Back") && Time.timeScale == 0) { GameRetire(); }
            //Pause switch
            if (Input.GetButtonDown("Start") && pauseButton.gameObject.activeSelf) { GamePause(); }
            //Statements checking
            if (GlobalData.Instance.currentHP < _pony.HP / 2 && !IsHPLow) {
                CharacterState(0, Skill.Condition.HPLow);
                IsHPLow = true;
            }
            if (GlobalData.Instance.currentHP >= _pony.HP / 2 && IsHPLow) {
                CharacterState(0, Skill.Condition.HPHigh);
                IsHPLow = false;
            }
            if (GlobalData.Instance.currentMP < _pony.MP / 2 && !IsMPLow) {
                CharacterState(0, Skill.Condition.MPLow);
                IsMPLow = true;
            }
            if (GlobalData.Instance.currentMP >= _pony.MP / 2 && IsMPLow) {
                CharacterState(0, Skill.Condition.MPHigh);
                IsMPLow = false;
            }
        }

        void FixedUpdate() {
            //Recalculate STM Value
            Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, 0 - (mainCamera.position.x - deltaCam) * 2);
            stamina.text = "" + Mathf.RoundToInt(Database.Instance.GetCurrSTM(Database.Instance.SelectedPony));
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
            charLuck.fillAmount = _pony.LUCK / 100f;
            //Set new maincamera x pos
            deltaCam = mainCamera.position.x;
            //Low HP screen effects
            mainCamera.GetComponent<ColorCorrectionCurves>().saturation = GlobalData.Instance.currentHP > _pony.HP / 2 ? 1 : GlobalData.Instance.currentHP / (_pony.HP / 2);
            mainCamera.GetComponent<ScreenOverlay>().intensity = GlobalData.Instance.currentHP > _pony.HP / 2 ? 1 : (10 - 10 * (GlobalData.Instance.currentHP / (_pony.HP / 2)));
            //KO screen draw
            if (GlobalData.Instance.currentHP <= 0) { ShowKOWindow(); }
            if (Database.Instance.GetCurrSTM(Database.Instance.SelectedPony) <= 0) { ShowSTMOutWindow(); }

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
                Time.timeScale = GlobalData.Instance.timeSpeed;
            } else {
                pauseText.text = "Resume";
                retireButton.gameObject.SetActive(true);
                SetInfoScreenState("", true);
                SoundManager.Instance.SetMuteState("a_run", true);
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
            SoundManager.Instance.SetMuteState("a_run", true);
            UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
        }

        void SetInfoScreenState(string text, bool skills) {
            InfoScreen.SetActive(true);
            SkillsPanel.SetActive(skills);
            InfoText.gameObject.SetActive(!skills);
            if (text != "") { InfoText.text = text; }
        }
    }
}
