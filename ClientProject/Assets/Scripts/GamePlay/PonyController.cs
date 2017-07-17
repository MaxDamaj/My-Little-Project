using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PonyController : MonoBehaviour {

    private SoundManager SM;
    private EndModeController _emc;
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
        SM = FindObjectOfType<SoundManager>();
        _cpf = FindObjectOfType<CameraPonyFollow>();
        _emc = FindObjectOfType<EndModeController>();
        _rigidbody = GetComponent<Rigidbody>();
        _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        anim = GetComponentInChildren<Animator>();
        jump_recover = 20;
    }

    void FixedUpdate() {
        if (jump_recover > 0) { jump_recover--; }
        //Movement
        _rigidbody.velocity = new Vector3((_pony.SPD * _emc.SPDmlp) / 10, _rigidbody.velocity.y, m_shift - 0.015f);
        if (s_shift == 0) {
            m_shift = Input.GetAxis("Vertical") * (_pony.SPD * _emc.SPDmlp) / 10 / 2;
        } else {
            m_shift = s_shift * (_pony.SPD * _emc.SPDmlp) / 10 / 2;
        }
        //Jumping
        if (Input.GetAxis("Jump") > 0.01f && jump_recover <= 0) {
            Jump();
        }
    }

    #endregion

    public void Jump() {
        if (anim.GetBool("ground")) {
            jump_recover = 20;
            anim.SetBool("ground", false);
            _rigidbody.AddForce(new Vector3(0f, 220f, 0f));
            SM.SetMuteState("a_run", true);
        }
    }
    public void SetShift(float value) {
        s_shift = value;
    }

    //----Colliders-Work----------------------------------------
    void OnCollisionStay(Collision coll) {
        //Walls Collider fix
        if (coll.gameObject.tag == "FarWall" && m_shift > 0f) m_shift = 0;
        if (coll.gameObject.tag == "NearWall" && m_shift < 0f) m_shift = 0;
        //Fall down
        if (coll.gameObject.tag == "Bottom") _emc.currentHP = 0;
    }

    //Bonuses picking up
    void OnTriggerEnter(Collider coll) {
        //Run pickup event
        if (coll.gameObject.name != "trigger") onPlayerPickup(gameObject.tag, coll.gameObject);
        //Check target tag
        switch (coll.gameObject.tag) {
            case "Bit":
                SM.PlaySound("a_coins");
                _emc.ShowPopupInfo("Bits");
                Database.Instance.IncreaseItemQuantity("Bits", 1);
                break;
            case "EoH_Laughter":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Laughter");
                Database.Instance.IncreaseItemQuantity("Laughter", 1);
                break;
            case "EoH_Generosity":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Generosity");
                Database.Instance.IncreaseItemQuantity("Generosity", 1);
                break;
            case "EoH_Honesty":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Honesty");
                Database.Instance.IncreaseItemQuantity("Honesty", 1);
                break;
            case "EoH_Kindness":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Kindness");
                Database.Instance.IncreaseItemQuantity("Kindness", 1);
                break;
            case "EoH_Loyalty":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Loyalty");
                Database.Instance.IncreaseItemQuantity("Loyalty", 1);
                break;
            case "EoH_Magic":
                SM.PlaySound("a_beeps");
                _emc.ShowPopupInfo("Magic");
                Database.Instance.IncreaseItemQuantity("Magic", 1);
                break;
        }
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Ground":
                anim.SetBool("ground", true);
                SM.SetMuteState("a_run", false);
                SM.PlaySound("a_landing");
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
            SM.PlaySound("a_thump"); _emc.currentHP -= damage * _emc.DMGmlp;
            _cpf.shake_intensity = camShaking * _emc.DMGmlp;
            Database.Instance.obstWithDamage++;
        } else {
            Database.Instance.obstNonDamage++;
        }
    }
}
