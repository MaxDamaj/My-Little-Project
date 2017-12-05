using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DatabaseManager : MonoBehaviour {

    [Header("Items Containers")]
    public Transform itemsInventory;
    public Transform[] itemsBelt;
    public Transform[] itemsFurnace;

    private SaveParcer parser;

    void Start() {
        parser = GetComponent<SaveParcer>();
        MusicManager.Instance.SetFolder("Music/Menu", 0);
        if (!Database.Instance.IsLoaded) {
            LoadState();
            Database.Instance.IsLoaded = true;
            if (Database.Instance.tutorialState <= 1)
                UnityEngine.SceneManagement.SceneManager.LoadScene("codex");
        } else {
            SaveState();
        }
    }

    //Loading State
    void LoadState() {
        Database.Instance.takenAchievements = new List<int>();
        Database.Instance.readenCodex = new List<int>();

        parser.LoadFromFile();

        for (int i = Database.Instance.takenAchievements.Count; i < DBAchievements.Instance.GetAchievementsCount(); i++) {
            Database.Instance.takenAchievements.Add(0);
        }
        for (int i = Database.Instance.readenCodex.Count; i < GlobalData.Instance.codexPagesCount; i++) {
            Database.Instance.readenCodex.Add(0);
        }

        //Time span calculate
        DateTime startDate = new DateTime(2016, 1, 1);
        DateTime currDate = DateTime.Now;
        TimeSpan elapsedSpan = new TimeSpan(currDate.Ticks - startDate.Ticks);
        Database.Instance.timeSpan = (int)elapsedSpan.TotalSeconds - Database.Instance.timeSpan;
        //Characters
        parser.LoadCharData();
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

    void OnApplicationQuit() {
        SaveState();
    }
}
