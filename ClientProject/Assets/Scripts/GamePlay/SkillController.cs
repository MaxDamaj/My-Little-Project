using UnityEngine;
using System.Collections;
using MLA.System;
using MLA.Gameplay.Common;
using MLA.UI.Controllers;
using MLA.UI.Windows;
using MLA.System.Controllers;

namespace MLA.Gameplay.Controllers {
    public class SkillController : MonoBehaviour {

        public bool IsSimulation = false;
        [SerializeField]
        GameObject[] fx = null;

        private GameObject Pony;
        private EndModeController _emc;
        private SimModeController _smc;
        private RaceCourceController _rcc;
        private UISkills _uiSkill;
        private IEnumerator cooldown;
        private CharsFMData Character;
        private float MPDrain;
        private float holdTimer = 0;

        private static SkillController controller;

        #region API

        public static SkillController Instance {
            get {
                if (controller == null) {
                    controller = FindObjectOfType<SkillController>();
                }
                return controller;
            }
        }

        void Start() {
            if (!IsSimulation) {
                Character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
            } else {
                Character = DBSimulation.Instance.simCharacter;
            }
            if (GlobalData.Instance.gameState == GameModeState.Endurance) {
                Pony = PonyController.Instance.gameObject;
            } else {
                Pony = PonyFreeMoveController.Instance.gameObject;
            }

            _emc = FindObjectOfType<EndModeController>();
            _smc = FindObjectOfType<SimModeController>();
            _rcc = FindObjectOfType<RaceCourceController>();
            _uiSkill = FindObjectOfType<UISkills>();
            //Set all skills to execute state
            for (int i = 0; i < Character.CharSkills.GetLength(0); i++) {
                Character.CharSkills[i].IsCooldown = false;
                _uiSkill.ActivateSkill(i);
            }
            if (_emc != null) { _emc.CharacterState = SkillConditionStats; }
            if (_smc != null) { _smc.CharacterState = SkillConditionStats; }
            if (_rcc != null) { _rcc.CharacterState = SkillConditionStats; }
            MPDrain = 0;
            //Attash FXs to pony
            for (int i = 0; i < 4; i++) {
                if (Character.CharSkills[i].fx != "") {
                    fx[i] = Instantiate(Resources.Load<GameObject>("FXs/" + Character.CharSkills[i].fx), Pony.transform);
                    fx[i].SetActive(false);
                }
            }
        }

        void Update() {
            if (Input.GetButtonDown("Fire3")) { //X Button DOWN
                SkillXDown();
            }
            if (Input.GetButton("Fire3")) { //X Button HOLD
                SkillXHold();
            }
            if (Input.GetButtonUp("Fire3")) { //X Button UP
                SkillXUp();
            }
            if (Input.GetButtonDown("Fire1")) { //Y Button
                SkillYDown();
            }
            if (Input.GetButtonDown("Fire2")) { //B Button
                SkillBDown();
            }
            if (holdTimer > 0) holdTimer -= Time.deltaTime;
        }

        #endregion

        public void SkillXDown() {
            SkillDash(1);
            SkillProjectile(1);
            SkillFly(1, true);
        }
        public void SkillXHold() {
            GlobalData.Instance.currentMP -= MPDrain;
            if (GlobalData.Instance.currentMP <= 0) {
                GlobalData.Instance.currentMP = 0;
                SkillFly(1, false);
            }
            if (holdTimer <= 0) {
                SkillProjectile(1);
            }
        }
        public void SkillXUp() {
            SkillFly(1, false);
        }
        public void SkillYDown() {
            SkillStatChange(2);
            SkillProjectile(2);
        }
        public void SkillBDown() {
            SkillRestoreStat(3);
            SkillItemGiving(3);
            SkillSlowMotion(3);
            SkillMPProtection(3);
        }

        #region Skills

