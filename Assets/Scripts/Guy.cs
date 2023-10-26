using System;
using UnityEngine;

public class Guy : MonoBehaviour {
    [Tooltip("Start at 0")]
    public int GuyId;

    Rigidbody2D _rb;

    public bool IsCombined; // used so combine func doesnt get called twice, from the two guys involved in collision

    void Awake() { _rb = GetComponent<Rigidbody2D>(); }

    // Return true if successfully combined
    public void Combine(Guy otherGuy) {
        if (GuyId == otherGuy.GuyId) { // combine with same Guy type
            if (!otherGuy.IsCombined) {
                IsCombined = true;
                
                GameObject higherGuyObj = GameManager.Instance.LookUpHigherGuyObj(GuyId);
                if (higherGuyObj != null) {
                    GameObject guyObj = Instantiate(GameManager.Instance.LookUpHigherGuyObj(GuyId), transform.position, transform.rotation);
                    guyObj.GetComponent<Collider2D>().enabled = true;
                    guyObj.GetComponent<Rigidbody2D>().gravityScale = 1f;
                }
            }
            
            Destroy(gameObject);
        }
    }

    public void Drop() {
        _rb.gravityScale = 1f;
        GetComponent<Collider2D>().enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.TryGetComponent(out Guy guy)) {
            Combine(guy);
        }
    }
}