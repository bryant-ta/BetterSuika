using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _leftBound;
    [SerializeField] float _rightBound;

    [SerializeField] Guy _heldGuy;
    [SerializeField] Transform _heldGuyLocation;

    bool _canInput;
    bool _canDrop;

    Guy _lastHeldGuy;

    void Start() {
        ReadyNextGuy();

        _canInput = true;
        _canDrop = true;
    }

    void Update() {
        if (!_canInput) return;
        
        // Movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Keep in bounds
        float positionX = Mathf.Clamp(transform.position.x, _leftBound, _rightBound);
        transform.position = new Vector3(positionX, transform.position.y, transform.position.z);

        if (_canDrop && Input.GetKey(KeyCode.Space)) {
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
        _heldGuy.transform.SetParent(_heldGuyLocation);
        _heldGuy.transform.localPosition = Vector3.zero;
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