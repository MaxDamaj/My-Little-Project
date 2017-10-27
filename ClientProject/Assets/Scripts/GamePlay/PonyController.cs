using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PonyController : MonoBehaviour {

    private CameraPonyFollow _cpf;

    private CharsFMData _pony;
    private Rigidbody _rigidbody;
    private Animator anim;
    private float jump_recover;
    private float m_shift;
    private float s_shift;
    private static PonyController controller;

    public delegate void PickupArgs(string tag, GameObject target);
    public static event PickupArgs onPlayerPickup; // event for picking up items

    #region API

    public static PonyController Instance {
        get {
            if (controller == null) {
                controller = FindObjectOfType<PonyController>();
            }
            return controller;
        }
    }

    void Start() {
        _cpf = FindObjectOfType<CameraPonyFollow>();
        _rigidbody = GetComponent<Rigidbody>();
        _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        anim = GetComponentInChildren<Animator>();
        jump_recover = 20;
    }

    void FixedUpdate() {
        if (jump_recover > 0) { jump_recover--; }
        //Movement
        _rigidbody.velocity = new Vector3((_pony.SPD * GlobalData.Instance.SPDmlp) / 10, _rigidbody.velocity.y, m_shift - 0.015f);
        if (s_shift == 0) {
            m_shift = Input.GetAxisRaw("Vertical") * (_pony.SPD * GlobalData.Instance.SPDmlp) / 10 / 2;
        } else {
            m_shift = s_shift * (_pony.SPD * GlobalData.Instance.SPDmlp) / 10 / 2;
        }
        //Jumping
        if (Input.GetAxis("Jump") > 0.01f && jump_recover <= 0) {
            Jump();
        }
    }

    #endregion

    #region Colliders

    void OnCollisionStay(Collision coll) {
        //Walls Collider fix
        if (coll.gameObject.tag == "FarWall" && m_shift > 0f) m_shift = 0;
        if (coll.gameObject.tag == "NearWall" && m_shift < 0f) m_shift = 0;
        //Fall down
        if (coll.gameObject.tag == "Bottom") GlobalData.Instance.currentHP = 0;
    }

    //Bonuses picking up
    void OnTriggerEnter(Collider coll) {
        //Run pickup event
        if (coll.gameObject.name != "trigger") onPlayerPickup(gameObject.tag, coll.gameObject);
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Ground":
                anim.SetBool("ground", true);
                SoundManager.Instance.SetMuteState("a_run", false);
                SoundManager.Instance.PlaySound("a_landing");
                break;
            case "Hay":
                CalculateObstacle(coll.gameObject.transform, 0.3f, 1f, 0.05f);
                break;
            case "WoodCrate":
                CalculateObstacle(coll.gameObject.transform, 0.3f, 2f, 0.1f);
                break;
            case "Rail":
                CalculateObstacle(coll.gameObject.transform, 0.3f, 4f, 0.2f);
                break;
            case "Firebarrel":
                CalculateObstacle(coll.gameObject.transform, 0.2f, 10f, 0.2f);
                break;
        }
    }

    void CalculateObstacle(Transform obstacle, float shift, float damage, float camShaking) {
        Database.Instance.obstTotal++;
        if (obstacle.position.x - transform.position.x > shift) {
            //SoundManager.Instance.PlaySound("a_thump");
            if (GlobalData.Instance.isMPProtection && GlobalData.Instance.currentMP >= damage) {
                GlobalData.Instance.currentMP -= damage * GlobalData.Instance.DMGmlp;
            } else {
                GlobalData.Instance.currentHP -= damage * GlobalData.Instance.DMGmlp;
            }
            _cpf.shake_intensity = camShaking * GlobalData.Instance.DMGmlp;
            if (!SkillController.Instance.IsSimulation) Database.Instance.obstWithDamage++;
        } else {
            if (!SkillController.Instance.IsSimulation) Database.Instance.obstNonDamage++;
        }
    }

    #endregion

    public void Jump() {
        if (anim.GetBool("ground")) {
            jump_recover = 20;
            anim.SetBool("ground", false);
            _rigidbody.AddForce(new Vector3(0f, 220f, 0f));
            SoundManager.Instance.SetMuteState("a_run", true);
        }
    }
    public void SetShift(float value) {
        s_shift = value;
    }

}
