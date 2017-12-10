using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ChallModifier {
    None, MPSlow
};

[Serializable]
public class Challenge {
    public string title;
    public Sprite bg;
    public Sprite map;
    public ChallModifier challengeModifier;
    public Vector2 timeRestr;
    public PonyType charRestr;
    public int PSRestr;
    public StorageData[] startFee;
    public StorageData[] reward;
    public GameObject terrain;
}

public class DBChallenges : MonoBehaviour {

    [SerializeField]
    List<Challenge> _challegesList = null;

    public Challenge GetChallenge(int index) {
        return _challegesList[index];
    }

    public int GetChallengesCount() {
        return _challegesList.Count;
    }

}
