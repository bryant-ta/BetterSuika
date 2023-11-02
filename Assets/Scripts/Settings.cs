using UnityEngine;

public class Settings : MonoBehaviour {
	public static Settings Instance { get; private set; }

	public bool UseMouse;
	public bool DebugMode;
	
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
}
