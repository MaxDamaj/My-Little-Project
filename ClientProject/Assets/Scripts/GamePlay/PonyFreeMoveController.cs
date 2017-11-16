using UnityEngine;
using System.Collections;

public class PonyFreeMoveController : MonoBehaviour {

    public Transform ponyBody;

    public float maxSpeed = 3f;

    //private CharsFMData _pony;
    private Rigidbody _rigidbody;
    private Animator anim;
    private float jump_recover;
    private Vector3 _start_position;
    private Camera _mainCamera;

    private static PonyFreeMoveController controller;

    public delegate void PickupArgs(string tag, GameObject target);
    public static event PickupArgs onPlayerPickup; // event for picking up items

    #region API

    public static PonyFreeMoveController Instance {
        get {
            if (controller == null) {
                controller = FindObjectOfType<PonyFreeMoveController>();
            }
            return controller;
        }
    }

    void Start() {
        SoundManager.Instance.UpdateSoundList();
        _rigidbody = GetComponent<Rigidbody>();
        _start_position = _rigidbody.transform.position;
        _mainCamera = Camera.main;

        //_pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);
        anim = GetComponentInChildren<Animator>();

        jump_recover = 20;
    }

    void FixedUpdate() {
        if (jump_recover > 0) { jump_recover--; }

        //Scale to another direction
        if (Input.GetAxis("Vertical") > 0.01f) {
            ponyBody.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            _mainCamera.transform.localPosition = new Vector3(1.4f, 1.3f, -2.8f);
        }
        if (Input.GetAxis("Vertical") < -0.01f) {
            ponyBody.localScale = new Vector3(0.55f, 0.55f, -0.55f);
            _mainCamera.transform.localPosition = new Vector3(-1.4f, 1.3f, -2.8f);
        }

        //Moving
        if (_rigidbody.velocity.magnitude < maxSpeed) {
            _rigidbody.AddRelativeForce(Input.GetAxisRaw("Vertical") * 0.4f, 0, 0, ForceMode.VelocityChange);
        }
        /*if (Input.GetAxisRaw("Vertical") == 0) {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }*/
        anim.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Vertical")));
        anim.SetFloat("speed", _rigidbody.velocity.sqrMagnitude / 11f);
        _rigidbody.angularVelocity = new Vector3(0, Input.GetAxisRaw("Horizontal") * 1.5f, 0);

        //Jumping
        if (Input.GetAxis("Jump") > 0.01f && jump_recover <= 0) {
            if (anim.GetBool("ground")) {
                jump_recover = 20;
                anim.SetBool("ground", false);
                _rigidbody.AddForce(new Vector3(0f, 220f, 0f));
                SoundManager.Instance.SetMuteState("a_run", true);
            }
        }

        //Sound of running
        if (!anim.GetBool("ground")) {
            SoundManager.Instance.SetMuteState("a_run", true);
        } else {
            SoundManager.Instance.SetMuteState("a_run", _rigidbody.velocity.sqrMagnitude < 10);
        }

    }

    #endregion

    #region Colliders

    void OnCollisionStay(Collision coll) {
        //Fall down
        if (coll.gameObject.tag == "Bottom") {
            gameObject.transform.position = _start_position;
        }
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Ground":
                anim.SetBool("ground", true);
                SoundManager.Instance.SetMuteState("a_run", false);
                SoundManager.Instance.PlaySound("a_landing");
                break;
        }
    }

    //Bonuses picking up
    void OnTriggerEnter(Collider coll) {
        //Run pickup event
        if (coll.gameObject.name != "trigger") onPlayerPickup(gameObject.tag, coll.gameObject);
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
            //_cpf.shake_intensity = camShaking * GlobalData.Instance.DMGmlp;
            if (!SkillController.Instance.IsSimulation) Database.Instance.obstWithDamage++;
        } else {
            if (!SkillController.Instance.IsSimulation) Database.Instance.obstNonDamage++;
        }
    }

    #endregion
}
