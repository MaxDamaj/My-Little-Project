using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndRewardController : MonoBehaviour {

    [Header("UI")]
    public RectTransform container;
    public Button[] diffButtons;
    public UIEndReward prefab;

    private List<UIEndReward> rewards;

    void Start() {
        //Gain Rewards for passed distance
        for (int i = 0; i < DBEndRewards.Instance.EasyRewards.GetLength(0); i++) {
            RewardForTask("Easy", i);
        }
        //Show rewards for Easy
        ShowPanels("Easy");
    }

    void ShowPanels(string diff) {
        rewards = new List<UIEndReward>();
        for (int i = 0; i < DBEndRewards.Instance.EasyRewards.GetLength(0); i++) {
            GameObject tmp = Instantiate(prefab.gameObject);
            tmp.transform.SetParent(container);
            tmp.transform.position = Vector3.zero;
            tmp.transform.rotation = Quaternion.identity;
            tmp.transform.localScale = Vector3.one;
            rewards.Add(tmp.GetComponent<UIEndReward>());
            //Set states
            rewards[i].EndDifficulty = diff;
            rewards[i].distance.text = "" + DBEndRewards.Instance.EasyRewards[i].distance;
            rewards[i].distance.color = Database.Instance.endRewardsEasy[i] == 1 ? Database.COLOR_GREEN : Color.white;
            rewards[i].taken.SetActive(Database.Instance.endRewardsEasy[i] == 1);
            rewards[i].rewardContainer.gameObject.SetActive(Database.Instance.endRewardsEasy[i] == 0);
            rewards[i].rewardContainer.text += DBEndRewards.Instance.EasyRewards[i].stage;
            for (int j = 0; j < DBEndRewards.Instance.EasyRewards[i].rewardItems.GetLength(0); j++) {
                rewards[i].rewIcons[j].sprite = Database.Instance.GetItemIcon(DBEndRewards.Instance.EasyRewards[i].rewardItems[j]);
                rewards[i].rewIcons[j].gameObject.SetActive(true);
                rewards[i].rewTexts[j].text = "" + DBEndRewards.Instance.EasyRewards[i].rewardPrices[j];
                rewards[i].rewTexts[j].gameObject.SetActive(true);
            }
        }
    }

    void RewardForTask(string diff, int index) {
        switch (diff) {
            case "Easy":
                if (Database.Instance.distEndEasy >= DBEndRewards.Instance.EasyRewards[index].distance && Database.Instance.endRewardsEasy[index] == 0) {
                    if (DBEndRewards.Instance.EasyRewards[index].stage == "") {
                        for (int i = 0; i < DBEndRewards.Instance.EasyRewards[index].rewardItems.GetLength(0); i++) {
                            Database.Instance.IncreaseItemQuantity(DBEndRewards.Instance.EasyRewards[index].rewardItems[i], DBEndRewards.Instance.EasyRewards[index].rewardPrices[i]);
                        }
                        UIMessageWindow.Instance.ShowMessage("You gain new reward for Endurance Mode!", 0, UIAction.nothing, true, false);
                        Database.Instance.endRewardsEasy[index] = 1;
                    }
                }
                break;
        }
    }

}
