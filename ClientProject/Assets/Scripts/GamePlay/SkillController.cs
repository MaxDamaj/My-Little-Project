using UnityEngine;
using System.Collections;

public class SkillController : MonoBehaviour {

    [SerializeField]
    GameObject[] fx = null;

    private GameObject Pony;
    private EndModeController _emc;
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
        Character = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        Pony = PonyController.Instance.gameObject;
        _emc = FindObjectOfType<EndModeController>();
        _uiSkill = FindObjectOfType<UISkills>();
        //Set all skilt to execute state
        for (int i = 0; i < Character.CharSkills.GetLength(0); i++) {
            Character.CharSkills[i].IsCooldown = false;
            _uiSkill.ActivateSkill(i);
        }
        _emc.CharacterState = SkillConditionStats;
        MPDrain = 0;
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
        SkillProjectile(1, 2);
        SkillFly(1, true);
    }
    public void SkillXHold() {
        _emc.currentMP -= MPDrain;
        if (_emc.currentMP <= 0) {
            _emc.currentMP = 0;
            SkillFly(1, false);
        }
        if (holdTimer <= 0) {
            SkillProjectile(1, 2);
        }
    }
    public void SkillXUp() {
        SkillFly(1, false);
    }
    public void SkillYDown() {
        SkillStatChange(2);
        SkillProjectile(2, 3);
    }
    public void SkillBDown() {
        SkillRestoreStat(3);
        SkillItemGiving(3);
        SkillSlowMotion(3);
    }

    #region Skills

    void SkillDash(int sn) {
        if ((int)Character.CharSkills[sn].skillType == 1 && _emc.currentMP >= Character.CharSkills[sn].MP_cost) {
            SoundManager.Instance.PlaySound("a_dash");
            _emc.currentMP -= Character.CharSkills[sn].MP_cost;
            _emc.SPDmlp = _emc.SPDmlp * Character.CharSkills[sn].SPDmlp;
            _emc.DMGmlp = _emc.DMGmlp * Character.CharSkills[sn].DMGmlp;
            _uiSkill.DeactivateSkill(sn);
            if (fx[sn] != null) fx[sn].SetActive(true);
            IEnumerator endDash = Dash(Character.CharSkills[sn].duration, sn);
            StartCoroutine(endDash);
            cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].duration);
            StartCoroutine(cooldown);
        }
    }
    void SkillProjectile(int sn, float lifetime) {
        if ((int)Character.CharSkills[sn].skillType == 3 && _emc.currentMP >= Character.CharSkills[sn].MP_cost) {
            GameObject spark_cl;
            Vector3 v3 = new Vector3(Pony.transform.position.x + 0.75f, Pony.transform.position.y, Pony.transform.position.z);
            if (Character.CharSkills[sn].projType == Skill.ProjectileType.Autofire) {
                SoundManager.Instance.PlaySound("a_gun");
                v3 = new Vector3(Pony.transform.position.x + 0.7f, Pony.transform.position.y - 0.1f, Pony.transform.position.z);
                holdTimer = Character.CharSkills[sn].duration;
            } else {
                SoundManager.Instance.PlaySound("a_shoot");
            }
            if (fx[sn] != null) {
                fx[sn].SetActive(true);
                IEnumerator fxDisable = DisableObject(fx[sn], 0.15f);
                StartCoroutine(fxDisable);
            }
            _emc.currentMP -= Character.CharSkills[sn].MP_cost;
            spark_cl = (GameObject)Instantiate(Character.CharSkills[sn].obj, v3, Character.CharSkills[sn].obj.transform.rotation);
            if (Character.CharSkills[sn].projType == Skill.ProjectileType.Light)
                spark_cl.GetComponent<Rigidbody>().AddForce(new Vector3(250f, 0f, 0f));
            if (Character.CharSkills[sn].projType == Skill.ProjectileType.Heavy)
                spark_cl.GetComponent<Rigidbody>().AddForce(new Vector3(400f, 100f, 0f));
            if (Character.CharSkills[sn].projType == Skill.ProjectileType.Autofire)
                spark_cl.GetComponent<Rigidbody>().AddForce(new Vector3(100f, 0f, 0f));
            Destroy(spark_cl, lifetime);
        }
    }
    void SkillStatChange(int sn) {
        if ((int)Character.CharSkills[sn].skillType == 4 && _emc.currentMP >= Character.CharSkills[sn].MP_cost) {
            _emc.currentMP -= Character.CharSkills[sn].MP_cost;
            if (Character.CharSkills[sn].statType == Skill.StatType.Health) { _emc.currentHP += Character.CharSkills[sn].multiplier; }
            if (Character.CharSkills[sn].statType == Skill.StatType.Mana) { _emc.currentMP = Character.MP * Character.CharSkills[sn].multiplier; }
            if (Character.CharSkills[sn].statType == Skill.StatType.Stamina) { Database.Instance.IncreaseCurrSTM(Database.Instance.SelectedPony, Character.CharSkills[sn].multiplier); }
            if ((int)Character.CharSkills[sn].statType == 3) { _emc.SPDmlp *= Character.CharSkills[sn].multiplier; }
            if ((int)Character.CharSkills[sn].statType == 4) { _emc.DMGmlp *= Character.CharSkills[sn].multiplier; }
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
            if (Character.CharSkills[sn].statType == Skill.StatType.Mana) { _emc.currentMP_rec *= Character.CharSkills[sn].multiplier; }
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
            _emc.ShowPopupInfo(it);
            IEnumerator endItemsGiving = ItemsGiving(Character.CharSkills[sn].cooldown, sn);
            StartCoroutine(endItemsGiving);
            cooldown = _uiSkill.StartCooldown(sn, (int)Character.CharSkills[sn].cooldown);
            StartCoroutine(cooldown);
        }
    }
    void SkillFly(int sn, bool active) {
        if (Character.CharSkills[sn].skillType == Skill.SkillType.Fly) {
            if (active && _emc.currentMP >= Character.CharSkills[sn].MP_cost * 5) {
                Pony.GetComponent<Rigidbody>().useGravity = false;
                _uiSkill.DeactivateSkill(0); //Deactivate Jump
                MPDrain = Character.CharSkills[sn].MP_cost;
                _emc.SPDmlp = _emc.SPDmlp * Character.CharSkills[sn].SPDmlp;
                _emc.DMGmlp = _emc.DMGmlp * Character.CharSkills[sn].DMGmlp;
                Pony.GetComponentInChildren<Animator>().SetBool("fly", true);
                Pony.GetComponent<Rigidbody>().velocity = Vector3.zero;
                SoundManager.Instance.SetMuteState("a_wings", false);
            }
            if (!active && Pony.GetComponentInChildren<Animator>().GetBool("fly")) {
                Pony.GetComponent<Rigidbody>().useGravity = true;
                Pony.GetComponentInChildren<Animator>().SetBool("fly", false);
                _uiSkill.ActivateSkill(0);
                MPDrain = 0;
                _emc.SPDmlp = _emc.SPDmlp / Character.CharSkills[sn].SPDmlp;
                _emc.DMGmlp = _emc.DMGmlp / Character.CharSkills[sn].DMGmlp;
                SoundManager.Instance.SetMuteState("a_wings", true);
            }
        }
    }
    void SkillSlowMotion(int sn) {
        if (Character.CharSkills[sn].skillType == Skill.SkillType.SlowMotion && !Character.CharSkills[sn].IsCooldown) {
            Character.CharSkills[sn].IsCooldown = true;
            _uiSkill.DeactivateSkill(sn);
            _emc.timeSpeed = Character.CharSkills[sn].multiplier;
            Time.timeScale = Character.CharSkills[sn].multiplier;
            if (fx[sn] != null) fx[sn].SetActive(true);
            SoundManager.Instance.PlaySound("a_slowdown");
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
                    if (Character.CharSkills[sn].statType == Skill.StatType.Mana) _emc.currentMP_rec *= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Speed) _emc.SPDmlp *= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Damage) _emc.DMGmlp *= Character.CharSkills[sn].multiplier;
                }
                if (cond == Skill.Condition.HPHigh) {
                    if (Character.CharSkills[sn].statType == Skill.StatType.Mana) _emc.currentMP_rec /= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Speed) _emc.SPDmlp /= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Damage) _emc.DMGmlp /= Character.CharSkills[sn].multiplier;
                }
            }
            //For MPLow skills
            if (Character.CharSkills[sn].condition == Skill.Condition.MPLow) {
                if (cond == Skill.Condition.MPLow) {
                    if (Character.CharSkills[sn].statType == Skill.StatType.Mana) _emc.currentMP_rec *= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Speed) _emc.SPDmlp *= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Damage) _emc.DMGmlp *= Character.CharSkills[sn].multiplier;
                }
                if (cond == Skill.Condition.MPHigh) {
                    if (Character.CharSkills[sn].statType == Skill.StatType.Mana) _emc.currentMP_rec /= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Speed) _emc.SPDmlp /= Character.CharSkills[sn].multiplier;
                    if (Character.CharSkills[sn].statType == Skill.StatType.Damage) _emc.DMGmlp /= Character.CharSkills[sn].multiplier;
                }
            }
        }
    }

    #endregion

    #region ENumerators

    //IENumerators
    IEnumerator Dash(float duration, int num) {
        yield return new WaitForSeconds(duration);
        _emc.SPDmlp = _emc.SPDmlp / Character.CharSkills[num].SPDmlp;
        _emc.DMGmlp = _emc.DMGmlp / Character.CharSkills[num].DMGmlp;
        if (fx[num] != null) fx[num].SetActive(false);
        _uiSkill.ActivateSkill(num);
    }
    IEnumerator StatChange(float duration, int num, int param) {
        yield return new WaitForSeconds(duration);
        if (param == 3) { _emc.SPDmlp = _emc.SPDmlp / Character.CharSkills[num].multiplier; }
        if (param == 4) { _emc.DMGmlp = _emc.DMGmlp / Character.CharSkills[num].multiplier; }
        if (fx[num] != null) fx[num].SetActive(false);
        _uiSkill.ActivateSkill(num);
    }
    IEnumerator StatRestore(float duration, float cooldown, int num, int param) {
        yield return new WaitForSeconds(duration);
        if (param == 1) { _emc.currentMP_rec /= Character.CharSkills[num].multiplier; }
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
        _emc.timeSpeed = 1;
        Time.timeScale = 1;
        yield return new WaitForSeconds(cooldown - duration);
        Character.CharSkills[num].IsCooldown = false;
        _uiSkill.ActivateSkill(num);
    }
    IEnumerator DisableObject(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    #endregion

    //Special Action
    void OnTriggerEnter(Collider coll) {
        if ((int)Character.CharSkills[0].passiveType == 0 && "EoH_" + Character.CharSkills[0].item == coll.gameObject.tag) {
            Character.CharSkills[0].ItemMultiplier(Character.CharSkills[0].item, Character.CharSkills[0].multiplier - 1);
        }
    }
    void OnCollisionEnter(Collision coll) {
        if ((int)Character.CharSkills[0].passiveType == 1 && Character.CharSkills[0].item == coll.gameObject.tag) {
            //pick random element from array
            string it = Character.CharSkills[0].items[Mathf.RoundToInt(Random.Range(0.0f, Character.CharSkills[0].items.GetLength(0) - 1.0f))];
            //increase element quantity and show popup
            Character.CharSkills[0].ItemMultiplier(it, Character.CharSkills[0].multiplier);
            _emc.ShowPopupInfo(it);
        }
    }  

}
