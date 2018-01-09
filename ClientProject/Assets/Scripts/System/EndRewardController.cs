using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MLA.System;
using MLA.UI.Windows;

namespace MLA.Gameplay.Controllers {
    public class EndRewardController : MonoBehaviour {

        [Header("UI")]
        public RectTransform container;
        public Button[] diffButtons;
        public UIEndReward prefab;

        private List<UIEndReward> rewards;

        void Start() {
            rewards = new List<UIEndReward>();
            //Gain Rewards for passed distance
            for (int i = 0; i < DBEndRewards.Instance.EasyRewards.GetLength(0); i++) {
                RewardForTask(Difficulty.Easy, Database.Instance.distEndEasy, i);
            }
            diffButtons[0].onClick.AddListener(delegate { ShowPanels(Difficulty.Easy); });
            diffButtons[1].onClick.AddListener(delegate { ShowPanels(Difficulty.Normal); });
            diffButtons[2].onClick.AddListener(delegate { ShowPanels(Difficulty.Hard); });
            //Show rewards for Easy
            ShowPanels(Difficulty.Easy);
        }

        void ShowPanels(Difficulty diff) {
            foreach (var reward in rewards) { Destroy(reward.gameObject); }
            rewards = new List<UIEndReward>();
            List<RunRewards> listRewards = new List<RunRewards>();
            List<int> listPassed = new List<int>();
            foreach (var button in diffButtons) { button.interactable = true; }
            diffButtons[(int)diff - 1].interactable = false;

            switch (diff) {
                case Difficulty.Easy:
                    listRewards.AddRange(DBEndRewards.Instance.EasyRewards);
                    listPassed.AddRange(Database.Instance.endRewardsEasy);
                    break;
                case Difficulty.Normal:
                    listRewards.AddRange(DBEndRewards.Instance.NormalRewards);
                    listPassed.AddRange(Database.Instance.endRewardsNormal);
                    break;
                case Difficulty.Hard:
                    listRewards.AddRange(DBEndRewards.Instance.HardRewards);
                    listPassed.AddRange(Database.Instance.endRewardsHard);
                    break;
            }

            //Create reward list
            for (int i = 0; i < listRewards.Count; i++) {
                GameObject tmp = Instantiate(prefab.gameObject);
                tmp.transform.SetParent(container);
                tmp.transform.position = Vector3.zero;
                tmp.transform.rotation = Quaternion.identity;
                tmp.transform.localScale = Vector3.one;
                rewards.Add(tmp.GetComponent<UIEndReward>());
                //Set states
                rewards[i].EndDifficulty = diff;
                rewards[i].distance.text = listRewards[i].distance.ToString();
                rewards[i].distance.color = listPassed[i] == 1 ? Database.COLOR_GREEN : Color.white;
                rewards[i].taken.SetActive(listPassed[i] == 1);
                rewards[i].rewardContainer.gameObject.SetActive(listPassed[i] == 0);
                rewards[i].rewardContainer.text += listRewards[i].stage;
                for (int j = 0; j < listRewards[i].rewardItems.GetLength(0); j++) {
                    rewards[i].rewIcons[j].sprite = Database.Instance.GetItemIcon(listRewards[i].rewardItems[j]);
                    rewards[i].rewIcons[j].gameObject.SetActive(true);
                    rewards[i].rewTexts[j].text = listRewards[i].rewardPrices[j].ToString();
                    rewards[i].rewTexts[j].gameObject.SetActive(true);
                }
            }
        }

        void RewardForTask(Difficulty diff, int distance, int index) {
            switch (diff) {
                case Difficulty.Easy:
                    if (distance >= DBEndRewards.Instance.EasyRewards[index].distance && Database.Instance.endRewardsEasy[index] == 0) {
                        if (DBEndRewards.Instance.EasyRewards[index].stage == "") {
                            for (int i = 0; i < DBEndRewards.Instance.EasyRewards[index].rewardItems.GetLength(0); i++) {
                                Database.Instance.IncreaseItemQuantity(DBEndRewards.Instance.EasyRewards[index].rewardItems[i], DBEndRewards.Instance.EasyRewards[index].rewardPrices[i]);
                            }
                            UIMessageWindow.Instance.ShowMessage("You gain new reward for Endurance Mode!", 0, UIAction.nothing, true, false);
                            Database.Instance.endRewardsEasy[index] = 1;
                        } else {
                            UIMessageWindow.Instance.ShowMessage("You are unlock Endurance Mode (Normal)!", 0, UIAction.nothing, true, false);
                            Database.Instance.enduranceLevel = 2;
                        }
                    }
                    break;
            }
        }

    }
}
