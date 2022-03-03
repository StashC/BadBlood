using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _jumpParameterName = "Jump";
    [SerializeField] private string _onDeathParameterName = "OnDeath";
    [SerializeField] private string _velocityXParameterName = "VelocityX";
    [SerializeField] private string _isGroundedParameterName = "IsGrounded";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        _animator.SetTrigger(_jumpParameterName);
    }

    public void SetVelocity(Vector2 velocity)
    {
        _animator.SetFloat(_velocityXParameterName, velocity.x);
    }

    public void SetIsGrounded(bool isGrounded)
    {
        _animator.SetBool(_isGroundedParameterName, isGrounded);
    }

    public void OnDeath()
    {
        _animator.SetTrigger(_onDeathParameterName);
    }
}
