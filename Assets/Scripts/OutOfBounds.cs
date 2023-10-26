using UnityEngine;

public class OutOfBounds : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Guy guy) && guy.Haslanded) {
			GameManager.Instance.LoseGame();
		}
	}
}
