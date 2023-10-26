using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _bound;

    [SerializeField] Guy _heldGuy;
    [SerializeField] Transform _heldGuyLocation;

    void Start() {
        ReadyNextGuy();
    }

    void Update() {
        // Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Keep in bounds
        float positionX = Mathf.Clamp(transform.position.x, -_bound, _bound);
        transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space)) {
            DropGuy();
        }
    }

    public void DropGuy() {
        _heldGuy.Drop();
        _heldGuy.transform.SetParent(null);
        _heldGuy = null;
        
        ReadyNextGuy();
    }

    public void ReadyNextGuy() {
        _heldGuy = GameManager.Instance.GetNextGuy();
        _heldGuy.transform.SetParent(_heldGuyLocation);
        _heldGuy.transform.localPosition = Vector3.zero;
        
    }
}