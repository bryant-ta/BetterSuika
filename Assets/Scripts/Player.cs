using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _leftBoundBase;
    [SerializeField] float _rightBoundBase;
    float _guyBoundOffset; // based on length of held Guy

    [SerializeField] Guy _heldGuy;
    Guy _lastHeldGuy;
    
    [HideInInspector] public bool CanInput;
    bool _canDrop;

    Camera _mainCamera;
    
    void Awake() {
        _mainCamera = Camera.main;
    }

    void Start() {
        ReadyNextGuy();

        CanInput = true;
        _canDrop = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UIManager.Instance.TogglePauseMenu();
        }
        
        if (!CanInput) return;
        
        // Movement
        if (Settings.Instance.UseMouse) { // Mouse movement
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(mousePosition.x, transform.localPosition.y, transform.localPosition.z);
        } else { // Keyboard movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            Vector3 movement = new Vector3(horizontalInput, 0, 0) * _moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
        
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
        _lastHeldGuy.OnLanded += ReadyNextGuy;
        
        _heldGuy.Drop();
        _heldGuy.transform.SetParent(null);
        _heldGuy = null;
        
        GameManager.Instance.DropSound.Play();
    }

    public void ReadyNextGuy() {
        _heldGuy = GameManager.Instance.GetNextGuy();
        _heldGuy.transform.SetParent(transform);
        _heldGuy.transform.localPosition = Vector3.zero;
        // _guyBoundOffset = _heldGuy.GetComponent<CircleCollider2D>().radius * _heldGuy.transform.localScale.x;
    }

    void AllowDrop() {
        _canDrop = true;
        if (_lastHeldGuy) {
            _lastHeldGuy.OnLanded -= AllowDrop;
            _lastHeldGuy.OnLanded -= ReadyNextGuy;
            _lastHeldGuy = null;
        }
    }

    // prob subscribe to GameManager end game event
    public void ResetState() {
        AllowDrop();
        CanInput = true;
        
        ReadyNextGuy();
        ReadyNextGuy(); // Do twice to guarantee load up of two guys
    }
}