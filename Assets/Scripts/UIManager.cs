using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager Instance { get; private set; }
	
	[SerializeField] TextMeshProUGUI _scoreText;
	
	[SerializeField] TextMeshProUGUI _firstPlaceScoreText;
	[SerializeField] TextMeshProUGUI _secondPlaceScoreText;
	[SerializeField] TextMeshProUGUI _thirdPlaceScoreText;
	
	void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}

	public void UpdateScoreText(int val) {
		_scoreText.text = val.ToString();
	}

	public void UpdateHighScoresText(List<int> highScores) {
		if (highScores.Count != 3) Debug.LogError("High scores list does not have 3 elements.");
		
		_firstPlaceScoreText.text = highScores[0].ToString();
		_secondPlaceScoreText.text = highScores[1].ToString();
		_thirdPlaceScoreText.text = highScores[2].ToString();
	}
}
