using System;
using UnityEngine;

public class Guy : MonoBehaviour {
    [Tooltip("Start at 0")]
    public int GuyId;
    [SerializeField] int _scoreValue;

    Rigidbody2D _rb;

    bool _isCombined;                            // used so combine func doesnt get called twice, from the two guys involved in collision
    public bool Haslanded { get; private set; } // used when checking out of bounds
    public Action OnLanded;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Return true if successfully combined
    public void TryCombine(Guy otherGuy) {
        if (GuyId == otherGuy.GuyId) {   // combine with same Guy type
            if (!otherGuy._isCombined) { // below happens once only
                _isCombined = true;
                
                // Create next higher Guy
                GameObject higherGuyObj = GameManager.Instance.LookUpHigherGuyObj(GuyId);
                if (higherGuyObj != null) {
                    Vector3 spawnPos = (transform.position + otherGuy.transform.position) / 2; // spawn at middle point
                    GameObject guyObj = Instantiate(GameManager.Instance.LookUpHigherGuyObj(GuyId), spawnPos, transform.rotation);
                    guyObj.GetComponent<Collider2D>().enabled = true;
                    guyObj.GetComponent<Rigidbody2D>().gravityScale = 1f;
                }
                
                // Gain points
                GameManager.Instance.AddScore(_scoreValue * 2); // adding both Guy scores
                
                GameManager.Instance.CombineSound.Play();
            }
            
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Drop() {
        _rb.gravityScale = 1f;
        GetComponent<Collider2D>().enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.TryGetComponent(out Guy guy)) {
            TryCombine(guy);
            Landed();
        } else if (col.collider.CompareTag("Floor")) {
            Landed();
        }
    }

    void Landed() {
        if (Haslanded) return;
        Haslanded = true;
        OnLanded?.Invoke();
    }
}