using UnityEngine;

public class Settings : MonoBehaviour {
	public static Settings Instance { get; private set; }
	public bool DebugMode;

	public bool UseMouse;
	public bool ReleaseToDrop;
	
	void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}

		ReleaseToDrop = true;
	}

	public void ToggleUseMouse() {
		UseMouse = !UseMouse;
	}
	public void ToggleReleaseToDrop() {
		ReleaseToDrop = !ReleaseToDrop;
	}

	public void OnUseMusicSlider(float val) {
		GameManager.Instance.MusicSound.volume = val;
	}
	public void OnUseEffectsSlider(float val) {
		GameManager.Instance.DropSound.volume = val;
		GameManager.Instance.CombineSound.volume = val;
	}
}
