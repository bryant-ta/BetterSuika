using UnityEngine;

public class OutOfBounds : MonoBehaviour {
	void OnTriggerStay2D(Collider2D other) {
		if (other.TryGetComponent(out Guy guy) && guy.Haslanded) {
			GameManager.Instance.LoseGame();
		}
	}
}
