using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private BatMode _batMode;

    [Header("Customization")]
    [SerializeField] private float _maxSpeed = 4;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _jumpForce = 100f;
    [SerializeField] private float _batModeForce = 100f;

    [Header("Feedback")]
    [SerializeField] private Vector2 _movementInput;


    private bool _isGrounded;


    //UNITY METHODS _________________________________

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _batMode = GetComponentInChildren<BatMode>();
    }

    private void Update()
    {
        UpdateIsGrounded();
    }
    private void FixedUpdate()
    {
        ApplyMovementPhysics();
    }

    //CUSTOM METHODS ________________________________
    public void UpdateMoveInput(Vector2 input)
    {
        _movementInput = input;
    }
    public void Jump()
    {
        ApplyJumpPhysics();
    }
    private void UpdateIsGrounded()
    {
        _isGrounded = _groundCheck.GetIsGrounded();
    }

    private void ApplyMovementPhysics()
    {
        Vector2 horizontalMoveInput = new Vector2(_movementInput.x, 0f);
        Vector2 moveForce = horizontalMoveInput * _acceleration;

        if (Mathf.Abs(_rigidBody.velocity.x + moveForce.x*Time.fixedDeltaTime) < _maxSpeed){
            _rigidBody.AddForce(horizontalMoveInput * _acceleration);
        }
    }

    private void ApplyJumpPhysics()
    {
        if (!_isGrounded)
        {
            if (_batMode.CheckBatModeAvailable())
            {
                _batMode.ActivateBatMode();
                _rigidBody.velocity = Vector2.zero;
                Vector2 batMovement = new Vector2(_movementInput.x, 0.5f);
                _rigidBody.AddForce(batMovement * _batModeForce);
            }
            return;
        }
        _rigidBody.AddForce(new Vector2(0, _jumpForce));
    }
}
