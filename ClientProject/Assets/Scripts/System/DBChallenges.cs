using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ChallType {
    ShortRun, LongRun, Mania, CardGame
};
public enum ChallModifier {
    None, MPSlow
};

[Serializable]
public class Challenge {
    public string title;
    public Sprite bg;
    public ChallType challengeType;
    public ChallModifier challengeModifier;
	public int externalIndex;
    public float distance;
    public float timeRestr;
    public PonyType charRestr;
    public int PSRestr;
    public StorageData[] startFee;
    public StorageData[] reward;
    public GameObject[] roads;
    public GameObject border;
    public GameObject[] bonuses;
    public GameObject[] packs;
    public int packsSpawnDelay;
}

[Serializable]
public class CardChallenge {
	public string enemyName;
	public Sprite enemyIcon;
	public int playerLUV;
	public int enemyLUV;
	public string[] enemyDeck;
	public CardsSpawn tradeRowType;
	public bool endlessOption;
}

public class DBChallenges : MonoBehaviour {

    [SerializeField]
    List<Challenge> _challegesList = null;
	[SerializeField]
	List<CardChallenge> _cardChallengesList = null;

    public Challenge GetChallenge(int index) {
        return _challegesList[index];
    }

    public int GetChallengesCount() {
        return _challegesList.Count;
    }

	public CardChallenge GetCardChallenge(int index) {
		return _cardChallengesList[_challegesList[index].externalIndex];
	}

}
