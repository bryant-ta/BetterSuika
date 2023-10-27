using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScoreManager {
    string savePath = "high_scores.json";

    public void SaveHighScore(List<int> highScores) {
        HighScoreData data = new HighScoreData {HighScores = highScores};
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, jsonData);
    }

    public List<int> LoadHighScores() {
        if (File.Exists(savePath)) {
            string jsonData = File.ReadAllText(savePath);
            return JsonUtility.FromJson<HighScoreData>(jsonData).HighScores;
        }

        return null;
    }
}

[Serializable]
public class HighScoreData {
    public List<int> HighScores = new();
}