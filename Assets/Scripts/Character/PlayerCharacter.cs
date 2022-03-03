using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerCharacterAnimController _animController;
    [SerializeField] private projectileLauncher _projectileLauncher;
    [SerializeField] private GameObject _timerPrefab;

    [Header("Customization")]
    [SerializeField] private float _shootingCooldown = 1.5f;
    private bool _canFire = true;


    //UNITY CALLS _____________________________________________________
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _projectileLauncher = GetComponentInChildren<projectileLauncher>();
        _animController = GetComponent<PlayerCharacterAnimController>();
    }

    //MOVEMENT __________________________________________________
    public void Move(Vector2 moveInput)
    {
        _characterMovement.UpdateMoveInput(moveInput);
    }
    public void Jump()
    {
        _characterMovement.Jump();
        _animController.Jump();
    }

    //FIRE __________________________________________
    public void TryFire()
    {
        if (!_canFire) return;
        _projectileLauncher.Shoot();
        _canFire = false;

        InstantiateTimer(_shootingCooldown, true, ResetCanFire);
    }
    public void ResetCanFire()
    {
        _canFire = true;
    }

    //HEALING ____________________________________________________


    //TIMERS _________________________________________
    private void InstantiateTimer(float duration, bool selfdestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(_timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfdestruct);
    }

    public void OnDeath()
    {
        Debug.Log("Player died");
        _animController.OnDeath();
    }
}
