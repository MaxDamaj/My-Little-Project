using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MLA.Gameplay.Scenes;

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

[Serializable]
public class Endurance {
    public TerrainSpawner terrain;
    public Difficulty difficulty;
}

namespace MLA.System {
    public class DBChallenges : MonoBehaviour {

        [SerializeField]
        List<Challenge> _challegesList = null;
        [SerializeField]
        List<Endurance> _enduranceList = null;

        public Challenge GetChallenge(int index) {
            return _challegesList[index];
        }
        public int GetChallengesCount() {
            return _challegesList.Count;
        }

        public Endurance GetEndurance(Difficulty diff) {
            return _enduranceList.Find(x => x.difficulty == diff);
        }
    }
}
