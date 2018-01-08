using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLA.System.Controllers {
    public class GameController : MonoBehaviour {

        private static GameController _controller;

        #region API

        public static GameController Instance {
            get {
                if (_controller == null) {
                    _controller = FindObjectOfType<GameController>();
                }
                return _controller;
            }
        }

        #endregion

        #region Actions

        public void BuyCharacter(int ID) {
            Database.Instance.SetCharFMRank(ID, 0);
            CharsFMData pony = Database.Instance.GetCharFMInfo(ID);
            for (int i = 0; i < pony.costPrises.GetLength(0); i++) {
                Database.Instance.IncreaseItemQuantity(pony.costItems[i], 0 - pony.costPrises[i]);
            }
            AchievementsController.Instance.Init(false);
        }

        public void StartEndurance(Difficulty diff) {
            GlobalData.Instance.difficulty = diff;
            UnityEngine.SceneManagement.SceneManager.LoadScene("road_endurance");
        }

        #endregion

    }
}
