using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementsController : MonoBehaviour {

    public RectTransform container;
    public UIAchievement sample;

    private static AchievementsController controller;

    #region API

    public static AchievementsController Instance {
        get {
            if (controller == null) {
                controller = FindObjectOfType<AchievementsController>();
            }
            return controller;
        }
    }

    void Start() {
        //Place achievements
        for (int i = 0; i < DBAchievements.Instance.GetAchievementsCount(); i++) {
            GameObject tmp = Instantiate(sample.gameObject);
            tmp.transform.SetParent(container);
            tmp.transform.position = Vector3.zero;
            tmp.transform.rotation = Quaternion.identity;
            tmp.transform.localScale = Vector3.one;
            tmp.GetComponent<UIAchievement>().ID = i;
        }
        Invoke("CheckStates", 0.8f);
    }

    #endregion

    public void CheckStates() {
        for (int i = 0; i < DBAchievements.Instance.GetAchievementsCount(); i++) {
            Achievement achievement = DBAchievements.Instance.GetAchievement(i);
            if (ReturnAchievementProgress(achievement.condition, achievement.value) >= 1 && Database.Instance.takenAchievements[i] == 0) {
                //Give Reward card
                if (achievement.reward.res1 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res1, achievement.reward.quan1); }
                if (achievement.reward.res2 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res2, achievement.reward.quan2); }
                if (achievement.reward.res3 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res3, achievement.reward.quan3); }
                //Show message
                Database.Instance.takenAchievements[i] = 1;
                UIMessageWindow.Instance.ShowMessage("You are gain achievement -" + achievement.title + "-", 0, UIAction.nothing, true, false);
            }
        }
        Invoke("SortAchievements", 0.5f);
    }

    public float ReturnAchievementProgress(AchieveType type, float value) {
        switch (type) {
            case AchieveType.TotalDist:
                return Database.Instance.distTotal / value;
            case AchieveType.EndDist:
                return Database.Instance.distEnd / value;
            case AchieveType.ChallDist:
                return Database.Instance.distChall / value;
            case AchieveType.TotalObst:
                return Database.Instance.obstTotal / value;
            case AchieveType.DamObst:
                return Database.Instance.obstWithDamage / value;
            case AchieveType.NonDamObst:
                return Database.Instance.obstNonDamage / value;
            case AchieveType.TotalPS:
                return UIPSCalculation.PartyStrength / value;
            case AchieveType.CharsOwned:
                return Database.Instance.GetUnlockedCharsCount() / value;
            case AchieveType.CraftedComps:
                return Database.Instance.craftedComps / value;
        }
        return 0;
    }

    public void SortAchievements() {
        List<UIAchievement> achList = new List<UIAchievement>();

        //Show inprogress achievements first
        achList.AddRange(container.GetComponentsInChildren<UIAchievement>());
        achList.RemoveAll(x => x.progressBar.fillAmount == 1);
        float maxValue = 0;
        for (int i = 0; i < achList.Count; i++) {
            if (achList[i].progressBar.fillAmount > maxValue) {
                maxValue = achList[i].progressBar.fillAmount;
                achList[i].transform.SetAsFirstSibling();
            }
        }

        //Show taken achievements last
        achList = new List<UIAchievement>();
        achList.AddRange(container.GetComponentsInChildren<UIAchievement>());
        achList.RemoveAll(x => x.progressBar.fillAmount < 1);
        for (int i = 0; i < achList.Count; i++) {
            achList[i].transform.SetAsLastSibling();
        }
    }



}
