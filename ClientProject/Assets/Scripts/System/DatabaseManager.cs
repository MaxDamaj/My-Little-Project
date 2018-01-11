using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MLA.UI.Windows;
using MLA.UI.Controllers;

namespace MLA.System.Controllers {
    public class DatabaseManager : MonoBehaviour {

        private SaveParcer parser;

        void Start() {
            parser = GetComponent<SaveParcer>();
            MusicManager.Instance.SetFolder("Music/Menu", 0);
            GlobalData.Instance.gameState = GameModeState.MainMenu;
            if (!Database.Instance.IsLoaded) {
                LoadState();
                Database.Instance.IsLoaded = true;
                if (Database.Instance.tutorialState <= 1)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("codex");
            } else {
                SaveState();
            }

            //Initializing Windows
            GlobalData.Instance.IsSimulation = false;
            MenuNavigation.Instance.Init();
            FindObjectOfType<UIOptionWindow>().Init();
            AchievementsController.Instance.Init(true);
            FindObjectOfType<UIEndurance>().Init();
        }

        //Loading State
        void LoadState() {
            Database.Instance.takenAchievements = new List<int>();
            Database.Instance.readenCodex = new List<int>();
            Database.Instance.endRewardsEasy = new List<int>();
            Database.Instance.endRewardsNormal = new List<int>();
            Database.Instance.endRewardsHard = new List<int>();

            parser.LoadFromFile();

            for (int i = Database.Instance.takenAchievements.Count; i < DBAchievements.Instance.GetAchievementsCount(); i++) { Database.Instance.takenAchievements.Add(0); }
            for (int i = Database.Instance.readenCodex.Count; i < GlobalData.Instance.codexPagesCount; i++) { Database.Instance.readenCodex.Add(0); }
            for (int i = Database.Instance.endRewardsEasy.Count; i < DBEndRewards.Instance.EasyRewards.GetLength(0); i++) { Database.Instance.endRewardsEasy.Add(0); }
            for (int i = Database.Instance.endRewardsNormal.Count; i < DBEndRewards.Instance.NormalRewards.GetLength(0); i++) { Database.Instance.endRewardsNormal.Add(0); }
            for (int i = Database.Instance.endRewardsHard.Count; i < DBEndRewards.Instance.HardRewards.GetLength(0); i++) { Database.Instance.endRewardsHard.Add(0); }

            //Time span calculate
            DateTime startDate = new DateTime(2016, 1, 1);
            DateTime currDate = DateTime.Now;
            TimeSpan elapsedSpan = new TimeSpan(currDate.Ticks - startDate.Ticks);
            Database.Instance.timeSpan = (int)elapsedSpan.TotalSeconds - Database.Instance.timeSpan;
            //Characters
            parser.LoadCharData();
            SetStoryLevel();
        }

        //Save State
        void SaveState() {
            //Timespan write
            DateTime startDate = new DateTime(2016, 1, 1);
            DateTime currDate = DateTime.Now;
            TimeSpan elapsedSpan = new TimeSpan(currDate.Ticks - startDate.Ticks);
            Database.Instance.timeSpan = (int)elapsedSpan.TotalSeconds;
            //Parse
            parser.SaveToFile();
        }

        //Clear State
        public void ClearState() {
            parser.ClearFile();
        }

        public void SetStoryLevel() {
            if (Database.Instance.GetItemQuantity("Cyclotrone") > 0) Database.Instance.SetStoryLevel(1);
            if (Database.Instance.GetItemQuantity("Generator") > 0) Database.Instance.SetStoryLevel(2);
            if (Database.Instance.GetItemQuantity("Static Cells Block") > 0) Database.Instance.SetStoryLevel(3);
            if (Database.Instance.GetItemQuantity("Field Compensator") > 0) Database.Instance.SetStoryLevel(4);
            if (Database.Instance.GetItemQuantity("Transformer") > 0) Database.Instance.SetStoryLevel(5);
        }

        void OnApplicationQuit() {
            SaveState();
        }
    }
}
