using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DatabaseManager : MonoBehaviour {

    [Header("Items Containers")]
    public Transform itemsInventory;
    public Transform[] itemsBelt;
    public Transform[] itemsFurnace;

    [Header("Card Game containers")]
    public Transform playerDeck;
    public Transform playerStack;

    private SaveParcer parser;

    void Start() {
        parser = GetComponent<SaveParcer>();
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
        parser.LoadFromFile();
        LoadCardsOnDeck();
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
        UpdatePlayerDeck();
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
    void LoadCardsOnDeck() {
        foreach (string item in Database.Instance.playerDeck) {
            if (item != "") {
                GameObject tmp = Instantiate(Resources.Load<GameObject>("Cards/" + item));
                tmp.transform.SetParent(playerDeck);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.rotation = playerDeck.rotation;
                tmp.name = item;
            }
        }
        foreach (string item in Database.Instance.playerStack) {
            if (item != "") {
                GameObject tmp = Instantiate(Resources.Load<GameObject>("Cards/" + item));
                tmp.transform.SetParent(playerStack);
                tmp.transform.localScale = Vector3.one;
                tmp.transform.rotation = playerStack.rotation;
                tmp.name = item;
            }
        }
    }
    void UpdatePlayerDeck() {
        if (playerDeck.childCount == 0) {
            LoadCardsOnDeck();
        }
        Database.Instance.playerDeck = new List<string>();
        Database.Instance.playerStack = new List<string>();
        for (int i = 0; i < playerDeck.childCount; i++) {
            Database.Instance.playerDeck.Add(playerDeck.GetChild(i).name);
        }
        for (int i = 0; i < playerStack.childCount; i++) {
            Database.Instance.playerStack.Add(playerStack.GetChild(i).name);
        }
    }
    void LoadItems() {
        foreach (string item in Database.Instance.itemsInventory) {
            if (item != "") {
                GameObject tmp = Instantiate(Database.Instance.GetUsableItem(item).prefab);
                tmp.transform.SetParent(itemsInventory);
                tmp.transform.localScale = Vector3.one;
            }
        }
        for (int i=0; i< Database.Instance.itemsBelt.Count; i++) {
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
