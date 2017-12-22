using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MLA.UI.Windows;
using MLA.UI.Common;

namespace MLA.System.Controllers {
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
            bool IsNewAchievementGain = false;
            string messageString = "You are gain achievements: ";
            for (int i = 0; i < DBAchievements.Instance.GetAchievementsCount(); i++) {
                Achievement achievement = DBAchievements.Instance.GetAchievement(i);
                if (ReturnAchievementProgress(achievement.condition, achievement.value) >= 1 && Database.Instance.takenAchievements[i] == 0) {
                    //Give Reward card
                    if (achievement.reward.res1 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res1, achievement.reward.quan1); }
                    if (achievement.reward.res2 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res2, achievement.reward.quan2); }
                    if (achievement.reward.res3 != string.Empty) { Database.Instance.IncreaseItemQuantity(achievement.reward.res3, achievement.reward.quan3); }
                    //Show message
                    Database.Instance.takenAchievements[i] = 1;
                    messageString += "\r\n-" + achievement.title + "-";
                    IsNewAchievementGain = true;
                }
            }
            if (IsNewAchievementGain) {
                UIMessageWindow.Instance.ShowMessage(messageString, 0, UIAction.nothing, true, false);
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
            achList.AddRange(container.GetComponentsInChildren<UIAchievement>());
            List<UIAchievement> achTaken = achList.FindAll(x => x.progressBar.fillAmount == 1);
            foreach (var ach in achList) {
                ach.transform.SetParent(container.parent);
            }
            achList.RemoveAll(x => x.progressBar.fillAmount == 1);

            //Show inprogress achievements first
            while (achList.Count > 0) {
                float maxValue = 0;
                int index = 0;
                for (int i = 0; i < achList.Count; i++) {
                    if (achList[i].progressBar.fillAmount > maxValue) {
                        maxValue = achList[i].progressBar.fillAmount;
                        index = i;
                    }
                }
                achList[index].transform.SetParent(container);
                achList.Remove(achList[index]);
            }

            //Show taken achievements last
            foreach (var ach in achTaken) {
                ach.transform.SetParent(container);
            }

        }

    }
}
