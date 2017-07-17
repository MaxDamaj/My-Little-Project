using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIAchievement : MonoBehaviour {

	[SerializeField]
	private DBAchievements DBA = null;
	[SerializeField]
	AchievementsController controller = null;

	[Header("UI")]
	public Text title;
	public Text description;
	public Text progressText;
	public Image progressBar;
	public Image rewardImage;
	public Text takenText;

	[Header("Common")]
	public int ID;
	public Color inProgressColor;
	public Color takenColor;
	private Achievement achievement;
	private Sprite cardIcon;

	void Start () {
		Database.onRefresh += RefreshUI;
		achievement = DBA.GetAchievement(ID);
		cardIcon = Resources.Load<Image>("Cards/" + achievement.reward).sprite;
		Invoke("RefreshUI", 0.5f);
	}

	void RefreshUI() {
		title.text = achievement.title;
		description.text = achievement.description;
		rewardImage.sprite = cardIcon;
		if (Database.Instance.takenAchievements[ID] > 0) {
			progressText.gameObject.SetActive(false);
			takenText.gameObject.SetActive(true);
			title.color = takenColor;
			progressBar.fillAmount = 1;
			progressBar.color = takenColor;
		} else {
			progressText.gameObject.SetActive(true);
			takenText.gameObject.SetActive(false);
			title.color = Color.white;
			progressBar.fillAmount = controller.ReturnAchievementProgress(achievement.condition, achievement.value);
			progressBar.color = inProgressColor;
			progressText.text = Mathf.RoundToInt(progressBar.fillAmount * achievement.value) + "/" + achievement.value;
		}
	}

	void OnDestroy() {
		Database.onRefresh -= RefreshUI;
	}
		
}
