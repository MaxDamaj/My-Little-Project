using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum AchieveType {
	Nothing, TotalDist, EndDist, ChallDist, TotalObst, DamObst, NonDamObst, PassChallInTotal, TotalPS, CharsOwned
};

[Serializable]
public class Achievement {
	public string title;
	public string description;
	public AchieveType condition;
	public float value;
	public string reward;
}

public class DBAchievements : MonoBehaviour {

	[SerializeField]
	List<Achievement> _achievementsList = null;

	public Achievement GetAchievement(int index) {
		return _achievementsList[index];
	}
	public int GetAchievementsCount() {
		return _achievementsList.Count;
	}


}
