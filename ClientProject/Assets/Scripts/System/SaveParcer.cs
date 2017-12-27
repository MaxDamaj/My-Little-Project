using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace MLA.System {
    public class SaveParcer : MonoBehaviour {

        private const string FILE_NAME = "gameSave.sav";
        private List<string> saveFile;

        public void SaveToFile() {
            string[] saveFiles;
            if (Directory.Exists(Application.persistentDataPath + "/Saves")) {
                saveFiles = Directory.GetFiles(Application.persistentDataPath + "/Saves");
            } else {
                Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
                saveFiles = Directory.GetFiles(Application.persistentDataPath + "/Saves");
            }
            if (saveFiles.GetLength(0) == 0) {
                File.Create(Application.persistentDataPath + "/Saves/" + FILE_NAME).Close();
            }
            saveFile = new List<string>();

            saveFile.Add("---My Little Adventure---");
            saveFile.Add("original story and characters created by Hasbro");
            saveFile.Add("this file contain your current save progress. You can edit this file but do not rename it! Incorrect data may damage your save progress!");
            saveFile.Add("");

            saveFile.Add("--Common--");
            saveFile.Add("tutorialState:" + Database.Instance.tutorialState);
            saveFile.Add("selectedPony:" + Database.Instance.SelectedPony);
            saveFile.Add("cameraShift:" + Database.Instance.cameraShift);
            saveFile.Add("optionBloom:" + Database.Instance.paramBloom);
            saveFile.Add("optionSSAO:" + Database.Instance.paramSSAO);
            saveFile.Add(SaveMultiline(Database.Instance.nowPlaying, "nowPlaying"));
            saveFile.Add(SaveMultiline(Database.Instance.PartyPony, "partyPonies"));
            saveFile.Add("timeSpan:" + Database.Instance.timeSpan);
            saveFile.Add("furnaceSlots:" + Database.Instance.furnaceSlots);
            saveFile.Add("musicVolume:" + Database.Instance.musicVolume);
            saveFile.Add("");

            saveFile.Add("--Statistic--");
            saveFile.Add("totalDistance:" + Database.Instance.distTotal);
            saveFile.Add("distanceInEndurance:" + Database.Instance.distEnd);
            saveFile.Add("distanceInChallenges:" + Database.Instance.distChall);
            saveFile.Add("obstaclesTotal:" + Database.Instance.obstTotal);
            saveFile.Add("obstaclesWithDamage:" + Database.Instance.obstWithDamage);
            saveFile.Add("obstaclesNonDamage:" + Database.Instance.obstNonDamage);
            saveFile.Add("enduranceDistanceEasy:" + Database.Instance.distEndEasy);
            saveFile.Add("enduranceDistanceNormal:" + Database.Instance.distEndNormal);
            saveFile.Add("enduranceDistanceHard:" + Database.Instance.distEndHard);
            saveFile.Add("craftedComponents:" + Database.Instance.craftedComps);
            saveFile.Add("");

            saveFile.Add("--Materials--");
            for (int i = 0; i < Database.Instance.ArrayItemsGetLenght(); i++) {
                saveFile.Add(Database.Instance.GetItemTitle(i) + ":" + Database.Instance.GetItemQuantity(i));
            }
            saveFile.Add("");

            saveFile.Add("--Challenges--");
            saveFile.Add(SaveMultiline(Database.Instance.passedChallenges, "passedChallenges"));
            saveFile.Add("");
            saveFile.Add("--Achievements--");
            saveFile.Add(SaveMultiline(Database.Instance.takenAchievements.ToArray(), "takenAchievements"));
            saveFile.Add("");
            saveFile.Add("--Codex--");
            saveFile.Add(SaveMultiline(Database.Instance.readenCodex.ToArray(), "readenCodex"));
            saveFile.Add("");
            saveFile.Add("--Endurance Rewards--");
            saveFile.Add(SaveMultiline(Database.Instance.endRewardsEasy, "enduranceRewardsEasy"));
            saveFile.Add("");

            saveFile.Add("--Characters--");
            for (int i = 0; i < Database.Instance.ArrayCharFMGetLenght(); i++) {
                saveFile.Add(GetCharStats(i));
            }
            saveFile.Add("");

            saveFile.Add("--Simulation--");
            saveFile.Add("obstaclesLow:" + DBSimulation.Instance.sectionBonusLow);
            saveFile.Add("obstaclesHigh:" + DBSimulation.Instance.sectionBonusHigh);
            saveFile.Add("TestSubjectHP:" + DBSimulation.Instance.simCharacter.HP);
            saveFile.Add("TestSubjectMP:" + DBSimulation.Instance.simCharacter.MP);
            saveFile.Add("TestSubjectSPD:" + DBSimulation.Instance.simCharacter.SPD);
            saveFile.Add("--Simulation Items--");
            for (int i = 0; i < DBSimulation.Instance.ArrayItemsGetLenght(); i++) {
                saveFile.Add(DBSimulation.Instance.GetItemTitle(i) + ":" + DBSimulation.Instance.GetItemQuantity(i));
            }

            //Write save info into file
            File.WriteAllLines(Application.persistentDataPath + "/Saves/" + FILE_NAME, saveFile.ToArray());
            Debug.Log("Current game progress saved in " + FILE_NAME + " file");
        }

        public void LoadFromFile() {
            string[] saveFiles = Directory.GetFiles(Application.persistentDataPath + "/Saves");
            if (saveFiles.GetLength(0) == 0) {
                return;
            }
            saveFile = new List<string>();
            saveFile.AddRange(File.ReadAllLines(saveFiles[0]));

            //Common
            Database.Instance.tutorialState = LoadLineInt(saveFile, "tutorialState", Database.Instance.tutorialState);
            Database.Instance.SelectedPony = LoadLineInt(saveFile, "selectedPony", Database.Instance.SelectedPony);
            Database.Instance.cameraShift = LoadLineFloat(saveFile, "cameraShift", Database.Instance.cameraShift);
            Database.Instance.paramBloom = LoadLineInt(saveFile, "optionBloom", Database.Instance.paramBloom);
            Database.Instance.paramSSAO = LoadLineInt(saveFile, "optionSSAO", Database.Instance.paramSSAO);
            Database.Instance.nowPlaying = LoadMultilineInt(saveFile, "nowPlaying", Database.Instance.nowPlaying);
            Database.Instance.PartyPony = LoadMultilineInt(saveFile, "partyPonies", Database.Instance.PartyPony);
            Database.Instance.timeSpan = LoadLineInt(saveFile, "timeSpan", Database.Instance.timeSpan);
            Database.Instance.furnaceSlots = LoadLineInt(saveFile, "furnaceSlots", Database.Instance.furnaceSlots);
            Database.Instance.musicVolume = LoadLineFloat(saveFile, "musicVolume", Database.Instance.musicVolume);

            //Statistic
            Database.Instance.distTotal = LoadLineInt(saveFile, "totalDistance", Database.Instance.distTotal);
            Database.Instance.distEnd = LoadLineInt(saveFile, "distanceInEndurance", Database.Instance.distEnd);
            Database.Instance.distChall = LoadLineInt(saveFile, "distanceInChallenges", Database.Instance.distChall);
            Database.Instance.obstTotal = LoadLineInt(saveFile, "obstaclesTotal", Database.Instance.obstTotal);
            Database.Instance.obstWithDamage = LoadLineInt(saveFile, "obstaclesWithDamage", Database.Instance.obstWithDamage);
            Database.Instance.obstNonDamage = LoadLineInt(saveFile, "obstaclesNonDamage", Database.Instance.obstNonDamage);
            Database.Instance.distEndEasy = LoadLineInt(saveFile, "enduranceDistanceEasy", Database.Instance.distEndEasy);
            Database.Instance.distEndNormal = LoadLineInt(saveFile, "enduranceDistanceNormal", Database.Instance.distEndNormal);
            Database.Instance.distEndHard = LoadLineInt(saveFile, "enduranceDistanceHard", Database.Instance.distEndHard);
            Database.Instance.craftedComps = LoadLineInt(saveFile, "craftedComponents", Database.Instance.craftedComps);

            //Materials
            for (int i = 0; i < Database.Instance.ArrayItemsGetLenght(); i++) {
                Database.Instance.SetItemQuantity(i, LoadLineFloat(saveFile, Database.Instance.GetItemTitle(i), Database.Instance.GetItemQuantity(i)));
            }

            //Challenges
            Database.Instance.passedChallenges = LoadMultilineInt(saveFile, "passedChallenges", Database.Instance.passedChallenges);
            //Achievements
            Database.Instance.takenAchievements.AddRange(LoadMultilineInt(saveFile, "takenAchievements", Database.Instance.takenAchievements.ToArray()));
            //Codex
            Database.Instance.readenCodex.AddRange(LoadMultilineInt(saveFile, "readenCodex", Database.Instance.readenCodex.ToArray()));
            //Endurance Rewards
            Database.Instance.endRewardsEasy = LoadMultilineInt(saveFile, "enduranceRewardsEasy", Database.Instance.endRewardsEasy);

            //Simulation
            DBSimulation.Instance.sectionBonusLow = LoadLineInt(saveFile, "obstaclesLow", DBSimulation.Instance.sectionBonusLow);
            DBSimulation.Instance.sectionBonusHigh = LoadLineInt(saveFile, "obstaclesHigh", DBSimulation.Instance.sectionBonusHigh);
            DBSimulation.Instance.simCharacter.HP = LoadLineFloat(saveFile, "TestSubjectHP", DBSimulation.Instance.simCharacter.HP);
            DBSimulation.Instance.simCharacter.MP = LoadLineFloat(saveFile, "TestSubjectMP", DBSimulation.Instance.simCharacter.MP);
            DBSimulation.Instance.simCharacter.SPD = LoadLineFloat(saveFile, "TestSubjectSPD", DBSimulation.Instance.simCharacter.SPD);
            for (int i = 0; i < DBSimulation.Instance.ArrayItemsGetLenght(); i++) {
                DBSimulation.Instance.SetItemQuantity(i, LoadLineFloat(saveFile, DBSimulation.Instance.GetItemTitle(i), DBSimulation.Instance.GetItemQuantity(i)));
            }

            //----
            Debug.Log("Game progress loaded from " + FILE_NAME + " file");
        }

        public void ClearFile() {
            File.Delete(Application.persistentDataPath + "/Saves/" + FILE_NAME);
        }

        //------Common-----------------------
        string SaveMultiline(List<string> lines, string lineTitle) {
            string value = lineTitle + ":";
            foreach (string item in lines) { value += (item + ","); }
            return value;
        }
        string SaveMultiline(int[] lines, string lineTitle) {
            string value = lineTitle + ":";
            foreach (int item in lines) { value += (item + ","); }
            return value;
        }
        string SaveMultiline(float[] lines, string lineTitle) {
            string value = lineTitle + ":";
            foreach (float item in lines) { value += (item + ","); }
            return value;
        }
        //-----------------------------------
        int LoadLineInt(List<string> source, string lineTitle, int defaultValue) {
            string line = source.Find(x => x.Contains(lineTitle));
            if (line != null) {
                return int.Parse(line.Split(':')[1]);
            }
            return defaultValue;
        }
        float LoadLineFloat(List<string> source, string lineTitle, float defaultValue) {
            string line = source.Find(x => x.Contains(lineTitle));
            if (line != null) {
                return float.Parse(line.Split(':')[1]);
            }
            return defaultValue;
        }
        List<string> LoadMultiline(List<string> source, string lineTitle, List<string> defaultValue) {
            string line = source.Find(x => x.Contains(lineTitle));
            if (line != null) {
                var list = new List<string>();
                list.AddRange(line.Split(':')[1].Split(','));
                list.RemoveAll(x => x == string.Empty);
                return list;
            }
            return defaultValue;
        }
        int[] LoadMultilineInt(List<string> source, string lineTitle, int[] defaultValue) {
            string line = source.Find(x => x.Contains(lineTitle));
            if (line != null) {
                var list = new List<string>();
                var value = new List<int>();
                list.AddRange(line.Split(':')[1].Split(','));
                list.RemoveAll(x => x == string.Empty);
                foreach (string item in list) {
                    value.Add(int.Parse(item));
                }
                if (defaultValue.GetLength(0) > value.Count) {
                    for (int i = value.Count; i < defaultValue.GetLength(0); i++) {
                        value.Add(0);
                    }
                }
                return value.ToArray();
            }
            return defaultValue;
        }
        //-----------------------------------
        string GetCharStats(int index) {
            CharsFMData character = Database.Instance.GetCharFMInfo(index);
            string line = character.CharName;
            line += ":" + character.HP + "," + character.MP + "," + character.Rank + "," + character.STMCurrent + "," + character.Rank + "," + character.LUCK;
            return line;
        }
        public void LoadCharData() {
            for (int i = 0; i < Database.Instance.ArrayCharFMGetLenght(); i++) {
                List<string> line = LoadMultiline(saveFile, Database.Instance.GetCharFMInfo(i).CharName, null);
                if (line != null) {
                    Database.Instance.SetCharFM_HP(i, float.Parse(line[0]));
                    Database.Instance.SetCharFM_MP(i, float.Parse(line[1]));
                    Database.Instance.SetCharFMRank(i, int.Parse(line[2]));
                    Database.Instance.SetCurrSTM(i, float.Parse(line[3]));
                    Database.Instance.SetCharFMRank(i, int.Parse(line[4]));
                    Database.Instance.SetCharFMLuck(i, float.Parse(line[5]));
                    //Correcting on-time stamina
                    Database.Instance.IncreaseCurrSTM(i, Database.Instance.timeSpan);
                }
            }

        }
    }
}
