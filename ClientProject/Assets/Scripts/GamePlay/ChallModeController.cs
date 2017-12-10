using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChallModeController : MonoBehaviour {

    [SerializeField]
    DBChallenges DBC = null;

    public GameObject finishLine;

    private EndModeController _emc;
    private Transform player;
    private Challenge challenge;
    private bool IsComplete;

    void Start() {
        MusicManager.Instance.SetFolder("Music/Challenges", 2);

        _emc = FindObjectOfType<EndModeController>();
        Invoke("FindPony", 0.4f);
        challenge = DBC.GetChallenge(GlobalData.Instance.nowChallenge);
        IsComplete = false;
        //finishLine.transform.position = new Vector3(challenge.distance / 2, 0f, 0f);
    }

    void FindPony() {
        player = PonyController.Instance.transform;
    }

    void GainReward() {
        if (!IsComplete) {
            foreach (var item in challenge.reward) {
                Database.Instance.IncreaseItemQuantity(item.ItemName, item.ItemQuantity);
            }
        }
    }

    void Update() {
        if (player != null) {
            //_emc.distance.text = "" + Mathf.RoundToInt(player.position.x * 2) + "/" + challenge.distance;
            //Pass Check
            /*if (player.position.x * 2 >= challenge.distance && !IsComplete) {
                Database.Instance.passedChallenges[GlobalData.Instance.nowChallenge]++;
                GainReward();
                IsComplete = true;
                _emc.ShowPassWindow();
            }*/
        }
    }


}