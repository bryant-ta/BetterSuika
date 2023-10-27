using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _leftBoundBase;
    [SerializeField] float _rightBoundBase;
    float _guyBoundOffset; // based on length of held Guy

    [SerializeField] Guy _heldGuy;

    bool _canInput;
    bool _canDrop;

    Guy _lastHeldGuy;

    void Start() {
        ReadyNextGuy();

        _canInput = true;
        _canDrop = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _canInput = !UIManager.Instance.TogglePauseMenu();
        }
        
        if (!_canInput) return;
        
        // Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Keep in bounds
        float positionX = Mathf.Clamp(transform.position.x, _leftBoundBase + _guyBoundOffset, _rightBoundBase - _guyBoundOffset);
        transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

        if (_canDrop && Input.GetButton("Fire1")) {
            DropGuy();
        }
    }

    public void DropGuy() {
        _lastHeldGuy = _heldGuy;
        _canDrop = false;
        _lastHeldGuy.OnLanded += AllowDrop;
        
        _heldGuy.Drop();
        _heldGuy.transform.SetParent(null);
        _heldGuy = null;
        
        ReadyNextGuy();
    }

    public void ReadyNextGuy() {
        _heldGuy = GameManager.Instance.GetNextGuy();
        _heldGuy.transform.SetParent(transform);
        _heldGuy.transform.localPosition = Vector3.zero;
        _guyBoundOffset = _heldGuy.GetComponent<CircleCollider2D>().radius;
    }

    void AllowDrop() {
        _canDrop = true;
        if (_lastHeldGuy) {
            _lastHeldGuy.OnLanded -= AllowDrop;
            _lastHeldGuy = null;
        }
    }

    // prob subscribe to GameManager end game event
    public void ResetState() {
        AllowDrop();
        _canInput = true;
        
        ReadyNextGuy();
        ReadyNextGuy(); // Do twice to guarantee load up of two guys
    }
}