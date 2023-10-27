using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField] Player _player;

    // Guys
    Guy _nextGuy;
    [SerializeField] Transform _nextGuyLocation;

    [SerializeField] List<GameObjectWeight> _guyData = new();
    RollTable<GameObject> _guyRoller = new();

    [Header("Debug")]
    [SerializeField] bool _debugMode;
    [SerializeField] List<GameObjectWeight> _debugGuyData = new();

    // Scoring
    public int Score { get; private set; }
    List<int> _highScores = new();
    HighScoreManager _hsm = new();

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        // Ensure guy data is sorted since LookUpHigherGuyObj() relies on index position
        //   Could replace with storing Guy directly and creating objects with factory
        _guyData.Sort((a, b) => a.obj.GetComponent<Guy>().GuyId.CompareTo(b.obj.GetComponent<Guy>().GuyId));

        if (_debugMode) {
            foreach (GameObjectWeight entry in _debugGuyData) {
                _guyRoller.Add(entry.obj, entry.weight);
            }
        } else {
            foreach (GameObjectWeight entry in _guyData) {
                _guyRoller.Add(entry.obj, entry.weight);
            }
        }
    }

    void Start() {
        LoadHighScores();
    }

    public void LoseGame() {
        print("LOST");
        TrySaveHighScore(Score); 
        ResetGame();
    }

    public void ResetGame() {
        GameObject[] guyObjs = GameObject.FindGameObjectsWithTag("Guy");
        foreach (GameObject guyObj in guyObjs) {
            Destroy(guyObj);
        }
        
        Score = 0;
        UIManager.Instance.UpdateScoreText(0);
        _player.ResetState();
    }

    #region Guys

    // Generate next Guy for player to drop
    public Guy GetNextGuy() {
        // first time generation
        if (_nextGuy == null) {
            GameObject firstGuyObj = Instantiate(_guyRoller.GetRandom(), _nextGuyLocation);
            _nextGuy = firstGuyObj.GetComponent<Guy>();
        }

        GameObject guyObj = Instantiate(_guyRoller.GetRandom(), _nextGuyLocation);
        Guy curGuy = _nextGuy;
        _nextGuy = guyObj.GetComponent<Guy>();

        return curGuy;
    }

    // Look up next tier of guy when guys combine
    public GameObject LookUpHigherGuyObj(int guyId) { return guyId + 1 < _guyData.Count ? _guyData[guyId + 1].obj : null; }

    #endregion

    #region Scoring

    public void AddScore(int val) {
        Score += val;
        UIManager.Instance.UpdateScoreText(Score);
    }

    void TrySaveHighScore(int score) {
        for (int i = 0; i < _highScores.Count; i++) {
            if (score > _highScores[i]) {
                _highScores.Insert(i, score);
                break;
            }
        }

        if (_highScores.Count > 3) { // clean up lowest score
            _highScores.RemoveAt(3);
            
            // Write to disk
            _hsm.SaveHighScore(_highScores);
        }

        UIManager.Instance.UpdateHighScoresText(_highScores);
    }

    // Inits high scores list with 0 values if no previous high scores files
    void LoadHighScores() {
        List<int> highScores = _hsm.LoadHighScores();
        if (highScores == null) {
            _highScores.AddRange(new List<int>() {0, 0, 0});
        } else {
            _highScores = highScores;
        }
        
        UIManager.Instance.UpdateHighScoresText(_highScores);
    }

    #endregion
}