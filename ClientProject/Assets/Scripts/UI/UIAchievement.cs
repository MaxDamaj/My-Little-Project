using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIAchievement : MonoBehaviour {

    [Header("UI")]
    public Text title;
    public Text description;
    public Text progressText;
    public Image progressBar;
    public Text takenText;
    [Header("Reward")]
    public GameObject rewardGameobject;
    public Image[] rewardIcons;
    public Text[] rewardTexts;

    [Header("Common")]
    public int ID;
    private Achievement achievement;

    void Start() {
        Database.onRefresh += RefreshUI;
        achievement = DBAchievements.Instance.GetAchievement(ID);

        rewardIcons[0].gameObject.SetActive(false);
        rewardIcons[1].gameObject.SetActive(false);
        rewardIcons[2].gameObject.SetActive(false);
        rewardTexts[0].gameObject.SetActive(false);
        rewardTexts[1].gameObject.SetActive(false);
        rewardTexts[2].gameObject.SetActive(false);

        if (achievement.reward.res1 != string.Empty) {
            rewardIcons[0].gameObject.SetActive(true);
            rewardTexts[0].gameObject.SetActive(true);
            rewardIcons[0].sprite = Database.Instance.GetItemIcon(achievement.reward.res1);
            rewardTexts[0].text = "" + achievement.reward.quan1;
        }
        if (achievement.reward.res2 != string.Empty) {
            rewardIcons[1].gameObject.SetActive(true);
            rewardTexts[1].gameObject.SetActive(true);
            rewardIcons[1].sprite = Database.Instance.GetItemIcon(achievement.reward.res2);
            rewardTexts[1].text = "" + achievement.reward.quan2;
        }
        if (achievement.reward.res3 != string.Empty) {
            rewardIcons[2].gameObject.SetActive(true);
            rewardTexts[2].gameObject.SetActive(true);
            rewardIcons[2].sprite = Database.Instance.GetItemIcon(achievement.reward.res3);
            rewardTexts[2].text = "" + achievement.reward.quan3;
        }

        Invoke("RefreshUI", 1f);
    }

    void RefreshUI() {
        title.text = achievement.title;
        description.text = achievement.description;
        if (Database.Instance.takenAchievements[ID] > 0) {
            progressText.gameObject.SetActive(false);
            takenText.gameObject.SetActive(true);
            rewardGameobject.SetActive(false);
            title.color = Database.COLOR_GREEN;
            progressBar.fillAmount = 1;
            progressBar.color = Database.COLOR_GREEN;
        } else {
            progressText.gameObject.SetActive(true);
            takenText.gameObject.SetActive(false);
            rewardGameobject.SetActive(true);
            title.color = Color.white;
            progressBar.fillAmount = AchievementsController.Instance.ReturnAchievementProgress(achievement.condition, achievement.value);
            progressBar.color = Database.COLOR_GREY;
            progressText.text = Mathf.RoundToInt(progressBar.fillAmount * achievement.value) + "/" + achievement.value;
        }
    }

    void OnDestroy() {
        Database.onRefresh -= RefreshUI;
    }

}
