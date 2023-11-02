using UnityEngine;

public class Settings : MonoBehaviour {
	public static Settings Instance { get; private set; }
	public bool DebugMode;

	public bool UseMouse;
	
	void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}

	public void ToggleUseMouse() {
		UseMouse = !UseMouse;
	}

	public void OnUseMusicSlider(float val) {
		GameManager.Instance.MusicSound.volume = val;
	}
	public void OnUseEffectsSlider(float val) {
		GameManager.Instance.DropSound.volume = val;
		GameManager.Instance.CombineSound.volume = val;
	}
}
