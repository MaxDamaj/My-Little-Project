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

        parser.LoadFromFile();

        for (int i = Database.Instance.takenAchievements.Count; i < DBAchievements.Instance.GetAchievementsCount(); i++) {
            Database.Instance.takenAchievements.Add(0);
        }

        LoadItems();
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
        SaveItems();
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

    //-----------------------------------------------------------
    void LoadItems() {
        foreach (string item in Database.Instance.itemsInventory) {
            if (item != "") {
                GameObject tmp = Instantiate(Database.Instance.GetUsableItem(item).prefab);
                tmp.transform.SetParent(itemsInventory);
                tmp.transform.localScale = Vector3.one;
            }
        }
        for (int i = 0; i < Database.Instance.itemsBelt.Count; i++) {
            string item = Database.Instance.itemsBelt[i];
            if (item != "") {
                GameObject tmp = Instantiate(Database.Instance.GetUsableItem(item).prefab);
                tmp.transform.SetParent(itemsBelt[i]);
                tmp.transform.localScale = Vector3.one;
            }
        }
        for (int i = 0; i < Database.Instance.itemsFurnace.Count; i++) {
            string item = Database.Instance.itemsFurnace[i];
            if (item != "") {
                GameObject tmp = Instantiate(Database.Instance.GetUsableItem(item).prefab);
                tmp.transform.SetParent(itemsFurnace[i]);
                tmp.transform.localScale = Vector3.one;
            }
        }
    }
    void SaveItems() {
        Database.Instance.itemsInventory = new List<string>();
        Database.Instance.itemsBelt = new List<string>();
        Database.Instance.itemsFurnace = new List<string>();
        for (int i = 0; i < itemsInventory.childCount; i++) {
            Database.Instance.itemsInventory.Add(itemsInventory.GetChild(i).GetComponent<CraftComponent>().title);
        }
        for (int i = 0; i < itemsBelt.GetLength(0); i++) {
            if (itemsBelt[i].childCount != 0) {
                Database.Instance.itemsBelt.Add(itemsInventory.GetChild(0).GetComponent<CraftComponent>().title);
            }
        }
        for (int i = 0; i < itemsFurnace.GetLength(0); i++) {
            if (itemsFurnace[i].childCount != 0) {
                if (itemsFurnace[i].GetChild(0).GetComponent<CraftComponent>().IsItem) {
                    Database.Instance.itemsFurnace.Add(itemsInventory.GetChild(0).GetComponent<CraftComponent>().title);
                }
            }
        }
    }

    void OnApplicationQuit() {
        SaveState();
    }
}
