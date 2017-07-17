using UnityEngine;
using System.Collections;

public class PonyFreeMoveController : MonoBehaviour {

    public Transform ponyBody;

    //private SoundManager SM;

    private CharsFMData _pony;
    private Rigidbody _rigidbody;
    private Animator anim;
    private float jump_recover;
    private float m_shift;
	private Transform _start_position;

    void Start() {
        //SM = FindObjectOfType<SoundManager>();
        _rigidbody = GetComponent<Rigidbody>();
		_start_position = _rigidbody.transform;

        _pony = Database.Instance.GetCharFMInfo(Database.Instance.SelectedPony);

        anim = GetComponentInChildren<Animator>();

        jump_recover = 20;
    }

    void FixedUpdate() {

        if (jump_recover > 0) { jump_recover--; }
        //Scale to another direction
        if (Input.GetAxis("Horizontal") > 0.01f) {
            ponyBody.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }
        if (Input.GetAxis("Horizontal") < -0.01f) {
            ponyBody.localScale = new Vector3(0.55f, 0.55f, -0.55f);
        }
        //Moving
        _rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal")*3, _rigidbody.velocity.y, m_shift);
        anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        if (anim.GetBool("ground")) {
            m_shift = Input.GetAxis("Vertical") * 3 * Input.GetAxis("Horizontal") * ponyBody.localScale.z;
        } else {
            m_shift = Input.GetAxis("Vertical") * 2;
        }
        
        //Jumping
        if (Input.GetAxis("Jump") > 0.01f && jump_recover <= 0) {
            if (anim.GetBool("ground")) {
                jump_recover = 20;
                anim.SetBool("ground", false);
                _rigidbody.AddForce(new Vector3(0f, 220f, 0f));
                //SM.SetMuteState("a_run", true);
            }
        }
    }

    //----Colliders-Work----------------------------------------
    void OnCollisionStay(Collision coll) {
        //Walls Collider fix
        if (coll.gameObject.tag == "FarWall" && m_shift > 0f) m_shift = 0;
        if (coll.gameObject.tag == "NearWall" && m_shift < 0f) m_shift = 0;
        //Fall down
		if (coll.gameObject.tag == "Bottom") {
			gameObject.transform.position = _start_position.position;
		}
    }

    void OnCollisionEnter(Collision coll) {
        switch (coll.gameObject.tag) {
            case "Ground":
                anim.SetBool("ground", true);
                //SM.SetMuteState("a_run", false);
                //SM.PlaySound("a_landing");
                break;
        }
    }
}
