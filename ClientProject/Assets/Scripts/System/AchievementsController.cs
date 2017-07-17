using UnityEngine;
using System.Collections;

public class AchievementsController : MonoBehaviour {

	[SerializeField]
	private DBAchievements DBA = null;

	public UIMessageWindow mWindow;

	[Header("Card Game containers")]
	public Transform playerStack;

	void Start () {
		Invoke("CheckStates", 1);
	}

	void CheckStates() {
		for (int i = 0; i < DBA.GetAchievementsCount(); i++) {
			Achievement achievement = DBA.GetAchievement(i);
			if (ReturnAchievementProgress(achievement.condition, achievement.value) >= 1 && Database.Instance.takenAchievements[i] == 0) {
				//Give Reward card
				GameObject tmp = Instantiate(Resources.Load<GameObject>("Cards/"+achievement.reward));
				tmp.transform.SetParent(playerStack);
				tmp.transform.localScale = Vector3.one;
				tmp.transform.rotation = playerStack.rotation;
				tmp.name = achievement.reward;
				//Show message
				Database.Instance.takenAchievements[i] = 1;
				mWindow.ShowMessage("You are gain achievement -"+achievement.title+"-", 0, UIMessageWindow.Action.nothing, true, false);
			}
		}
	}

	public float ReturnAchievementProgress(AchieveType type, float value) {
		switch (type) {
		case AchieveType.TotalDist:
			return Database.Instance.distTotal/value;
		case AchieveType.EndDist:
			return Database.Instance.distEnd/value;
		case AchieveType.ChallDist:
			return Database.Instance.distChall/value;
		case AchieveType.TotalObst:
			return Database.Instance.obstTotal/value;
		case AchieveType.DamObst:
			return Database.Instance.obstWithDamage/value;
		case AchieveType.NonDamObst:
			return Database.Instance.obstNonDamage/value;
		case AchieveType.CharsOwned:
			return Database.Instance.GetUnlockedCharsCount()/value;
		}
		return 0;
	}


}