        void SkillDash(int sn) {
            if ((int)Character.CharSkills[sn].skillType == 1 && GlobalData.Instance.currentMP >= Character.CharSkills[sn].MP_cost && !Character.CharSkills[sn].IsCooldown) {
                Character.CharSkills[sn].IsCooldown = true;
                SoundManager.Instance.PlaySound(Character.CharSkills[sn].sound);
                GlobalData.Instance.currentMP -= Character.CharSkills[sn].MP_cost;
                GlobalData.Instance.SPDmlp = GlobalData.Instance.SPDmlp * Character.CharSkills[sn].SPDmlp;
                GlobalData.Instance.DMGmlp = GlobalData.Instance.DMGmlp * Character.CharSkills[sn].DMGmlp;
                _uiSkill.DeactivateSkill(sn);
                if (fx[sn] != null) fx[sn].SetActive(true);
                IEnumerator endDash = Dash(Character.CharSkills[sn].duration, sn);
                StartCoroutine(endDash);
                cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].duration);
                StartCoroutine(cooldown);
            }
        }
        void SkillProjectile(int sn) {
            if ((int)Character.CharSkills[sn].skillType == 3 && GlobalData.Instance.currentMP >= Character.CharSkills[sn].MP_cost) {
                GameObject spark_cl;
                Vector3 v3 = Pony.transform.position + Character.CharSkills[sn].projPosition;
                holdTimer = Character.CharSkills[sn].duration;
                SoundManager.Instance.PlaySound(Character.CharSkills[sn].sound);
                if (fx[sn] != null) {
                    fx[sn].SetActive(true);
                    IEnumerator fxDisable = DisableObject(fx[sn], 0.15f);
                    StartCoroutine(fxDisable);
                }
                GlobalData.Instance.currentMP -= Character.CharSkills[sn].MP_cost;
                spark_cl = Instantiate(Character.CharSkills[sn].obj, v3, Pony.transform.rotation);
                spark_cl.transform.SetParent(Pony.transform);
                spark_cl.transform.localPosition = Character.CharSkills[sn].projPosition;
                spark_cl.GetComponent<Projectile>().DestroyObject(Character.CharSkills[sn].cooldown);
            }
        }
        void SkillStatChange(int sn) {
            if ((int)Character.CharSkills[sn].skillType == 4 && GlobalData.Instance.currentMP >= Character.CharSkills[sn].MP_cost) {
                GlobalData.Instance.currentMP -= Character.CharSkills[sn].MP_cost;
                if (Character.CharSkills[sn].statType == Skill.StatType.Health) { GlobalData.Instance.currentHP += Character.CharSkills[sn].multiplier; }
                if (Character.CharSkills[sn].statType == Skill.StatType.Mana) { GlobalData.Instance.currentMP = Character.MP * Character.CharSkills[sn].multiplier; }
                if (Character.CharSkills[sn].statType == Skill.StatType.Stamina) {
                    Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, Character.CharSkills[sn].multiplier);
                    if (fx[sn] != null) {
                        fx[sn].SetActive(true);
                        IEnumerator fxDisable = DisableObject(fx[sn], 2f);
                        StartCoroutine(fxDisable);
                    }
                }
                if ((int)Character.CharSkills[sn].statType == 3) { GlobalData.Instance.SPDmlp *= Character.CharSkills[sn].multiplier; }
                if ((int)Character.CharSkills[sn].statType == 4) { GlobalData.Instance.DMGmlp *= Character.CharSkills[sn].multiplier; }
                if ((int)Character.CharSkills[sn].statType > 2) {
                    _uiSkill.DeactivateSkill(sn);
                    if (fx[sn] != null) fx[sn].SetActive(true);
                    IEnumerator endStatChange = StatChange(Character.CharSkills[sn].duration, sn, (int)Character.CharSkills[sn].statType);
                    StartCoroutine(endStatChange);
                    cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].duration);
                    StartCoroutine(cooldown);
                }
            }
        }
        void SkillRestoreStat(int sn) {
            if ((int)Character.CharSkills[sn].skillType == 5 && !Character.CharSkills[sn].IsCooldown) {
                Character.CharSkills[sn].IsCooldown = true;
                if (Character.CharSkills[sn].statType == Skill.StatType.Mana) { GlobalData.Instance.currentMP_rec *= Character.CharSkills[sn].multiplier; }
                _uiSkill.DeactivateSkill(sn);
                if (fx[sn] != null) fx[sn].SetActive(true);
                IEnumerator endStatRestore = StatRestore(Character.CharSkills[sn].duration, Character.CharSkills[sn].cooldown, sn, (int)Character.CharSkills[sn].statType);
                StartCoroutine(endStatRestore);
                cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].cooldown);
                StartCoroutine(cooldown);
            }
        }
        void SkillItemGiving(int sn) {
            if ((int)Character.CharSkills[sn].skillType == 6 && !Character.CharSkills[sn].IsCooldown) {
                Character.CharSkills[sn].IsCooldown = true;
                _uiSkill.DeactivateSkill(sn);
                string it = Character.CharSkills[sn].items[Mathf.RoundToInt(Random.Range(0.0f, Character.CharSkills[sn].items.GetLength(0) - 1.0f))];
                Character.CharSkills[sn].ItemMultiplier(it, Character.CharSkills[sn].multiplier);
                PickupPopup.Instance.ShowPopupInfo(it, false);
                IEnumerator endItemsGiving = ItemsGiving(Character.CharSkills[sn].cooldown, sn);
                StartCoroutine(endItemsGiving);
                cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].cooldown);
                StartCoroutine(cooldown);
                if (fx[sn] != null) {
                    fx[sn].SetActive(true);
                    IEnumerator fxDisable = DisableObject(fx[sn], 2f);
                    StartCoroutine(fxDisable);
                }
            }
        }
        void SkillFly(int sn, bool active) {
            if (Character.CharSkills[sn].skillType == Skill.SkillType.Fly) {
                if (active && GlobalData.Instance.currentMP >= Character.CharSkills[sn].MP_cost * 5) {
                    Pony.GetComponent<Rigidbody>().useGravity = false;
                    _uiSkill.DeactivateSkill(0); //Deactivate Jump
                    MPDrain = Character.CharSkills[sn].MP_cost;
                    GlobalData.Instance.SPDmlp = GlobalData.Instance.SPDmlp * Character.CharSkills[sn].SPDmlp;
                    GlobalData.Instance.DMGmlp = GlobalData.Instance.DMGmlp * Character.CharSkills[sn].DMGmlp;
                    Pony.GetComponentInChildren<Animator>().SetBool("fly", true);
                    Pony.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    SoundManager.Instance.SetMuteState(Character.CharSkills[sn].sound, false);
                }
                if (!active && Pony.GetComponentInChildren<Animator>().GetBool("fly")) {
                    Pony.GetComponent<Rigidbody>().useGravity = true;
                    Pony.GetComponentInChildren<Animator>().SetBool("fly", false);
                    _uiSkill.ActivateSkill(0);
                    MPDrain = 0;
                    GlobalData.Instance.SPDmlp = GlobalData.Instance.SPDmlp / Character.CharSkills[sn].SPDmlp;
                    GlobalData.Instance.DMGmlp = GlobalData.Instance.DMGmlp / Character.CharSkills[sn].DMGmlp;
                    SoundManager.Instance.SetMuteState(Character.CharSkills[sn].sound, true);
                }
            }
        }
        void SkillSlowMotion(int sn) {
            if (Character.CharSkills[sn].skillType == Skill.SkillType.SlowMotion && !Character.CharSkills[sn].IsCooldown) {
                Character.CharSkills[sn].IsCooldown = true;
                _uiSkill.DeactivateSkill(sn);
                GlobalData.Instance.timeSpeed = Character.CharSkills[sn].multiplier;
                Time.timeScale = Character.CharSkills[sn].multiplier;
                SoundManager.Instance.PlaySound(Character.CharSkills[sn].sound);
                if (fx[sn] != null) fx[sn].SetActive(true);
                IEnumerator endSlowMotion = SlowMotion(Character.CharSkills[sn].duration, Character.CharSkills[sn].cooldown, sn);
                StartCoroutine(endSlowMotion);
                cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].cooldown);
                StartCoroutine(cooldown);
            }
        }
        void SkillConditionStats(int sn, Skill.Condition cond) {
            if (Character.CharSkills[sn].passiveType == Skill.PassiveType.StatMultiplier) {
                //For HPLow skills
                if (Character.CharSkills[sn].condition == Skill.Condition.HPLow) {
                    if (cond == Skill.Condition.HPLow) {
                        if (Character.CharSkills[sn].statType == Skill.StatType.Mana) GlobalData.Instance.currentMP_rec *= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Speed) GlobalData.Instance.SPDmlp *= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Damage) GlobalData.Instance.DMGmlp *= Character.CharSkills[sn].multiplier;
                    }
                    if (cond == Skill.Condition.HPHigh) {
                        if (Character.CharSkills[sn].statType == Skill.StatType.Mana) GlobalData.Instance.currentMP_rec /= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Speed) GlobalData.Instance.SPDmlp /= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Damage) GlobalData.Instance.DMGmlp /= Character.CharSkills[sn].multiplier;
                    }
                }
                //For MPLow skills
                if (Character.CharSkills[sn].condition == Skill.Condition.MPLow) {
                    if (cond == Skill.Condition.MPLow) {
                        if (Character.CharSkills[sn].statType == Skill.StatType.Mana) GlobalData.Instance.currentMP_rec *= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Speed) GlobalData.Instance.SPDmlp *= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Damage) GlobalData.Instance.DMGmlp *= Character.CharSkills[sn].multiplier;
                    }
                    if (cond == Skill.Condition.MPHigh) {
                        if (Character.CharSkills[sn].statType == Skill.StatType.Mana) GlobalData.Instance.currentMP_rec /= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Speed) GlobalData.Instance.SPDmlp /= Character.CharSkills[sn].multiplier;
                        if (Character.CharSkills[sn].statType == Skill.StatType.Damage) GlobalData.Instance.DMGmlp /= Character.CharSkills[sn].multiplier;
                    }
                }
            }
        }
        void SkillMPProtection(int sn) {
            if (Character.CharSkills[sn].skillType == Skill.SkillType.MPProtection && !Character.CharSkills[sn].IsCooldown) {
                Character.CharSkills[sn].IsCooldown = true;
                _uiSkill.DeactivateSkill(sn);
                GlobalData.Instance.isMPProtection = true;
                SoundManager.Instance.PlaySound(Character.CharSkills[sn].sound);
                if (fx[sn] != null) fx[sn].SetActive(true);
                IEnumerator endProtection = DisableProtection(Character.CharSkills[sn].duration, Character.CharSkills[sn].cooldown, sn);
                StartCoroutine(endProtection);
                cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].cooldown);
                StartCoroutine(cooldown);
            }
        }

        #endregion

        #region ENumerators

        //IENumerators
        IEnumerator Dash(float duration, int num) {
            yield return new WaitForSeconds(duration);
            GlobalData.Instance.SPDmlp = GlobalData.Instance.SPDmlp / Character.CharSkills[num].SPDmlp;
            GlobalData.Instance.DMGmlp = GlobalData.Instance.DMGmlp / Character.CharSkills[num].DMGmlp;
            if (fx[num] != null) fx[num].SetActive(false);
            Character.CharSkills[num].IsCooldown = false;
            _uiSkill.ActivateSkill(num);
        }
        IEnumerator StatChange(float duration, int num, int param) {
            yield return new WaitForSeconds(duration);
            if (param == 3) { GlobalData.Instance.SPDmlp = GlobalData.Instance.SPDmlp / Character.CharSkills[num].multiplier; }
            if (param == 4) { GlobalData.Instance.DMGmlp = GlobalData.Instance.DMGmlp / Character.CharSkills[num].multiplier; }
            if (fx[num] != null) fx[num].SetActive(false);
            _uiSkill.ActivateSkill(num);
        }
        IEnumerator StatRestore(float duration, float cooldown, int num, int param) {
            yield return new WaitForSeconds(duration);
            if (param == 1) { GlobalData.Instance.currentMP_rec /= Character.CharSkills[num].multiplier; }
            if (fx[num] != null) fx[num].SetActive(false);
            yield return new WaitForSeconds(cooldown - duration);
            Character.CharSkills[num].IsCooldown = false;
            _uiSkill.ActivateSkill(num);
        }
        IEnumerator ItemsGiving(float cooldown, int num) {
            yield return new WaitForSeconds(cooldown);
            Character.CharSkills[num].IsCooldown = false;
            _uiSkill.ActivateSkill(num);
        }
        IEnumerator SlowMotion(float duration, float cooldown, int num) {
            yield return new WaitForSeconds(duration);
            if (fx[num] != null) fx[num].SetActive(false);
            GlobalData.Instance.timeSpeed = 1;
            Time.timeScale = 1;
            yield return new WaitForSeconds(cooldown - duration);
            Character.CharSkills[num].IsCooldown = false;
            _uiSkill.ActivateSkill(num);
        }
        IEnumerator DisableObject(GameObject obj, float delay) {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
        }
        IEnumerator DisableProtection(float duration, float cooldown, int num) {
            yield return new WaitForSeconds(duration);
            if (fx[num] != null) fx[num].SetActive(false);
            GlobalData.Instance.isMPProtection = false;
            yield return new WaitForSeconds(cooldown - duration);
            Character.CharSkills[num].IsCooldown = false;
            _uiSkill.ActivateSkill(num);
        }

        #endregion

        //Special Action
        void OnTriggerEnter(Collider coll) {
            if (coll.gameObject.GetComponent<OnDestroyFX>() == null) return;
            if ((int)Character.CharSkills[0].passiveType == 0 && Character.CharSkills[0].item == coll.gameObject.GetComponent<OnDestroyFX>().objectType) {
                Character.CharSkills[0].ItemMultiplier(Character.CharSkills[0].item, Character.CharSkills[0].multiplier - 1);
                if (fx[0] != null) {
                    fx[0].SetActive(true);
                    IEnumerator fxDisable = DisableObject(fx[0], 2f);
                    StartCoroutine(fxDisable);
                }
            }
            if ((int)Character.CharSkills[0].passiveType == 1 && Character.CharSkills[0].item == "EoH" && coll.name.Contains("eoh")) {
                //pick random element from array
                string it = Character.CharSkills[0].items[Mathf.RoundToInt(Random.Range(0.0f, Character.CharSkills[0].items.GetLength(0) - 1.0f))];
                //increase element quantity and show popup
                float rnd = Random.Range(0.0f, 1.0f);
                if (rnd < Character.CharSkills[0].chance) {
                    Character.CharSkills[0].ItemMultiplier(it, Character.CharSkills[0].multiplier);
                    PickupPopup.Instance.ShowPopupInfo(it, false);
                }
            }
        }
        void OnCollisionEnter(Collision coll) {
            if (coll.gameObject.GetComponent<OnDestroyFX>() == null) return;
            if ((int)Character.CharSkills[0].passiveType == 1 && Character.CharSkills[0].item == coll.gameObject.GetComponent<OnDestroyFX>().objectType) {
                //pick random element from array
                string it = Character.CharSkills[0].items[Mathf.RoundToInt(Random.Range(0.0f, Character.CharSkills[0].items.GetLength(0) - 1.0f))];
                //increase element quantity and show popup
                float rnd = Random.Range(0.0f, 1.0f);
                if (rnd < Character.CharSkills[0].chance) {
                    Character.CharSkills[0].ItemMultiplier(it, Character.CharSkills[0].multiplier);
                    PickupPopup.Instance.ShowPopupInfo(it, false);
                }
            }
        }
    }
}
