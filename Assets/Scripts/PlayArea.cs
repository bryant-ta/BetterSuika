using UnityEngine;

public class PlayArea : MonoBehaviour {
	void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Guy guy) && guy.Destroyed) GameManager.Instance.LoseGame();
	}
}
